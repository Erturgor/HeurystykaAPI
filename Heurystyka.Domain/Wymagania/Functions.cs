using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Heurystyka.Domain.Wymagania
{
    public class Funkcje
    {
        public static double Rastrigin(double[] x)
        {
            double A = 10.0;
            double n = x.Length;
            double sum = 0.0;

            for (int i = 0; i < x.Length; i++)
            {
                double term = Math.Pow(x[i], 2) - A * Math.Cos(2 * Math.PI * x[i]);
                sum += term;
            }

            return A * n + sum;
        }
        public static double Sphere(double[] x)
        {
            double sum = 0.0;

            for (int i = 0; i < x.Length; i++)
            {
                double term = Math.Pow(x[i], 2);
                sum += term;
            }

            return sum;
        }
        public static double Beale(double[] x)
        {
            double sum = 0.0;

            sum = Math.Pow(1.5 - x[0] + x[0] * x[1], 2) + Math.Pow(2.25 - x[0] + x[0] * x[1] * x[1], 2) + Math.Pow(2.625 - x[0] + x[0] * Math.Pow(x[1], 3), 2);
            return sum;
        }
        public static double Bukin(double[] x)
        {
            double sum = 0.0;

            sum = 100.0 * Math.Sqrt(Math.Abs(x[1] - 0.01 * Math.Pow(x[0], 2))) + 0.01 * Math.Abs(x[0] + 10.0);

            return sum;
        }
        public static double HimmelBlau(double[] x)
        {
            double sum = 0.0;

            sum = Math.Pow(Math.Pow(x[0], 2) + x[1] - 11.0, 2) + Math.Pow(x[0] + Math.Pow(x[1], 2) - 7, 2);

            return sum;
        }
        public static double Rosenbrock(double[] x)
        {
            double sum = 0.0;

            for (int i = 0; i < x.Length - 1; i++)
            {
                double term1 = 100.0 * Math.Pow(x[i + 1] - Math.Pow(x[i], 2), 2);
                double term2 = Math.Pow(1.0 - x[i], 2);
                sum += term1 + term2;
            }

            return sum;
        }
    }
}
