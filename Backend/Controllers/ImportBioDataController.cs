using Backend.Services;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportBioDataController : ControllerBase
    {
        private readonly ICsvImporter _csvImporter;
        private readonly IElasticService _elasticService;
        private readonly ILogger<ImportBioDataController> _logger;

        public ImportBioDataController(ICsvImporter csvImporter, IElasticService elasticService, ILogger<ImportBioDataController> logger)
        {
            _csvImporter = csvImporter;
            _elasticService = elasticService;
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
