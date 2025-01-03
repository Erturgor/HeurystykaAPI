using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Domain.Wymagania
{
    public interface IOptimizationAlgorithm
    {
        string Name { get; set; }
        double Solve(fitnessFunction f, double[,] Domain, int Size = 10, int Iteration = 5, int Dimensions = 3, bool resume=false, params double[] parameters);
        public string stringReportGenerator();
        public void pdfReportGenerator();
        ParamInfo[] ParamsInfo { get; set; }
        IStateWriter writer { get; set; }
        IStateReader reader { get; set; }
        double[] XBest { get; set; }
        double FBest { get; set; }
        int NumberOfEvaluationFitnessFunction { get; set; }

    }
}
