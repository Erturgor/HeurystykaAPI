using Heurystyka.Domain;
using Heurystyka.Domain.Wymagania;
using Heurystyka.Infrastructure;
using iText.Commons.Actions.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Application
{
    public class MultipleAlgorithms
    {
        private readonly DataContext dataContext;
        private readonly StateMonitor _stateMonitor;
        public ReportMultiple Report { get; set; }
        public MultipleAlgorithms(DataContext dataContext, StateMonitor stateMonitor)
        {
            this.dataContext = dataContext;
            _stateMonitor = stateMonitor;
        }

        public async Task<ReportMultiple> ExecuteOptimizationAsync(BestRequest request)
        {
            _stateMonitor.UpdateState("Start");
            await EnsureMaxReportsLimitAsync();
            Report = new ReportMultiple
            {
                CreatedAt = DateTime.UtcNow,
                Reports = new List<ReportSingle>()
            };
            foreach (var (functionString, index) in request.AlgorithmNames.Select((func, idx) => (func, idx)))
            {
                var results = new List<double>();
                var xbestList = new List<double[]>();
                var algorithm = OptionsService.GetAlgorithms()[functionString];
                var fitnessFunction = OptionsService.GetTestFunctionNames()[request.TestFunctionName];
                double[] parameters = new double[0];
                AlgorithmResult algorithmResult = await dataContext.AlgorithmResults.FirstOrDefaultAsync(ar => ar.AlgorithmName == request.TestFunctionName);
                if (algorithmResult != null) { 
                    parameters = algorithmResult.Parameters.Select(p => p.ParameterValue).ToArray();
                }
                for (int i = 0; i < request.repetitions[index]; i++)
                {
                    _stateMonitor.UpdateState($"Obecnie wykonywany: {functionString} dla parametrów:  {string.Join(", ", parameters)} Powtórzenie nr.{i + 1}");
                    var result = await Task.Run(() => algorithm.Solve(
                        fitnessFunction,
                        ConvertTo2DArray(request.domain),
                        request.Size,
                        request.Iteration,
                        request.Dimensions,
                        request.load,
                        parameters));
                    results.Add(result);
                    xbestList.Add(algorithm.XBest);
                }
                int bestIndex = results.IndexOf(results.Min());
                double[] xBest = xbestList[bestIndex];
                var reportSingle = new ReportSingle
                {
                    XBest = string.Join(", ", xBest),
                    FBest = results[bestIndex],
                    AlgorithmName = functionString,
                    AlgorithmFunction = request.TestFunctionName
                };
                Report.Reports.Add(reportSingle);
                await UpdateReportAsync(Report);

            }
            _stateMonitor.UpdateState("Koniec");
            return Report;
        }
        static double[,] ConvertTo2DArray(double[][] jagged)
        {
            int rows = jagged.Length;
            int cols = jagged[0].Length;

            double[,] result = new double[rows, cols];

            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    result[i, j] = jagged[i][j];
                }
            }

            return result;
        }
        private async Task EnsureMaxReportsLimitAsync()
        {
            var reportsToDelete = await dataContext.ReportMultiples
                .OrderByDescending(r => r.CreatedAt)
                .Skip(10)
                .ToListAsync();

            if (reportsToDelete.Any())
            {
                dataContext.ReportMultiples.RemoveRange(reportsToDelete);
                await dataContext.SaveChangesAsync();
            }
        }
        private async Task UpdateReportAsync(ReportMultiple reportMultiple)
        {
            var existingReport = await dataContext.ReportMultiples
                .Include(r => r.Reports)
                .FirstOrDefaultAsync(r => r.CreatedAt == reportMultiple.CreatedAt);

            if (existingReport != null)
            {
                existingReport.Reports = reportMultiple.Reports;
                dataContext.ReportMultiples.Update(existingReport);
                await dataContext.SaveChangesAsync();
            }
        }
    }
}
