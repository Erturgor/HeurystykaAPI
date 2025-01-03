﻿using Heurystyka.Domain;
using Heurystyka.Domain.Wymagania;
using Heurystyka.Domain.Wymagania.Algorithms;

namespace Heurystyka.Domain
{
    public delegate double fitnessFunction(double[] arg);
    public class Configuration
    {
public static Dictionary<string,fitnessFunction> BuiltInTestFunctions { get; } = new()
    {
        { "Beale", Heurystyka.Domain.Wymagania.Funkcje.Beale },
        { "Rosenbrock", Heurystyka.Domain.Wymagania.Funkcje.Rosenbrock },
        { "Rastrigin", Heurystyka.Domain.Wymagania.Funkcje.Rastrigin },
        { "HimmelBlau", Heurystyka.Domain.Wymagania.Funkcje.HimmelBlau },
        { "Bukin", Heurystyka.Domain.Wymagania.Funkcje.Bukin },
        { "Sphere", Heurystyka.Domain.Wymagania.Funkcje.Sphere }
    };

    public static Dictionary<string, IOptimizationAlgorithm> BuiltInAlgorithms { get; } = new()
    {
        { "ArtificialBeeColony", new ArtificialBeeColony() },
        { "Equilibrium", new Equilibrium() }
    };

    public static Dictionary<string, fitnessFunction> DynamicTestFunctions { get; private set; } = new();
    public static Dictionary<string, IOptimizationAlgorithm> DynamicAlgorithms { get; private set; } = new();

    public static void LoadPlugins()
    {
 
        var loadedFunctions = PluginLoader.LoadTestFunctions();
        foreach (var (name, function) in loadedFunctions)
        {
            DynamicTestFunctions[name] = function;
        }

        var loadedAlgorithms = PluginLoader.LoadAlgorithms();
        foreach (var (name, algorithm) in loadedAlgorithms)
        {
            DynamicAlgorithms[name] = algorithm;
        }
    }
        public static Dictionary<string, fitnessFunction> GetAllTestFunctions()
        {
            return BuiltInTestFunctions
                .Concat(DynamicTestFunctions)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }

        public static Dictionary<string, IOptimizationAlgorithm> GetAllAlgorithms()
        {
            return BuiltInAlgorithms
                .Concat(DynamicAlgorithms)
                .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
        }
    }
}