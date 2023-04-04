using AutoMapper;
using LE.Library.Kernel;
using LE.UserService.Dtos;
using LE.UserService.Models.Requests;
using LE.UserService.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
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
        public async Task<IActionResult> GetVocabulariesExploreAsync([FromQuery] string[] terms, [FromQuery] string[] defines, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            var termLocales = terms.Select(x => x.ToUpper()).ToArray();
            var defineLocales = defines.Select(x => x.ToUpper()).ToArray();

            var response = await _vocabService.SuggestVocabularyPackagesAsync(uuid, termLocales, defineLocales, cancellationToken);
            return Ok(response);
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
            dto.TermLocale = dto.TermLocale?.ToUpper();
            dto.DefineLocale = dto.DefineLocale?.ToUpper();
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
            dto.PackageId = vocabularyId;
            dto.TermLocale = dto.TermLocale?.ToUpper();
            dto.DefineLocale = dto.DefineLocale?.ToUpper();
            await _vocabService.CreateOrUpdateVocabularyPackageAsync(dto, cancellationToken);
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


        [HttpGet("/api/practice-list/overview")]
        public async Task<IActionResult> GetPracticesAsync(CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");

            var response = await _vocabService.GetPracticeResultsAsync(uuid, cancellationToken);
            return Ok(response);
        }


        [HttpGet("/api/practice-list/vocabularies/{vocabularyId}")]
        public async Task<IActionResult> GetPracticeAsync(Guid vocabularyId, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");

            var response = await _vocabService.GetPracticeVocabulariesAsync(vocabularyId, cancellationToken);
            return Ok(response);
        }

        [HttpPut("/api/practice-list/vocabularies/{vocabularyId}/tracking")]
        public async Task<IActionResult> TrackingPracticeAsync(Guid vocabularyId, PracticeTrackingRequest request, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");
            if (!request.IsValid())
                return BadRequest("Quality must be in range 0-5");

            await _vocabService.TrackingPracticeAsync(vocabularyId, request.VocabTrackings, cancellationToken);
            return Ok();
        }

        [HttpPost("{vocabularyId}/put-in-practice-list")]
        public async Task<IActionResult> PutInPracticeAsync(Guid vocabularyId, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");

            await _vocabService.PutInPracticeListAsync(vocabularyId, uuid, cancellationToken);
            return Ok();
        }

        [HttpPost("{vocabularyId}/put-out-practice-list")]
        public async Task<IActionResult> PutOutPracticeAsync(Guid vocabularyId, CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            if (uuid == Guid.Empty)
                return BadRequest("Require Access token");

            await _vocabService.PutOutPracticeListAsync(vocabularyId, uuid, cancellationToken);
            return Ok();
        }
    }
}
