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
        private readonly StateMonitor _stateMonitor;
        public MultipleAlgorithmsController(DataContext dataContext, StateMonitor stateMonitor)
        {
            _service = new MultipleAlgorithms(dataContext, stateMonitor);
            _stateMonitor = stateMonitor;
        }
        [HttpPost]
        public async Task<ReportMultiple> ExecuteOptimizationAsync([FromBody] BestRequest request)
        {
            return await _service.ExecuteOptimizationAsync(request); ;
        }

        [HttpGet("status")]
        public ActionResult<string> GetState()
        {
            return Ok(_stateMonitor.currentState);
        }
    }
}
