using Heurystyka.Application;
using Microsoft.AspNetCore.Mvc;

namespace HeurystykaAPI
{
    public class OptionsController:BaseApiController
    {

        [HttpGet("functions")]
        public IActionResult GetTestFunctions()
        {
            return Ok(OptionsService.GetTestFunctionNames().Keys.ToList());
        }

        [HttpGet("algorithms")]
        public IActionResult GetAlgorithms()
        {
            return Ok(OptionsService.GetAlgorithms().Keys.ToList());
        }
    }
}
