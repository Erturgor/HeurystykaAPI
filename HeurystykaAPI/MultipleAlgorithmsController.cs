using Heurystyka.Application;
using Heurystyka.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;

namespace HeurystykaAPI
{
    public class MultipleAlgorithmsController : BaseApiController
    {
        private readonly MultipleAlgorithms _service;
        public MultipleAlgorithmsController(DataContext dataContext)
        {
            _service = new MultipleAlgorithms(dataContext);
        }
        [HttpPost]
        public async Task<List<string>> ExecuteOptimizationAsync([FromBody] BestRequest request)
        {
            return await _service.ExecuteOptimizationAsync(request); ;
        }
        [HttpGet]
        public List<string> Report()
        {
            return _service.reports;

        }
    }
}
