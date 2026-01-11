using FlyMaps.Models;
using FlyMaps.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FlyMaps.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BioDataController : Controller
    {
        private readonly ILogger<BioDataController> _logger;
        private readonly IBioDataImporter _bioDataImporter;
        private readonly IBioDataService _bioDataService;

        public BioDataController(ILogger<BioDataController> logger, IBioDataImporter bioDataImporter, IBioDataService bioDataService)
        {
            _logger = logger;
            _bioDataImporter = bioDataImporter;
            _bioDataService = bioDataService;
        }

        [HttpPost("import")]
        public async Task<IActionResult> ImportData()
        {
            await _bioDataImporter.ImportDataAsync();

            return Ok(new { message = "Data imported successfully!" });
        }

        [HttpGet("genes/{symbol}")]
        public async Task<ActionResult<Gene>> GetGene(string symbol)
        {
            try
            {
                var gene = await _bioDataService.GetGeneDetailsAsync(symbol);
                if (gene == null)
                {
                    return NotFound($"Gene with Symbol {symbol} was not found.");
                }
                return Ok(gene);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving gene with Symbol {GeneId}", symbol);
                return StatusCode(500, "An error occurred while retrieving the gene");
            }
        }

        [HttpGet("genes/search")]
        public async Task<ActionResult<List<Gene>>> SearchGenes([FromQuery] string query)
        {
            try
            {
                var genes = await _bioDataService.SearchGenesAsync(query);
                return Ok(genes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching genes with query {Query}", query);
                return StatusCode(500, "An error occurred while searching genes");
            }
        }
    }
}
