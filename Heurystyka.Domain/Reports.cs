using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Domain
{
    public class ReportSingle
    {
        public Guid Id { get; set; }
        public string XBest { get; set; }
        public double FBest { get; set; }
        public string AlgorithmName { get; set; } // Nowe pole
        public string AlgorithmFunction { get; set; } // Nowe pole
        public string? Parameters { get; set; }
    }
    public class ReportMultiple
    {
        public Guid Id { get; set; }
        public List<ReportSingle> Reports { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
