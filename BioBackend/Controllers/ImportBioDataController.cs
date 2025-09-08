using BioBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace BioBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportBioDataController : ControllerBase
    {
        private readonly ICsvImporter _csvImporter;
        private readonly ILogger<ImportBioDataController> _logger;

        public ImportBioDataController(ICsvImporter csvImporter,ILogger<ImportBioDataController> logger)
        {
            _csvImporter = csvImporter;
            _logger = logger;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportData()
        {
            await _csvImporter.ImportDataAsync();

            return Ok(new { message = "Data imported successfully!" });
        }
    }
}
