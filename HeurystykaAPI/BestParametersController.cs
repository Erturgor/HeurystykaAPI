using Heurystyka.Domain;
using Heurystyka.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace HeurystykaAPI
{
    public class BestParametersController : BaseApiController
    {
        private readonly DataContext dataContext;
        public BestParametersController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public async Task<ActionResult<AlgorithmResult>> GetParameters(string name)
        {
            return await dataContext.AlgorithmResults.FirstOrDefaultAsync(ar => ar.AlgorithmName == name);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdateAlgorithmResult([FromBody] AlgorithmResult newResult)
        {
            if (newResult == null || string.IsNullOrEmpty(newResult.AlgorithmName) || newResult.Parameters == null)
            {
                return BadRequest("Invalid data."); 
            }

            var existingResult = await dataContext.AlgorithmResults
                .Include(ar => ar.Parameters)
                .FirstOrDefaultAsync(ar => ar.AlgorithmName == newResult.AlgorithmName);

            if (existingResult != null)
            {

                foreach (var updatedParameter in newResult.Parameters)
                {
                    var existingParameter = existingResult.Parameters
                        .FirstOrDefault(p => p.Id == updatedParameter.Id);

                    if (existingParameter != null)
                    {
                        existingParameter.ParameterName = updatedParameter.ParameterName;
                        existingParameter.ParameterValue = updatedParameter.ParameterValue;
                    }
                    else
                    {
                        existingResult.Parameters.Add(updatedParameter);
                    }
                }

                await dataContext.SaveChangesAsync();

                return Ok(); 
            }
            else
            {

                dataContext.AlgorithmResults.Add(newResult);
                await dataContext.SaveChangesAsync();
                return Ok(); 
            }
        }
    }
}
