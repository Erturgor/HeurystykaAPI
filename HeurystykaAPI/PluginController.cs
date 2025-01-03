using Heurystyka.Domain;
using Microsoft.AspNetCore.Mvc;

namespace HeurystykaAPI
{
    public class PluginController:BaseApiController
    {
        [HttpPost("upload")]
        public IActionResult UploadPlugin(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("Brak pliku.");

            if (Path.GetExtension(file.FileName) != ".dll")
                return BadRequest("Plik musi mieć rozszerzenie .dll");


            string targetDirectory = GetTargetDirectory(file.FileName);
            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory); 
            }

            var pluginPath = Path.Combine(targetDirectory, file.FileName);
            using (var stream = new FileStream(pluginPath, FileMode.Create))
            {
                file.CopyTo(stream);
            }


            Configuration.LoadPlugins();

            return Ok("Wtyczka została dodana.");
        }


        private string GetTargetDirectory(string fileName)
        {

            if (fileName.Contains("Algorytm", StringComparison.OrdinalIgnoreCase))
            {
                return Path.Combine("Algorytmy"); // Folder na algorytmy
            }
            else if (fileName.Contains("Funkcja", StringComparison.OrdinalIgnoreCase))
            {
                return Path.Combine("Funkcje"); // Folder na funkcje testowe
            }
            else
            {
                return Path.Combine("UnknownPlugins"); 
            }
        }

    }
}
