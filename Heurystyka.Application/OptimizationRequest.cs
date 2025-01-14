namespace Heurystyka.Application
{
    public class OptimizationRequest
    {
        public string AlgorithmName { get; set; }
        public List<string> TestFunctionNames { get; set; }
        public List<double[]> ParameterRanges { get; set; }
        public List<double[][]> domain { get; set; }
        public double[] repetitions { get; set; }
        public bool load { get; set; }
        public int Size { get; set; }
        public int Dimensions { get; set; }
        public int Iteration {  get; set; } 
    }
    public class BestRequest
    {
        public string TestFunctionName { get; set; }
        public List<string> AlgorithmNames { get; set; }
        public bool load { get; set; }
        public double[] repetitions { get; set; }
        public double[][] domain { get; set; }
        public int Size { get; set; }
        public int Dimensions { get; set; }
        public int Iteration { get; set; }
    }
    
}
