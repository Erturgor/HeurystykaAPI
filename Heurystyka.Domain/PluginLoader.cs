using Heurystyka.Domain.Wymagania;
using Heurystyka.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Loader;
using Heurystyka.Domain.Wymagania.Algorithms;

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
                    var assembly = Assembly.LoadFrom(Path.GetFullPath(file));
                    var types = assembly.GetTypes().Where(type => type.GetInterfaces().Any(k => k.Name == "IOptimizationAlgorithm")).ToList();


                    foreach (var type in types)
                    {

                        {

                            var instance = Activator.CreateInstance(type);
                            dynamic fc = instance as IOptimizationAlgorithm ?? (dynamic)instance;
                            algorithms[type.Name] = fc;
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
                    var assembly = Assembly.LoadFrom(Path.GetFullPath(file));
                    var types =  assembly.GetTypes().Where(type => type.GetInterfaces().Any(k => k.Name == "ITestFunction")).ToList();



                    foreach (var type in types)
                    {
                       
                        {

                            var instance = Activator.CreateInstance(type);
                            dynamic fc = instance as ITestFunction ?? (dynamic)instance;
                            fitnessFunction fitness = new fitnessFunction((double[] args) => fc.Evaluate(args));
                            testFunctions[type.Name] = fitness;
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
