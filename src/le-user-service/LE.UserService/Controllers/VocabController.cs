using AutoMapper;
using LE.Library.Kernel;
using LE.UserService.Dtos;
using LE.UserService.Models.Requests;
using LE.UserService.Services;
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
        private readonly IVocabService _vocabService;
        public VocabController(IMapper mapper, IRequestHeader requestHeader, IVocabService vocabService)
        {
            _mapper = mapper;
            _requestHeader = requestHeader;
            _vocabService = vocabService;
        }

        [HttpGet("")]
        public async Task<IActionResult> GetVocabulariesAsync(CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            var vocabs = await _vocabService.GetVocabularyPackagesAsync(uuid, cancellationToken);
            return Ok(vocabs);
        }

        [HttpGet("{vocabularyId}")]
        public async Task<IActionResult> GetVocabularyAsync(Guid vocabularyId, CancellationToken cancellationToken = default)
        {
            var vocab = await _vocabService.GetVocabularyPackageAsync(vocabularyId, cancellationToken);
            return Ok(vocab);
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

            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");

            var dto = _mapper.Map<VocabularyPackageDto>(request);
            dto.UserId = uuid;
            var id = await _vocabService.CreateOrUpdateVocabularyPackageAsync(dto, cancellationToken);
            return Ok(id);
        }

        [HttpPut("{vocabularyId}/update")]
        public async Task<IActionResult> UpdateVocabularyAsync(Guid vocabularyId, VocabularyRequest request, CancellationToken cancellationToken = default)
        {
            if (request == null)
                return BadRequest();

            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");

            var dto = _mapper.Map<VocabularyPackageDto>(request);
            dto.UserId = uuid;
            return Ok();
        }

        [HttpPost("{vocabularyId}/clone")]
        public async Task<IActionResult> CloneVocabularyAsync(Guid vocabularyId, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");

            var id = await _vocabService.CloneVocabularyPackageAsync(vocabularyId, uuid, cancellationToken);
            return Ok(id);
        }


        [HttpGet("api/practice-list")]
        public async Task<IActionResult> GetPracticesAsync(CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");
            return Ok();
        }


        [HttpGet("api/practice-list/vocabularies/{vocabularyId}")]
        public async Task<IActionResult> GetPracticeAsync(Guid vocabularyId, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");
            return Ok();
        }

        [HttpPut("api/practice-list/vocabularies/{vocabularyId}/tracking")]
        public async Task<IActionResult> TrackingPracticeAsync(Guid vocabularyId, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");
            return Ok();
        }

        [HttpPost("{vocabularyId}/put-in-practice-list")]
        public async Task<IActionResult> PutInPracticeAsync(Guid vocabularyId, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");

            var id = await _vocabService.CloneVocabularyPackageAsync(vocabularyId, uuid, cancellationToken);
            return Ok(id);
        }

        [HttpPost("{vocabularyId}/put-out-practice-list")]
        public async Task<IActionResult> PutOutPracticeAsync(Guid vocabularyId, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");

            var id = await _vocabService.CloneVocabularyPackageAsync(vocabularyId, uuid, cancellationToken);
            return Ok(id);
        }
    }
}
