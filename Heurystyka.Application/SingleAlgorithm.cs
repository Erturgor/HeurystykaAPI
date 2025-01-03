using Heurystyka.Domain;
using Heurystyka.Domain.Wymagania;
using System;
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
                var combinations = GetCartesianProduct(paramRange);
                for (var i = 0; i < request.ParameterRanges.Count; i++)
                {
                    List<double> par = new List<double>();
                    for (var x = request.ParameterRanges[i][0]; x < request.ParameterRanges[i][1]; x += request.ParameterRanges[i][2])
                    {
                        par.Add(x);
                    }
                    paramRange.Add(par);
                }
                foreach (var combination in combinations)
                {
                    var algorithm = OptionsService.GetAlgorithms()[request.AlgorithmName];
                    var result = await Task.Run(() => algorithm.Solve(
                        fitnessFunction,
                        domain,
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

        static IEnumerable<IEnumerable<double>> GetCartesianProduct(List<IEnumerable<double>> sets)
        {
            IEnumerable<IEnumerable<double>> product = new[] { Enumerable.Empty<double>() };

            foreach (var set in sets)
            {
                product = product.SelectMany(
                    prefix => set,
                    (prefix, item) => prefix.Concat(new[] { item })
                );
            }

            return product;
        }

    }

}

