using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Domain
{
    public class Funkcja : ITestFunction
    {
        public string Name { get; set; } = "Test";

        public double Evaluate(double[] parameters)
        {
            return 0;
        }
    }
}
