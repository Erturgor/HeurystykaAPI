using Heurystyka.Domain;
using Heurystyka.Infrastructure;
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
        public List<string> reports { get; set; } = new List<string>();
        public MultipleAlgorithms(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<List<string>> ExecuteOptimizationAsync(BestRequest request)
        {
            reports.Clear();
            foreach (var (functionString, index) in request.AlgorithmNames.Select((func, idx) => (func, idx)))
            {
                var algorithm = OptionsService.GetAlgorithms()[functionString];
                var fitnessFunction = OptionsService.GetTestFunctionNames()[request.TestFunctionName];
                double[] parameters = new double[0];
                AlgorithmResult algorithmResult = await dataContext.AlgorithmResults.FirstOrDefaultAsync(ar => ar.AlgorithmName == request.TestFunctionName);
                if (algorithmResult != null) { 
                    parameters = algorithmResult.Parameters.Select(p => p.ParameterValue).ToArray();
                }
                var result = await Task.Run(() => algorithm.Solve(
                        fitnessFunction,
                        ConvertTo2DArray(request.domain),
                        request.Size,
                        request.Iteration,
                        request.Dimensions,
                        request.load,
                        parameters));

                lock (reports)
                {
                    reports.Add($"{functionString} {request.TestFunctionName} {algorithm.stringReportGenerator()}\n");
                }
            }
            return reports;
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
    }
}
