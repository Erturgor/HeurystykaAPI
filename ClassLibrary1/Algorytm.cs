
using Heurystyka.Domain;
using Heurystyka.Domain.Wymagania;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;



public class Algorytm : IOptimizationAlgorithm
    {

        public string Name { get; set; } = "AlgorytmTest";
        public double[] XBest { get; set; }
        public double FBest { get; set; }
        public int NumberOfEvaluationFitnessFunction { get; set; }

        public int currentIteration;
        public int iteration { get; set; }
        public int dimensions { get; set; }

        public fitnessFunction fun { get; set; }
        public ParamInfo[] ParamsInfo { get; set; } = Array.Empty<ParamInfo>();
        public IStateWriter writer { get; set; } 
        public IStateReader reader { get; set; } 




        public int size { get; set; }
        double[,] domain;
        List<double[]> bees;
        double[] fitnesses;
        int[] trial;



        private bool readFile()
        {
            string filePath = "ABC.txt";
            if (File.Exists(filePath))
            {
                try
                {
                  
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
            string filePath = "ABC.txt";
            try
            {
               
            }
            catch (Exception ex)
            {
                
            }
        }
        private void generateBees()
        {
            bees = new List<double[]>();
            fitnesses = new double[size];
            trial = new int[size];
            for (int j = 0; j < size; j++)
            {
                trial[j] = 0;
            }
            Random rd = new Random();
            for (int i = 0; i < size; i++)
            {
                double[] bee = new double[dimensions];
                for (int j = 0; j < dimensions; j++)
                    bee[j] = domain[j, 0] + rd.NextDouble() * (domain[j, 1] - domain[j, 0]);
                bees.Add(bee);
                fitnesses[i] = fitness(bees[i]);
                trial[i] = 0;
            }
            FBest = fitnesses[0];
            XBest = bees[0];
        }

        private double fitness(double[] value)
        {
            return 1 / (1 + fun(value));
        }

        private double[] generateNewSolution(int i)
        {
            Random random = new Random();
            int k;
            do
            {
                k = random.Next(size);
            } while (k == i);

            double[] solution = new double[dimensions];
            for (int j = 0; j < dimensions; j++)
            {
                double phi = random.NextDouble() * 2 - 1; // Random number in [-1, 1]
                solution[j] = bees[i][j] + phi * (bees[i][j] - bees[k][j]);
                solution[j] = Math.Max(domain[j, 0], Math.Min(domain[j, 1], solution[j]));
            }

            return solution;
        }
        private void SendEmployedBees()
        {
            for (int i = 0; i < size; i++)
            {
                createAndCheckFitness(i);
            }
        }

        private void SendOnlookerBees()
        {
            Random rd = new Random();
            double totalFitness = 0.0;
            double[] cumulativeProbabilities = new double[size];

            for (int i = 0; i < size; i++)
                totalFitness += fitnesses[i];

            double cumulativeSum = 0.0;
            for (int i = 0; i < size; i++)
            {
                cumulativeSum += fitnesses[i] / totalFitness;
                cumulativeProbabilities[i] = cumulativeSum;
            }
            for (int i = 0; i < size; i++)
            {
                double randomValue = rd.NextDouble();

                for (int j = 0; j < size; j++)
                {
                    if (randomValue < cumulativeProbabilities[j])
                    {
                        createAndCheckFitness(j);
                        break;
                    }
                }
            }
        }

        private void createAndCheckFitness(int i)
        {
            double[] newSolution = generateNewSolution(i);
            double newFitness = fitness(newSolution);
            if (newFitness > fitnesses[i])
            {
                bees[i] = newSolution;
                fitnesses[i] = newFitness;
                trial[i] = 0;
            }
            else
            {
                trial[i]++;
            }
            NumberOfEvaluationFitnessFunction++;
        }

        private void SendScoutBees()
        {
            Random random = new Random();
            for (int i = 0; i < size; i++)
            {
                if (trial[i] > size * dimensions)
                {
                    for (int j = 0; j < dimensions; j++)
                        bees[i][j] = domain[j, 0] + random.NextDouble() * (domain[j, 1] - domain[j, 0]);

                    fitnesses[i] = fitness(bees[i]);
                    trial[i] = 0;
                }
            }
        }

        public double Solve(fitnessFunction f, double[,] Domain, int Size = 10, int Iteration = 5, int Dimensions = 3, bool resume = false, params double[] parameters)
        {
            fun = f;
            domain = Domain;
            dimensions = Dimensions;
            size = Size;
            iteration = Iteration;
            if (domain == null || fun == null || domain.GetLength(1) !=2)
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
                trial = new int[size];
                for (int i = 0; i < size; i++)
                {
                    trial[i] = 0;
                }
            }
            if (!readed) { generateBees(); currentIteration = 1; }
            while (currentIteration <= iteration)
            {
                SendEmployedBees();
                SendOnlookerBees();
                SendScoutBees();
                for (int j = 1; j < size; j++)
                {
                    if (fitnesses[j] > FBest)
                    {
                        FBest = fitnesses[j];
                        XBest = bees[j];
                    }
                }
                currentIteration++;
                if (currentIteration % 10 == 1)
                {
                    writeFile();
                }
            }
            FBest = f(XBest);
            return FBest;
        }

        public string stringReportGenerator()
        {
            StringBuilder report = new StringBuilder();
            report.AppendLine($"Best X Values: {string.Join(", ", XBest)}");
            report.AppendLine($"Best Fitness (FBest): {FBest}");
            report.AppendLine($"Number of Evaluations: {NumberOfEvaluationFitnessFunction}");
            return report.ToString();
        }
        public void pdfReportGenerator()
        {
            
        }
    }
   

