using AutoMapper;
using LE.Library.Kernel;
using LE.UserService.Models.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api/vocabularies")]
    [ApiController]
    public class VocabController : ControllerBase
    {
        private readonly IRequestHeader _requestHeader;
        private readonly IMapper _mapper;
        public VocabController(IMapper mapper, IRequestHeader requestHeader)
        {
            _mapper = mapper;
            _requestHeader = requestHeader;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetVocabulariesAsync(CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            return Ok();
        }

        [HttpGet("explore")]
        public async Task<IActionResult> GetVocabulariesExploreAsync(string term, string define, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            return Ok();
        }
         
        [HttpPost("/api/vocabulary/create")]
        public async Task<IActionResult> CreateVocabularyAsync(VocabularyRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                return BadRequest();

            return Ok();
        }

        [HttpPut("{vocabularyId}/update")]
        public async Task<IActionResult> UpdateVocabularyAsync(Guid vocabularyId, VocabularyRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                return BadRequest();

            return Ok();
        }


    }
}
