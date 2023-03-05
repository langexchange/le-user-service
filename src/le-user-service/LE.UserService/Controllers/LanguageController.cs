using AutoMapper;
using LE.UserService.Models.Responses;
using LE.UserService.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api/languages")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private ILangService _langService;
        private readonly IMapper _mapper;
        public LanguageController(ILangService langService, IMapper mapper)
        {
            _langService = langService;
            _mapper = mapper;
        }

        [HttpPost]
        public async Task<IActionResult> InitLanguage()
        {
            await _langService.InitLanguage();
            return Ok();
        }

        [HttpGet("")]
        public async Task<IActionResult> GetLanguages(CancellationToken cancellationToken = default)
        {
            var dtos = await _langService.GetLanguages(cancellationToken);
            var response = _mapper.Map<List<LangResponse>>(dtos);
            return Ok(response);
        }
    }
}
