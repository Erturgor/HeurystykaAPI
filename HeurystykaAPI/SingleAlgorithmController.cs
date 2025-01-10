using Heurystyka.Application;
using Heurystyka.Domain;
using Heurystyka.Infrastructure;
using iText.Layout.Tagging;
using Microsoft.AspNetCore.Mvc;

namespace HeurystykaAPI
{

    public class SingleAlgorithmController : BaseApiController
    {
        private readonly SingleAlgorithm _service;

        public SingleAlgorithmController(DataContext dataContext)
        {
            _service = new SingleAlgorithm(dataContext);
        }
        [HttpPost]
        public async Task<ReportMultiple> ExecuteOptimizationAsync([FromBody] OptimizationRequest request)
        {
            return await _service.ExecuteOptimizationAsync(request); 
        }
        [HttpGet("raport")]
        public IActionResult Report()
        {
            if(_service.Report != null) return Ok(_service.Report);
            return NoContent();
            
        }
        [HttpGet("status")]
        public IActionResult Status()
        {
            return Ok(_service.state);

        }
    }
}
