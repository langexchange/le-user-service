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
        private ILanguageService _languageService;
        public Neo4jController(ICountryService countryService, ILanguageService languageService)
        {
            _countryService = countryService;
            _languageService = languageService;
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


        [HttpPost("languages")]
        public async Task<IActionResult> InitLanguages(CancellationToken cancellationToken = default)
        {
            await _languageService.SeedDataAsync(cancellationToken);
            return Ok();
        }

        [HttpGet("languages")]
        public async Task<IActionResult> GetLanguages(CancellationToken cancellationToken = default)
        {
            var dtos = await _languageService.GetLangsAsync(cancellationToken);
            return Ok(dtos);
        }
    }
}
