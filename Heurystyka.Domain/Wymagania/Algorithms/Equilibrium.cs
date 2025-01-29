using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Heurystyka.Domain.Wymagania.Algorithms
{
    public class Equilibrium : IOptimizationAlgorithm
    {
        // Zalozenia odgorne
        public string Name { get; set; } = "Equilibrum Optimizer Algorithm";
        public double[] XBest { get; set; }
        public double FBest { get; set; }
        public int NumberOfEvaluationFitnessFunction { get; set; } = 0;

        // Wymagania do testowania kazdej klasy tak czy siak 
        public int size { get; set; }
        public int iteration { get; set; }
        public int dimensions { get; set; }
        double[,] domain;
        public fitnessFunction fun { get; set; }

        //Parametry Equilibrum
        double a1 { get; set; } = 2;
        double a2 { get; set; } = 1;
        double GP { get; set; } = 0.5;
        public ParamInfo[] ParamsInfo { get; set; } 
        public IStateWriter writer { get; set; } = new EquilibrumWriter();
        public IStateReader reader { get; set; } = new EquilibrumStateReader();


        int currentIteration;
        List<double[]> equilibrumPool;
        List<double[]> particles;
        List<double[]> oldParticles;//Czastki pamietaja swoje jedno polozenie wczesniej
        public Equilibrium()
        {
            ParamsInfo = new ParamInfo[3];
            ParamsInfo[0] = new ParamInfo("a1", "controls the exploration quantity", -10, 10);
            ParamsInfo[1] = new ParamInfo("gp", "controls the participation probability of concentration updating by the generation rate", 0.0, 1.0);
            ParamsInfo[2] = new ParamInfo("a2", "controls the exploitation feature", -10, 10);

        }


        private bool readFile()
        {
            string filePath = "equilibrum.txt";

            if (File.Exists(filePath))
            {
                try
                {

                    State state = reader.LoadFromFileStateOfAlghoritm(filePath);
                    if (state == null) return false;
                    XBest = state.XBest;
                    FBest = state.FBest;
                    NumberOfEvaluationFitnessFunction = state.NumberOfEvaluationFitnessFunction;

                    size = state.Hive.Count;
                    iteration = state.Iteration;
                    dimensions = state.Hive[0].Length;
                    currentIteration = state.Iteration;

                    particles = state.Hive;
                    oldParticles = copy();
                    equilibrumPool = new List<double[]>();
                    for (int i = 0; i < 5; i++)
                    {
                        double[] equilibrum = new double[dimensions];
                        for (int j = 0; j < dimensions; j++)
                        {
                            equilibrum[j] = Math.Pow(10, 15);
                        }
                        equilibrumPool.Add(equilibrum);
                    }
                    checkFitness();
                    return true;

                }
                catch (Exception ex)
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
        private void writeFile()
        {
            string filePath = "equilibrum.txt";
            State state = new State(XBest, FBest, currentIteration, null, [], particles, NumberOfEvaluationFitnessFunction);
            try
            {
                writer.SaveToFileStateOfAlghoritm(state, "equilibrum.txt");
            }
            catch (Exception ex)
            {

            }
        }
        private void generateParticles()
        {
            equilibrumPool = new List<double[]>();
            particles = new List<double[]>();
            oldParticles = new List<double[]>();
            Random rd = new Random();
            for (int i = 0; i < size; i++)
            {
                double[] particle = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    particle[j] = domain[j, 0] + rd.NextDouble() * (domain[j, 1] - domain[j, 0]);
                }
                particles.Add(particle);
            }
            for (int i = 0; i < 5; i++)
            {
                double[] equilibrum = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                {
                    equilibrum[j] = Math.Pow(10, 15);
                }
                equilibrumPool.Add(equilibrum);
            }
            oldParticles = copy();//kopiujemy poprzedni wynik na poczatku po prostu to samo
        }

        private List<double[]> copy()
        {
            List<double[]> temp = new List<double[]>();
            foreach (var array in particles)
            {
                double[] copiedArray = new double[array.Length];
                Array.Copy(array, copiedArray, array.Length);
                temp.Add(copiedArray);
            }
            return temp;
        }

        private void checkFitness()
        {

            for (int i = 0; i < size; i++)
            {
                if (fun(particles[i]) < fun(equilibrumPool[0]))
                {
                    var temp = equilibrumPool[0];
                    equilibrumPool[0] = particles[i];
                    particles[i] = temp;
                    NumberOfEvaluationFitnessFunction++;
                }
                else if (fun(particles[i]) < fun(equilibrumPool[1]))
                {
                    var temp = equilibrumPool[1];
                    equilibrumPool[1] = particles[i];
                    particles[i] = temp;
                    NumberOfEvaluationFitnessFunction++;
                }
                else if (fun(particles[i]) < fun(equilibrumPool[2]))
                {
                    var temp = equilibrumPool[2];
                    equilibrumPool[2] = particles[i];
                    particles[i] = temp;
                    NumberOfEvaluationFitnessFunction++;
                }
                else if (fun(particles[i]) < fun(equilibrumPool[3]))
                {
                    var temp = equilibrumPool[3];
                    equilibrumPool[3] = particles[i];
                    particles[i] = temp;
                    NumberOfEvaluationFitnessFunction++;

                }
            }
            for (var i = 0; i < dimensions; i++)
            {
                var average = equilibrumPool[4];
                average[i] = (equilibrumPool[0][i] + equilibrumPool[1][i] + equilibrumPool[2][i] + equilibrumPool[3][i]) / 4;
            }


        }
        private double[] randomTable()
        {
            double[] table = new double[dimensions];
            Random rd = new Random();

            for (int i = 0; i < dimensions; i++)
            {
                table[i] = rd.NextDouble();
            }
            return table;
        }
        private void memorySave()
        {
            for (int i = 0; i < size; i++)
            {
                if (fun(particles[i]) > fun(oldParticles[i]))
                {
                    particles[i] = oldParticles[i];
                }
            }
            oldParticles = copy();
        }
        private double[] generateGCP()
        {
            Random rd = new Random();
            double r1 = rd.NextDouble();
            double r2 = rd.NextDouble();
            double[] GCP = new double[dimensions];
            for (int i = 0; i < dimensions; i++)
            {
                if (r2 >= GP)
                {
                    GCP[i] = 0.5 * r1;
                }
                else GCP[i] = 0;
            }
            return GCP;
        }

        public double Solve(fitnessFunction f, double[,] Domain, int Size = 10, int Iteration = 5, int Dimensions = 3, bool resume=false, params double[] parameters)
        {
            size = Size;
            iteration = Iteration;
            dimensions = Dimensions;
            domain = Domain;
            fun = f;
            if (parameters.Length == 3)
            {
              if(parameters[0] >= ParamsInfo[0].LowerBoundary && parameters[0] <= ParamsInfo[0].UpperBoundary)
                    {
                    a1 = parameters[0];
                }
                if (parameters[1] >= ParamsInfo[1].LowerBoundary && parameters[1] <= ParamsInfo[1].UpperBoundary)
                {

                    a2 = parameters[1];
                }
                if (parameters[2] >= ParamsInfo[2].LowerBoundary && parameters[2] <= ParamsInfo[2].UpperBoundary)
                {

                    GP = parameters[2];
                }
            }

            if (domain == null || fun == null || domain.GetLength(1) != 2)
            {
                throw new ArgumentException("wrong type");
            }
            if (domain.GetLength(0) == 1)
            {
                double[,] transformed = new double[dimensions, 2];

                for (int i = 0; i < dimensions; i++)
                {
                    for (int j = 0; j < 2; j++)
                    {
                        transformed[i, j] = domain[0, j];
                    }
                }
                domain = transformed;
            }
            bool readed = false;
            if (resume)
            {
                readed = readFile();
            }
            if (!readed) { generateParticles(); currentIteration = 1; }
            while (currentIteration <= iteration)
            {
                checkFitness();
                if (currentIteration > 1) memorySave();
                double t = Math.Pow((1 - currentIteration / iteration), (a2 * currentIteration / iteration));
                for (int j = 0; j < size; j++)
                {
                    Random rd = new Random();
                    var Ceq = equilibrumPool[rd.Next(equilibrumPool.Count)];
                    var lamda = randomTable();
                    var r = randomTable();
                    var F = new double[dimensions];
                    for (int k = 0; k < dimensions; k++)
                    {
                        F[k] = a1 * Math.Sign(r[k] - 0.5) * (Math.Pow(Math.E, lamda[k] * t) - 1);
                    }
                    var generationControlParameter = generateGCP();
                    var G0 = new double[dimensions];
                    var G = new double[dimensions];
                    for (int k = 0; k < dimensions; k++)
                    {
                        G0[k] = generationControlParameter[k] * (Ceq[k] - lamda[k] * particles[j][k]);
                        G[k] = G0[k] * F[k];
                        var temp = Ceq[k] + (particles[j][k] - Ceq[k]) * F[k] + G[k] * (1 - F[k]);
                        particles[j][k] = Math.Max(domain[k,0], Math.Min(domain[k, 1], temp));

                    }
                }
                currentIteration++;
                if (currentIteration % 10 == 1)
                {
                    equilibrumPool.Sort((x1, x2) => fun(x1).CompareTo(fun(x2))); //czysto teoretycznie average moze byc lepszy
                    XBest = equilibrumPool[0];
                    FBest = fun(equilibrumPool[0]);
                    writeFile();
                }
            }
            equilibrumPool.Sort((x1, x2) => fun(x1).CompareTo(fun(x2))); //czysto teoretycznie average moze byc lepszy
            XBest = equilibrumPool[0];
            FBest = fun(equilibrumPool[0]);
            return FBest;
        }
        public string stringReportGenerator() {
            StringBuilder report = new StringBuilder();
            report.AppendLine($"Best X Values: {string.Join(", ", XBest)}");
            report.AppendLine($"Best Fitness (FBest): {FBest}");
            report.AppendLine($"Number of Evaluations: {NumberOfEvaluationFitnessFunction}");
            return report.ToString();
        }
        public void pdfReportGenerator()
        {
            using (var writer = new PdfWriter("Equilibrum.pdf"))
            {
                using (var pdf = new PdfDocument(writer))
                {
                    var document = new Document(pdf);

                    document.Add(new Paragraph($"Best X Values: {string.Join(", ", XBest)}"));
                    document.Add(new Paragraph($"Best Fitness (FBest): {FBest}"));
                    document.Add(new Paragraph($"Number of Evaluations: {NumberOfEvaluationFitnessFunction}"));
                }
            }
        }
    }
    public class EquilibrumWriter : IStateWriter
    {
        public void SaveToFileStateOfAlghoritm(State state, string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine($"{state.Iteration}");

                writer.WriteLine($"{state.NumberOfEvaluationFitnessFunction}");
                writer.WriteLine($"{state.FBest}");
                writer.WriteLine($"{string.Join(" ", state.XBest)}");
                for (int i = 0; i < state.Hive.Count; i++)
                {
                    var bee = state.Hive[i];

                    writer.WriteLine($"{string.Join(" ", bee)}");
                }
            }
        }
    }
    public class EquilibrumStateReader : IStateReader
    {

        public State LoadFromFileStateOfAlghoritm(string path)
        {
            try
            {
                using (StreamReader reader = new StreamReader(path))
                {
                    string line;
                    State state = new State();
                    line = reader.ReadLine();
                    if (line != null)
                    {
                        state.Iteration = int.Parse(line.Trim());
                    }

                    line = reader.ReadLine();
                    if (line != null)
                    {
                        state.NumberOfEvaluationFitnessFunction = int.Parse(line.Trim());
                    }
                    if (line != null)
                    {
                        state.FBest = double.Parse(line.Trim());
                    }
                    if (line != null)
                    {
                        string[] parts = line.Split(' ');
                        state.XBest = parts.Take(parts.Length).Select(double.Parse).ToArray();

                    }

                    List<double> fitnessList = new List<double>();

                    while ((line = reader.ReadLine()) != null)
                    {
                        string[] parts = line.Split(' ');

                        var bee = parts.Take(parts.Length).Select(double.Parse).ToArray();
                        state.Hive.Add(bee);
                    }
                    state.Fitnesses = fitnessList.ToArray();
                    return state;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

}
