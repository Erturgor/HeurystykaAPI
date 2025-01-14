using Heurystyka.Domain;
using Heurystyka.Infrastructure;
using iText.Commons.Actions.Contexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace HeurystykaAPI
{
    public class DatabaseController : BaseApiController
    {
        private readonly DataContext dataContext;
        public DatabaseController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet("Parametry")]
        public async Task<ActionResult<AlgorithmResult>> GetParameters(string name)
        {
            var result = await dataContext.AlgorithmResults.Include(ar => ar.Parameters).FirstOrDefaultAsync(ar => ar.AlgorithmName == name);
            if (result == null)
            {
                return NotFound("Algorithm result not found.");
            }
            return Ok(result);
        }
        [HttpGet("Raporty")]
        public async Task<ActionResult<List<ReportMultiple>>> GetAllAlgorithmResults()
        {
            var Report = await dataContext.ReportMultiples
                .OrderByDescending(ar => ar.CreatedAt)
                .Include(ar => ar.Reports)  
                .ToListAsync(); 

      
            if (Report == null || !Report.Any())
            {
                return NoContent(); 
            }

            return Ok(Report);
        }
        [HttpGet("RaportOstatni")]
        public async Task<ActionResult<ReportMultiple>> GetReport()
        {
            var Report = await dataContext.ReportMultiples
                .OrderByDescending(ar => ar.CreatedAt)
                .Include(ar => ar.Reports).FirstOrDefaultAsync();



            if (Report == null)
            {
                return NoContent();
            }

            return Ok(Report);
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
                        .FirstOrDefault(p => p.ParameterName == updatedParameter.ParameterName);

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
                var newAlgorithmResult = new AlgorithmResult
                {
                    AlgorithmName = newResult.AlgorithmName,
                    Parameters = new List<AlgorithmParameter>() 
                };

                foreach (var param in newResult.Parameters)
                {
                    newAlgorithmResult.Parameters.Add(new AlgorithmParameter
                    {
                        ParameterName = param.ParameterName,
                        ParameterValue = param.ParameterValue
                    });
                }
                dataContext.AlgorithmResults.Add(newAlgorithmResult);
                await dataContext.SaveChangesAsync();
                return Ok(); 
            }
        }
    }
}
