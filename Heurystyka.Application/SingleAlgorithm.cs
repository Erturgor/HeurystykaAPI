using Heurystyka.Domain;
using Heurystyka.Domain.Wymagania;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Application
{
    public class SingleAlgorithm
    {
        public List<string> reports { get; set; } = new List<string>();
        public async Task<List<string>> ExecuteOptimizationAsync(OptimizationRequest request)
        {
            reports.Clear();
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
                    var algorithm = OptionsService.GetAlgorithms()[request.AlgorithmName];
                    var result = await Task.Run(() => algorithm.Solve(
                        fitnessFunction,
                        ConvertTo2DArray(domain),
                        request.Size,
                        request.Iteration,
                        request.Dimensions,
                        request.load,
                        combination.ToArray()));

                    lock (reports)
                    {
                        string parametersAsString = string.Join(" ", combination.Select(p => p.ToString()));
                        reports.Add($"{functionString} {request.AlgorithmName} {parametersAsString} {algorithm.stringReportGenerator()}\n");
                    }
                }
            }
            return reports;
        }

        static IEnumerable<IEnumerable<double>> CartesianProduct(List<IEnumerable<double>> sets)
        {
            // Base case: If only one set is left, return it
            if (sets.Count == 1)
            {
                return sets[0].Select(item => new[] { item });
            }

            // Recursive step: Take the first set and combine it with the Cartesian product of the rest
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
    }

}

