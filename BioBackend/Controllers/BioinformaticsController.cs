using BioBackend.Models;
using BioBackend.Services;
using Microsoft.AspNetCore.Mvc;

namespace BioBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BioinformaticsController : Controller
    {
        private readonly IBioinformaticsService _bioinformaticsService;
        private readonly ILogger<BioinformaticsController> _logger;
        public BioinformaticsController(IBioinformaticsService bioinformaticsService, ILogger<BioinformaticsController> logger)
        {
            _bioinformaticsService = bioinformaticsService;
            _logger = logger;
        }

        [HttpGet("diseases")]
        public async Task<ActionResult<IEnumerable<Disease>>> GetAllDiseases()
        {
            try
            {
                var diseases = await _bioinformaticsService.GetAllDiseasesAsync();
                return Ok(diseases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all diseases");
                return StatusCode(500, "An error occurred while retrieving diseases");
            }
        }

        [HttpGet("diseases/{diseaseId}")]
        public async Task<ActionResult<Disease>> GetDiseaseById(string diseaseId)
        {
            try
            {
                var disease = await _bioinformaticsService.FindDiseaseAndRelatedGenesAsync(diseaseId);
                if (disease == null)
                {
                    return NotFound($"Disease with ID {diseaseId} was not found.");
                }
                return Ok(disease);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving disease with ID {DiseaseId}", diseaseId);
                return StatusCode(500, "An error occurred while retrieving the disease");
            }
        }

        [HttpGet("diseases/paged")]
        public async Task<IActionResult> GetDiseasesPaged([FromQuery] int pageNumber = 1, [FromQuery] int pageSize = 10, [FromQuery] string? searchQuery = null)
        {
            try
            {
                var (items, totalCount) = await _bioinformaticsService.GetDiseasesPagedAsync(pageNumber, pageSize, searchQuery);

                return Ok(new
                {
                    Items = items,
                    TotalCount = totalCount,
                    PageNumber = pageNumber,
                    PageSize = pageSize
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving paged diseases");
                return StatusCode(500, "An error occurred while retrieving diseases");
            }
        }

        [HttpGet("genes")]
        public async Task<ActionResult<IEnumerable<Gene>>> GetAllGenes()
        {
            try
            {
                var genes = await _bioinformaticsService.GetAllGenesAsync();
                return Ok(genes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all genes");
                return StatusCode(500, "An error occurred while retrieving genes");
            }
        }

        [HttpGet("genes/{geneId}")]
        public async Task<ActionResult<Gene>> GetGeneById(string geneId)
        {
            try
            {
                var gene = await _bioinformaticsService.FindGeneAndRelatedEntitiesAsync(geneId);
                if (gene == null)
                {
                    return NotFound($"Gene with ID {geneId} was not found.");
                }
                return Ok(gene);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving gene with ID {GeneId}", geneId);
                return StatusCode(500, "An error occurred while retrieving the gene");
            }
        }

        [HttpGet("drugs")]
        public async Task<ActionResult<IEnumerable<Drug>>> GetAllDrugs()
        {
            try
            {
                var drugs = await _bioinformaticsService.GetAllDrugsAsync();
                return Ok(drugs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all drugs");
                return StatusCode(500, "An error occurred while retrieving drugs");
            }
        }

        [HttpGet("drugs/{drugId}")]
        public async Task<ActionResult<Drug>> GetDrugById(string drugId)
        {
            try
            {
                var drug = await _bioinformaticsService.FindDrugAndRelatedEntitiesAsync(drugId);
                if (drug == null)
                {
                    return NotFound($"Drug with ID {drugId} was not found.");
                }
                return Ok(drug);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving drug with ID {DrugId}", drugId);
                return StatusCode(500, "An error occurred while retrieving the drug");
            }
        }

        // Search endpoints
        [HttpGet("search/diseases")]
        public async Task<ActionResult<IEnumerable<Disease>>> SearchDiseases([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest("Search query cannot be empty");
                }

                var diseases = await _bioinformaticsService.SearchDiseasesAsync(query);
                return Ok(diseases);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching diseases with query {Query}", query);
                return StatusCode(500, "An error occurred while searching diseases");
            }
        }

        [HttpGet("search/genes")]
        public async Task<ActionResult<IEnumerable<Gene>>> SearchGenes([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest("Search query cannot be empty");
                }

                var genes = await _bioinformaticsService.SearchGenesAsync(query);
                return Ok(genes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching genes with query {Query}", query);
                return StatusCode(500, "An error occurred while searching genes");
            }
        }

        [HttpGet("search/drugs")]
        public async Task<ActionResult<IEnumerable<Drug>>> SearchDrugs([FromQuery] string query)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(query))
                {
                    return BadRequest("Search query cannot be empty");
                }

                var drugs = await _bioinformaticsService.SearchDrugsAsync(query);

                return Ok(drugs);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching drugs with query {Query}", query);
                return StatusCode(500, "An error occurred while searching drugs");
            }
        }

        [HttpGet("relationships")]
        public async Task<IActionResult> GetRelationships()
        {
            try
            {
                var relationships = await _bioinformaticsService.GetRelationships();

                return Ok(relationships);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting relationships}");
                return StatusCode(500, "An error occurred while getting relationships");
            }
        }




        [HttpGet("diseases/{diseaseId}/genes")]
        public async Task<ActionResult<IEnumerable<Gene>>> GetGenesForDisease(string diseaseId)
        {
            var genes = await _bioinformaticsService.GetGenesForDiseaseAsync(diseaseId);
            if (!genes.Any()) return NotFound($"No genes found for disease {diseaseId}");
            return Ok(genes);
        }

        [HttpGet("genes/{geneId}/diseases")]
        public async Task<ActionResult<IEnumerable<Disease>>> GetDiseasesForGene(string geneId)
        {
            var diseases = await _bioinformaticsService.GetDiseasesForGeneAsync(geneId);
            if (!diseases.Any()) return NotFound($"No diseases found for gene {geneId}");
            return Ok(diseases);
        }

        [HttpGet("genes/{geneId}/drugs")]
        public async Task<ActionResult<IEnumerable<Drug>>> GetDrugsForGene(string geneId)
        {
            var drugs = await _bioinformaticsService.GetDrugsForGeneAsync(geneId);
            if (!drugs.Any()) return NotFound($"No drugs found for gene {geneId}");
            return Ok(drugs);
        }

        [HttpGet("drugs/{drugId}/genes")]
        public async Task<ActionResult<IEnumerable<Gene>>> GetGenesForDrug(string drugId)
        {
            var genes = await _bioinformaticsService.GetGenesForDrugAsync(drugId);
            if (!genes.Any()) return NotFound($"No genes found for drug {drugId}");
            return Ok(genes);
        }

    }
}
