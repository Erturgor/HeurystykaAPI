using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Domain.Wymagania
{
    public interface IGenerateTextReport
    {
        string ReportString(Outcome outcome);
    }

    public interface IGeneratePDFReport
    {
        void GenerateReport(Outcome outcome, string path);
    }
    public class Outcome
    {
        public double[] XBest { get; set; }
        public double FBest { get; set; }
        public int NumberOfEvaluationFitnessFunction { get; set; }
    }
}
