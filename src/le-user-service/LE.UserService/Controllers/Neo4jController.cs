using LE.UserService.Neo4jData.Services;
using LE.UserService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api/seed-data")]
    [ApiController]
    public class Neo4jController : ControllerBase
    {
        private ICountryService _countryService;
        public Neo4jController(ICountryService countryService)
        {
            _countryService = countryService;
        }

        [HttpPost("countries")]
        public async Task<IActionResult> InitCountry(CancellationToken cancellationToken = default)
        {
            await _countryService.SeedDataAsync(cancellationToken);
            return Ok();
        }

        [HttpGet("countries")]
        public async Task<IActionResult> GetCountries(CancellationToken cancellationToken = default)
        {
            var dtos = await _countryService.GetCountriesAsync(cancellationToken);
            return Ok(dtos);
        }
    }
}
