using AutoMapper;
using LE.Library.Kernel;
using LE.Library.MessageBus;
using LE.UserService.Application.Events;
using LE.UserService.Dtos;
using LE.UserService.Enums;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Infrastructure.Infrastructure.Entities;
using LE.UserService.Models.Requests;
using LE.UserService.Neo4jData.DALs;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services.Implements
{
    public class VocabService : IVocabService
    {
        private LanggeneralDbContext _context;
        private readonly IMapper _mapper;
        private readonly IVocabPackageDAL _vocabPackageDAL;
        private readonly IMessageBus _messageBus;
        private readonly IRequestHeader _requestHeader;

        public VocabService(LanggeneralDbContext context, IMapper mapper, IVocabPackageDAL vocabPackageDAL, IMessageBus messageBus, IRequestHeader requestHeader)
        {
            _context = context;
            _mapper = mapper;
            _vocabPackageDAL = vocabPackageDAL;
            _messageBus = messageBus;
            _requestHeader = requestHeader;
        }

        public async Task<Guid> CloneVocabularyPackageAsync(Guid packageId, Guid userId, CancellationToken cancellationToken = default)
        {
            var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Packageid == packageId && x.IsRemoved == false);
            if(vocabPackage == null)
                return Guid.Empty;

            var newVocabPackage = vocabPackage;
            newVocabPackage.Packageid = Guid.NewGuid();
            newVocabPackage.Userid = userId;
            newVocabPackage.IsPublic = true;
            newVocabPackage.IsRemoved = false;
            newVocabPackage.CreatedAt = DateTime.UtcNow;
            newVocabPackage.UpdatedAt = null;
            newVocabPackage.DeletedAt = null;
            _context.Vocabpackages.Add(newVocabPackage);
            await _context.SaveChangesAsync();

            var vocabDto = _mapper.Map<VocabularyPackageDto>(newVocabPackage);
            await _vocabPackageDAL.CreateOrUpdateVocabPackageAsync(vocabDto, cancellationToken);

            return newVocabPackage.Packageid;
        }

        public async Task<Guid> CreateOrUpdateVocabularyPackageAsync(VocabularyPackageDto dto, CancellationToken cancellationToken = default)
        {
            var vocabPackageEntity = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Packageid == dto.PackageId);
            if(vocabPackageEntity == null)
            {
                dto.PackageId = Guid.NewGuid();
                var vocabPackage = _mapper.Map<Vocabpackage>(dto);

                _context.Vocabpackages.Add(vocabPackage);
            }
            else
            {
                vocabPackageEntity.Name = dto.Title;
                vocabPackageEntity.Description = dto.Description;
                vocabPackageEntity.Term = dto.TermLocale;
                vocabPackageEntity.Define = dto.DefineLocale;
                vocabPackageEntity.IsPublic = dto.IsPublic;
                vocabPackageEntity.Imageurl = dto.ImageUrl;
                vocabPackageEntity.VocabularyPairs = JsonConvert.SerializeObject(dto.VocabularyDtos);
                vocabPackageEntity.UpdatedAt = DateTime.UtcNow;
                _context.Update(vocabPackageEntity);
            }
            await _context.SaveChangesAsync();

            //save in neo4j
            await _vocabPackageDAL.CreateOrUpdateVocabPackageAsync(dto, cancellationToken);

            return vocabPackageEntity == null ? dto.PackageId : vocabPackageEntity.Packageid;
        }

        public async Task<UserVocabPackageDto> GetVocabularyPackageAsync(Guid packageId, CancellationToken cancellationToken = default)
        {
            //var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Packageid == packageId && x.IsRemoved == false);
            //if(vocabPackage == null)
            //    return null;
            //return _mapper.Map<VocabularyPackageDto>(vocabPackage);
            var dto = await _vocabPackageDAL.GetVocabularyPackageAsync(packageId, cancellationToken);
            if(dto == null)
                return null;
            await CalculateProcessPracticeAsync(dto, cancellationToken);
            return dto;
        }

        public async Task<UserVocabPackageDto> GetVocabularyPackageAsync(Guid uId, Guid packageId, CancellationToken cancellationToken = default)
        {
            //var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Packageid == packageId && x.IsRemoved == false);
            //if(vocabPackage == null)
            //    return null;
            //return _mapper.Map<VocabularyPackageDto>(vocabPackage);
            var dto = await _vocabPackageDAL.GetVocabularyPackageAsync(uId, packageId, cancellationToken);
            if (dto == null)
                return null;
            await CalculateProcessPracticeAsync(dto, cancellationToken);
            return dto;
        }

        public async Task<UserVocabPackageDto> GetVocabularyPackagesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            //var vocabPackages = await _context.Vocabpackages.Where(x => x.Userid == userId && x.IsRemoved == false).ToListAsync();
            //if (vocabPackages == null)
            //    return null;
            //return _mapper.Map<List<UserVocabPackageDto>>(vocabPackages);
            var dto = await _vocabPackageDAL.GetVocabularyPackageByUserAsync(userId, cancellationToken);
            if (dto == null)
                return null;
            await CalculateProcessPracticeAsync(dto, cancellationToken);
            return dto;
        }

        private async Task CalculateProcessPracticeAsync(UserVocabPackageDto dto, CancellationToken cancellationToken)
        {
            var vocabularyPackageDtos = new List<VocabularyPackageDto>();
            foreach (var vocabularyPackageDto in dto.vocabularyPackageDtos)
            {
                var practiceResult = await GetPracticeResultAsync(vocabularyPackageDto.PackageId, cancellationToken);
                if (practiceResult != null)
                {
                    vocabularyPackageDto.PracticeResultDto.IsPracticed = true;
                    vocabularyPackageDto.PracticeResultDto.TotalVocabs = practiceResult.TotalVocabs;
                    vocabularyPackageDto.PracticeResultDto.CurrentNumOfVocab = practiceResult.CurrentNumOfVocab;
                }
                vocabularyPackageDtos.Add(vocabularyPackageDto);
            }
            dto.vocabularyPackageDtos = dto.vocabularyPackageDtos.Count != 0? vocabularyPackageDtos : dto.vocabularyPackageDtos;
        }

        public async Task<bool> IsBelongToUser(Guid packageId, Guid userId, CancellationToken cancellationToken = default)
        {
            var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Userid == userId && x.Packageid == packageId);
            if (vocabPackage == null)
                return false;
            return true;
        }

        public async Task SetVocabularyPackageState(Guid packageId, VocabPackageState state, CancellationToken cancellationToken = default)
        {
            var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Packageid == packageId);
            if (vocabPackage == null)
                return;

            switch (state)
            {
                case VocabPackageState.Publish:
                    vocabPackage.IsPublic = true;
                    break;
                case VocabPackageState.Private:
                    vocabPackage.IsPublic = false;
                    break;
                case VocabPackageState.Delete:
                    vocabPackage.IsRemoved = true;
                    vocabPackage.DeletedAt = DateTime.UtcNow;
                    break;
                default:
                    return;
            }
            _context.Update(vocabPackage);
            _context.SaveChanges();
        }

        public async Task<List<UserVocabPackageDto>> SuggestVocabularyPackagesAsync(Guid userId, string[] termLocale, string[] defineLocale, CancellationToken cancellationToken = default)
        {
            var result = await _vocabPackageDAL.SuggestVocabAsync(userId, termLocale, defineLocale, cancellationToken);
            return result;
        }

        public async Task PutInPracticeListAsync(Guid packageId, Guid userId, CancellationToken cancellationToken = default)
        {
            var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.IsRemoved == false && x.Userid == userId && x.Packageid == packageId);
            if (vocabPackage == null)
                return;

            var vocabularies = JsonConvert.DeserializeObject<List<VocabularyDto>>(vocabPackage.VocabularyPairs);

            var vocabEntities = new List<Vocabulary>();
            foreach(var vocabularyPair in vocabularies)
            {
                var vocabEntity = new Vocabulary { 
                    Packageid = packageId, 
                    Vocabid = Guid.NewGuid(), 
                    Front = vocabularyPair.Term, 
                    Back = vocabularyPair.Define, 
                    Imageurl = vocabularyPair.ImageUrl
                };
                vocabEntities.Add(vocabEntity);
            }

            vocabPackage.IsPracticed = true;
            _context.Update(vocabPackage);

            await _context.Vocabularies.AddRangeAsync(vocabEntities);
            _context.SaveChanges();
        }

        public async Task PutOutPracticeListAsync(Guid packageId, Guid userId, CancellationToken cancellationToken = default)
        {
            var vocabularies = await _context.Vocabularies.Where(x => x.Packageid == packageId).ToListAsync();
            var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.IsRemoved == false && x.Userid == userId && x.Packageid == packageId);
            
            if (vocabularies == null || vocabularies.Count == 0 || vocabPackage == null)
                return;

            vocabPackage.IsPracticed = false;
            _context.Update(vocabPackage);

            _context.Vocabularies.RemoveRange(vocabularies);
            await _context.SaveChangesAsync();
        }

        public async Task TrackingPracticeAsync(Guid packageId, List<PracticeVocabTracking> vocabTrackings, CancellationToken cancellationToken = default)
        {
            var tasks = new List<Task>();
            foreach(var vocabularyPair in vocabTrackings)
            {
                tasks.Add(EstimateVocabPracticeAsync(packageId, vocabularyPair.VocabularyId, vocabularyPair.Quality, cancellationToken));
            }
            
            await Task.WhenAll(tasks);
        }

        private async Task EstimateVocabPracticeAsync(Guid packageId, Guid vocabId, int quality, CancellationToken cancellationToken)
        {
            //var vocab = await _context.Vocabularies.FirstOrDefaultAsync(x => x.Vocabid == vocabId && x.Packageid == packageId);
            //fix for by pass
            var vocab = await _context.Vocabularies.FirstOrDefaultAsync(x => x.Vocabid == packageId && x.Packageid == vocabId);
            if (vocab == null)
                return;

            if (quality == 0)
                quality = 1;
            else if (quality == 1)
                quality = 3;
            else quality = 5;
            //super memo 2
            if(quality >= 3)
            {
                if (vocab.Repetitions == 0)
                    vocab.Interval = 1;
                else if (vocab.Repetitions == 1)
                    vocab.Interval = 6;
                else if (vocab.Repetitions > 1)
                {
                    var newInterval = Convert.ToDecimal(vocab.Interval) * vocab.Easiness;
                    vocab.Interval = (int)Math.Ceiling(newInterval.Value);
                }

                vocab.Repetitions += 1;
                var newEasiness = vocab.Easiness + Convert.ToDecimal((0.1 - (5 - quality) * (0.08 + (5 - quality) * 0.02)));
                vocab.Easiness = newEasiness;
            }
            else
            {
                vocab.Interval = 1;
                vocab.Repetitions = 0;
            }

            if(vocab.Easiness < 1.3M)
            {
                vocab.Easiness = 1.3M;
            }

            //update lastlearn and nextlearn
            vocab.LastLearned = DateTime.UtcNow;
            vocab.NextLearned = DateTime.UtcNow.AddDays(Convert.ToDouble(vocab.Interval));
            _context.Update(vocab);
            await _context.SaveChangesAsync();
        }

        public async Task<List<PracticeResultDto>> GetPracticeResultsAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var vocabPackageIds = await _context.Vocabpackages.Where(x => x.Userid == userId && x.IsPracticed == true)
                                .Select(x => x.Packageid).ToListAsync();

            var result = new List<PracticeResultDto>(); 

            foreach (var packageId in vocabPackageIds)
            {
                var dto = await GetPracticeResultAsync(packageId, cancellationToken);
                result.Add(dto);
            }

            return result;
        }

        private async Task<PracticeResultDto> GetPracticeResultAsync(Guid packageId, CancellationToken cancellationToken)
        {
            var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Packageid == packageId && x.IsPracticed == true);
            var vocabs = await _context.Vocabularies.Where(x => x.Packageid == packageId).ToListAsync();
            if (vocabPackage == null || vocabs == null)
                return null;

            var onPracticeVocabs = vocabs.Where(x => x.LastLearned == null || x.NextLearned == null || x.NextLearned <= DateTime.UtcNow).ToList();

            var result = new PracticeResultDto
            {
                PackageId = packageId,
                Title = vocabPackage.Name,
                Description = vocabPackage.Description,
                TotalVocabs = vocabs.Count(),
                CurrentNumOfVocab = onPracticeVocabs.Count()
            };
            return result;
        }

        public async Task<PracticeVocabulariesDto> GetPracticeVocabulariesAsync(Guid packageId, CancellationToken cancellationToken = default)
        {
            var vocabs = await _context.Vocabularies
                        .Where(x => x.Packageid == packageId && (x.LastLearned == null || x.NextLearned == null || x.NextLearned <= DateTime.UtcNow))
                        .ToListAsync();

            if (vocabs == null)
                return null;
            var practiceVocabularies = vocabs.Select(x => new PracticeVocabularyDto { VocabId = x.Vocabid, Term = x.Front, Define = x.Back, ImageUrl = x.Imageurl });
            var result = new PracticeVocabulariesDto
            {
                PackageId = packageId,
                PracticeVocabularies = practiceVocabularies
            };
            return result;
        }

        private async Task WorkAroundNotifyProcess(Guid userId, CancellationToken cancellationToken = default)
        {
            var vocabPackageIds = await _context.Vocabpackages.Where(x => x.Userid == userId && x.IsPracticed == true)
                                .Select(x => x.Packageid).ToListAsync();
            if (vocabPackageIds == null || !vocabPackageIds.Any())
                return;
            var practiceResult = await GetPracticeResultsAsync(userId, cancellationToken);
            var @event = new LearningVocabProcessCalculatedEvent
            {
                UserId = userId,
                Result = practiceResult
            };

            await _messageBus.PublishAsync(@event, _requestHeader);
        }
        
        public async Task StatisticVocabLearningProcessAsync(CancellationToken cancellationToken = default)
        {
            var ids = await _context.Users.Select(x => x.Userid).ToListAsync();
            foreach(var id in ids.ToList())
            {
                await WorkAroundNotifyProcess(id, cancellationToken);
                await Task.Delay(1000);
            }
        }

        public async Task DeleteVocabularyPackageAsync(Guid packageId, CancellationToken cancellationToken = default)
        {
            var vocabPackage = await _context.Vocabpackages.FirstOrDefaultAsync(x => x.Packageid == packageId);
            if (vocabPackage == null)
                return;
            _context.Vocabpackages.Remove(vocabPackage);
            await _context.SaveChangesAsync();

            await _vocabPackageDAL.DeleteVocabPackageAsync(packageId, cancellationToken);
        }
    }
}
