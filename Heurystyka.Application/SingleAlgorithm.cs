using Heurystyka.Domain;
using Heurystyka.Domain.Wymagania;
using Heurystyka.Infrastructure;
using iText.Commons.Actions.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Heurystyka.Application
{
    public class SingleAlgorithm
    {
        private readonly DataContext dataContext;
        private readonly StateMonitor _stateMonitor;
        public ReportMultiple Report { get; set; } = new ReportMultiple();
        public SingleAlgorithm (DataContext dataContext, StateMonitor stateMonitor)
        {
            this.dataContext = dataContext;
            _stateMonitor = stateMonitor;
        }
        public async Task<ReportMultiple> ExecuteOptimizationAsync(OptimizationRequest request)
        {
            _stateMonitor.UpdateState("Start");
            await EnsureMaxReportsLimitAsync();
            Report.CreatedAt  = DateTime.UtcNow;
            Report.Reports = new List<ReportSingle>();
            await dataContext.ReportMultiples.AddAsync(Report);
            await dataContext.SaveChangesAsync();

            foreach (var (functionString, index) in request.TestFunctionNames.Select((func, idx) => (func, idx)))
            {
                fitnessFunction fitnessFunction = OptionsService.GetTestFunctionNames()[functionString];
                var domain = request.domain[index];

                List<IEnumerable<double>> paramRange= new List<IEnumerable<double>>();
                for (var i = 0; i < request.ParameterRanges.Count; i++)
                {
                    List<double> par = new List<double>();
                    for (var x = request.ParameterRanges[i][0]; x <= request.ParameterRanges[i][1]; x += request.ParameterRanges[i][2])
                    {
                        par.Add(x);
                    }
                    paramRange.Add(par);
                }
                var cartesianProduct = CartesianProduct(paramRange);

                foreach (var combination in cartesianProduct)
                {
                    var results = new List<double>();
                    var xbestList = new List<double[]>();
                    var algorithm = OptionsService.GetAlgorithms()[request.AlgorithmName];
                    for (int i = 0; i < request.repetitions[index]; i++)
                    {
                        _stateMonitor.UpdateState($"Obecnie wykonywany: {functionString} dla parametrów: {string.Join(", ", combination)} Powtórzenie nr.{i+1}");
                        var result = await Task.Run(() => algorithm.Solve(
                            fitnessFunction,
                            ConvertTo2DArray(domain),
                            request.Size,
                            request.Iteration,
                            request.Dimensions,
                            request.load,
                            combination.ToArray()));
                        results.Add(result);
                        xbestList.Add(algorithm.XBest);
                    }
                    int bestIndex = results.IndexOf(results.Min());
                    double[] xBest = xbestList[bestIndex];
                    var reportSingle = new ReportSingle
                    {
                        XBest = string.Join(", ", xBest),
                        FBest = results[bestIndex],
                        Parameters = string.Join(", ", combination),
                        AlgorithmName = request.AlgorithmName,
                        AlgorithmFunction = functionString
                    };
                    Report.Reports.Add(reportSingle);
                    await UpdateReportAsync(Report);
                }
               
            }
            _stateMonitor.UpdateState("Koniec");
            return Report;
        }

        static IEnumerable<IEnumerable<double>> CartesianProduct(List<IEnumerable<double>> sets)
        {
            if (sets.Count == 1)
            {
                return sets[0].Select(item => new[] { item });
            }


            var firstSet = sets[0];
            var restProduct = CartesianProduct(sets.Skip(1).ToList());

            return firstSet.SelectMany(
                item => restProduct,
                (item, rest) => new[] { item }.Concat(rest)
            );
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

