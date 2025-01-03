using Heurystyka.Domain.Wymagania;
using Heurystyka.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Domain
{
    public class PluginLoader
    {

        public static Dictionary<string, IOptimizationAlgorithm> LoadAlgorithms()
        {
            string pluginsPath = "Algorytmy";
            var algorithms = new Dictionary<string, IOptimizationAlgorithm>();

            if (!Directory.Exists(pluginsPath))
                return algorithms;

            var dllFiles = Directory.GetFiles(pluginsPath, "*.dll");

            foreach (var file in dllFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    var types = assembly.GetTypes()
                        .Where(t => typeof(IOptimizationAlgorithm).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in types)
                    {
                        if (Activator.CreateInstance(type) is IOptimizationAlgorithm algorithm)
                        {
                            algorithms[type.Name] = algorithm;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading algorithm from {file}: {ex.Message}");
                }
            }

            return algorithms;
        }

        public static Dictionary<string, fitnessFunction> LoadTestFunctions()
        {
            string pluginsPath = "Funkcje";
            var testFunctions = new Dictionary<string, fitnessFunction>();

            if (!Directory.Exists(pluginsPath))
                return testFunctions;

            var dllFiles = Directory.GetFiles(pluginsPath, "*.dll");

            foreach (var file in dllFiles)
            {
                try
                {
                    var assembly = Assembly.LoadFrom(file);
                    var types = assembly.GetTypes()
                        .Where(t => typeof(ITestFunction).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

                    foreach (var type in types)
                    {
                        if (Activator.CreateInstance(type) is ITestFunction testFunction)
                        {
                            testFunctions[type.Name] = testFunction.Evaluate;
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error loading test function from {file}: {ex.Message}");
                }
            }

            return testFunctions;
        }
    }
}
