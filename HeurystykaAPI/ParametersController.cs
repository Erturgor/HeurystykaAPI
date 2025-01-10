using Heurystyka.Application;
using Heurystyka.Domain;
using Heurystyka.Infrastructure;
using Microsoft.AspNetCore.Mvc;

namespace HeurystykaAPI
{
    public class ParametersController:BaseApiController
    {
        [HttpGet]
        public async Task<ActionResult<int>> GetParameters(string name)
        {
            return Ok(OptionsService.GetAlgorithms()[name].ParamsInfo.Length);
        }
    }
}
