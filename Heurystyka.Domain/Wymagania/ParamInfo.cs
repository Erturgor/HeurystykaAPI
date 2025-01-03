using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Domain.Wymagania
{
    public class ParamInfo
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public double UpperBoundary { get; set; }
        public double LowerBoundary { get; set; }
        public ParamInfo(string name, string description, double upperBoundary, double lowerBoundary)
        {
            Name = name;
            Description = description;
            UpperBoundary = upperBoundary;
            LowerBoundary = lowerBoundary;

        }
    }
}
