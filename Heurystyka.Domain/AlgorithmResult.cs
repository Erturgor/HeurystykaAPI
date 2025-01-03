using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Domain
{
    public class AlgorithmParameter
    {
        public Guid Id { get; set; }
        public string ParameterName { get; set; } = null!;
        public double ParameterValue { get; set; }
    }
    public class AlgorithmResult
    {
        public Guid Id { get; set; }
        public string AlgorithmName { get; set; }
        public List<AlgorithmParameter> Parameters { get; set; } = new List<AlgorithmParameter>();
    }
}
