using LifeMapsDemo.Services;
using Microsoft.AspNetCore.Mvc;

namespace LifeMapsDemo.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImportBioDataController : ControllerBase
    {
        private readonly ImportDataService _importDataService;

        public ImportBioDataController(ImportDataService importDataService)
        {
            _importDataService = importDataService;
        }

        [HttpPost("import")]
        public void ImportData()
        {
            _importDataService.ImportDataAsync();

            //return Ok();// new { message = "Data imported ok!" });
        }
    }
}
