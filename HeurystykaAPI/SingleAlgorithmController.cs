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
        private readonly StateMonitor _stateMonitor;
        public SingleAlgorithmController(StateMonitor stateMonitor,DataContext dataContext)
        {
            _service = new SingleAlgorithm(dataContext,stateMonitor);
            _stateMonitor = stateMonitor;
        }
        [HttpPost]
        public async Task<ReportMultiple> ExecuteOptimizationAsync([FromBody] OptimizationRequest request)
        {
            var a = await _service.ExecuteOptimizationAsync(request);
            return a;
        }

        [HttpGet("status")]
        public ActionResult<string> GetState()
        {
            return Ok(_stateMonitor.currentState);
        }

    }
}
