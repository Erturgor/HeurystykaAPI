using Heurystyka.Application;
using Heurystyka.Domain;
using iText.Layout.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace HeurystykaAPI
{

    public class SingleAlgorithmController : BaseApiController
    {
        private readonly SingleAlgorithm _service;

        public SingleAlgorithmController()
        {
            _service = new SingleAlgorithm();
        }
        [HttpPost]
        public async Task<List<string>> ExecuteOptimizationAsync([FromBody] OptimizationRequest request)
        {
            return await _service.ExecuteOptimizationAsync(request); 
        }
        [HttpGet]
        public List<string> Report()
        {
            return  _service.reports;
        }
    }
}
