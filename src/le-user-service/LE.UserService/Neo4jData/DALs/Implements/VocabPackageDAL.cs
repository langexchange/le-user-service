using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Neo4jData.DALs.Implements.CypherResults;
using LE.UserService.Neo4jData.DALs.NodeRelationConstants;
using LE.UserService.Neo4jData.DALs.Schemas;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs.Implements
{
    public class VocabPackageDAL : BaseDAL, IVocabPackageDAL
    {
        private readonly ILogger<VocabPackageDAL> _logger;
        public VocabPackageDAL(Neo4jContext context, ILogger<VocabPackageDAL> logger, IMapper mapper) : base(context, mapper)
        {
            _logger = logger;
        }

        public async Task<bool> ConfigVocabPackageAsync(Guid vocabPackageId, bool? isPublish, bool? isDelete, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Write.Merge($"(vp:{VocabPackageSchema.VOCAB_PACKAGE_LABEL} {{ id: $id }})")
               .WithParam("id", vocabPackageId)
               .OnCreate()
               .Set("vp = $vp1")
               .WithParam("vp1", new
               {
                   id = vocabPackageId,
                   isPublic = isPublish,
                   createdAt = DateTime.UtcNow,
               });
            if (isDelete.HasValue)
            {
                cypher = cypher.OnMatch()
                   .Set("vp += $vp2")
                   .WithParam("vp2", new
                   {
                       deletedAt = DateTime.UtcNow,
                   })
                   .With("vp");
            }
            else
            {
                cypher = cypher.OnMatch()
                   .Set("vp += $vp2")
                   .WithParam("vp2", new
                   {
                       isPublic = isPublish,
                       updatedAt = DateTime.UtcNow,
                   })
                   .With("vp");
            }

            var cypherResult = await cypher.Return<VocabPackageSchema>("vp").ResultsAsync;
            var vocabSchema = cypherResult.FirstOrDefault();

            return vocabSchema?.CreatedAt != null;
        }

        public async Task<bool> CreateOrUpdateVocabPackageAsync(VocabularyPackageDto vocabPackageDto, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Write.Merge($"(vp:{VocabPackageSchema.VOCAB_PACKAGE_LABEL} {{ id: $id }})")
              .WithParam("id", vocabPackageDto.PackageId)
              .OnCreate()
              .Set("vp = $vp1")
              .WithParam("vp1", new
              {
                  id = vocabPackageDto.PackageId,
                  title = vocabPackageDto.Title,
                  description = vocabPackageDto.Description,
                  isPublic = vocabPackageDto.IsPublic,
                  termLocale = vocabPackageDto.TermLocale,
                  defineLocale = vocabPackageDto.DefineLocale,
                  vocabularies = JsonConvert.SerializeObject(vocabPackageDto.VocabularyDtos),
                  createdAt = DateTime.UtcNow,
              })
              .OnMatch()
              .Set("vp += $vp2")
              .WithParam("vp2", new
              {
                  title = vocabPackageDto.Title,
                  description = vocabPackageDto.Description,
                  isPublic = vocabPackageDto.IsPublic,
                  termLocale = vocabPackageDto.TermLocale,
                  defineLocale = vocabPackageDto.DefineLocale,
                  vocabularies = JsonConvert.SerializeObject(vocabPackageDto.VocabularyDtos),
                  updatedAt = DateTime.UtcNow,
              })
              .With("vp");
            cypher = cypher.Match($"(u: {UserSchema.USER_LABEL} {{ id : $userId }})")
                .WithParam("userId", vocabPackageDto.UserId)
                .Merge($"(u)-[r:{RelationValues.HAS_VOCAB_PACKAGE}]->(vp)")
                .With("vp");

            var cypherResult = await cypher.Return<VocabPackageSchema>("vp").ResultsAsync;
            var vocabPackageSchema = cypherResult.FirstOrDefault();
            return vocabPackageSchema?.CreatedAt != null;
        }

        public Task<List<UserVocabPackageDto>> FilterVocabByLocaleAsync(string termLocale, string defineLocale, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<UserVocabPackageDto> GetVocabularyPackageAsync(Guid packageId, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Match($"(u:{UserSchema.USER_LABEL})-[:{RelationValues.HAS_VOCAB_PACKAGE}]->(vp:{VocabPackageSchema.VOCAB_PACKAGE_LABEL})")
                        .Where("vp.id = $id")
                        .AndWhere("vp.deletedAt is null")
                        .AndWhere("vp.isPublic = true")
                        .WithParam("id", packageId)
                        .With("u, COLLECT(vp) as vocabularyPackages")
                        .With("{userInfo: u, vocabularyPackages: vocabularyPackages} as result");

            var value = (await cypher.ReturnAsync<VocabPackagesCypherResult>("result", cancellationToken));
            return value.Select(x => _mapper.ToVocabPackageDto(x)).FirstOrDefault();
        }

        public async Task<UserVocabPackageDto> GetVocabularyPackageByUserAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Match($"(u:{UserSchema.USER_LABEL})-[:{RelationValues.HAS_VOCAB_PACKAGE}]->(vp:{VocabPackageSchema.VOCAB_PACKAGE_LABEL})")
                       .Where("u.id = $id")
                       .AndWhere("vp.deletedAt is null")
                       .AndWhere("vp.isPublic = true")
                       .WithParam("id", userId)
                       .With("u, COLLECT(vp) as vocabularyPackages")
                       .With("{userInfo: u, vocabularyPackages: vocabularyPackages} as result");

            var value = (await cypher.ReturnAsync<VocabPackagesCypherResult>("result", cancellationToken));
            return value.Select(x => _mapper.ToVocabPackageDto(x)).FirstOrDefault();
        }

        public Task<List<UserVocabPackageDto>> SuggestVocabAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
