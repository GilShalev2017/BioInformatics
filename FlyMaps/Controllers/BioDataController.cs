using FlyMaps.Services;
using Microsoft.AspNetCore.Mvc;

namespace FlyMaps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BioDataController : Controller
    {
        private readonly ILogger<BioDataController> _logger;
        private readonly IBioDataImporter _bioDataImporter;

        public BioDataController(ILogger<BioDataController> logger, IBioDataImporter bioDataImporter)
        {
            _logger = logger;
            _bioDataImporter = bioDataImporter;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportData()
        {
            await _bioDataImporter.ImportDataAsync();

            return Ok(new { message = "Data imported successfully!" });
        }
    }
}
