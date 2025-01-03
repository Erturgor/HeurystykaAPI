using Heurystyka.Domain.Wymagania;
using Heurystyka.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Application
{
    public class OptionsService
    {
        public static Dictionary<string, fitnessFunction> GetTestFunctionNames() => Configuration.GetAllTestFunctions();

        public static Dictionary<string, IOptimizationAlgorithm> GetAlgorithms() => Configuration.GetAllAlgorithms();
    }
}
