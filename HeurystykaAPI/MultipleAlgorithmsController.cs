using Heurystyka.Application;
using Heurystyka.Domain;
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
        public async Task<ReportMultiple> ExecuteOptimizationAsync([FromBody] BestRequest request)
        {
            return await _service.ExecuteOptimizationAsync(request); ;
        }
        [HttpGet("raport")]
        public IActionResult Report()
        {
            if (_service.Report != null) return Ok(_service.Report);
            return NoContent();

        }
        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok(_service.state);

        }
    }
}
