using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Domain
{
    public interface ITestFunction
    {
        string Name { get; }
        double Evaluate(double[] parameters);
    }
}
