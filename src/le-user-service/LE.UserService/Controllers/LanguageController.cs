using LE.UserService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api/language")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private ILangService _langService;
        public LanguageController(ILangService langService)
        {
            _langService = langService;
        }

        [HttpPost]
        public async Task<IActionResult> InitLanguage()
        {
            await _langService.InitLanguage();
            return Ok();
        }
    }
}
