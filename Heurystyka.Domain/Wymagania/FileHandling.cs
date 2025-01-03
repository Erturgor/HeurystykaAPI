using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Domain.Wymagania
{
    public interface IStateWriter
    {
        void SaveToFileStateOfAlghoritm(State state, string path);
    }
    public interface IStateReader
    {
        State LoadFromFileStateOfAlghoritm(string path);
    }
    public class State
    {


        public State()
        {
        }

        public State(double[] xBest, double fBest, int iteration, double[] fitnesses, double[] parameters, List<double[]> hive, int numberOfEvaluationFitnessFunction)
        {
            XBest = xBest;
            FBest = fBest;
            Iteration = iteration;
            Fitnesses = fitnesses;
            this.parameters = parameters;
            Hive = hive;
            NumberOfEvaluationFitnessFunction = numberOfEvaluationFitnessFunction;
        }

        public double[] XBest { get; set; }
        public double FBest { get; set; }
        public int Iteration { get; set; }
        public double[] Fitnesses { get; set; }
        public double[] parameters { get; set; }
        public List<double[]> Hive { get; set; } =new List<double[]>();
        public int NumberOfEvaluationFitnessFunction { get; set; }
    }
}
