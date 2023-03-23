using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Enums;
using LE.UserService.Neo4jData.DALs.Implements.CypherResults;
using LE.UserService.Neo4jData.DALs.NodeRelationConstants;
using LE.UserService.Neo4jData.DALs.Schemas;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs.Implements
{
    public class UserDAL : BaseDAL, IUserDAL
    {
        private readonly ILogger<UserDAL> _logger;
        public UserDAL(Neo4jContext context, ILogger<UserDAL> logger, IMapper mapper):base(context, mapper)
        {
            _logger = logger;
        }

        public async Task<bool> ChangeAvatarAsync(Guid id, string avatar, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Write.Merge($"(u:{UserSchema.USER_LABEL} {{ id: $id }})")
               .WithParam("id", id)
               .OnCreate()
               .Set("u = $u1")
               .WithParam("u1", new
               {
                   id = id,
                   avatar = avatar,
                   createdAt = DateTime.UtcNow,
               })
               .OnMatch()
               .Set("u += $u2")
               .WithParam("u2", new
               {
                   avatar = avatar,
                   updatedAt = DateTime.UtcNow,
               })
               .Return<UserSchema>("u");
            var userSchema = (await cypher.ResultsAsync).FirstOrDefault();

            return userSchema?.CreatedAt != null;
        }

        public async Task CrudFriendRelationshipAsync(Guid fromId, Guid toId, string relation, ModifiedState mode, CancellationToken cancellationToken)
        {
            var cypher = _context.Cypher.Write.Match($"(u1: {UserSchema.USER_LABEL} {{ id: $fromId}} )")
                                              .WithParam("fromId", fromId)
                                              .Match($"(u2: {UserSchema.USER_LABEL} {{ id: $toId}} )")
                                              .WithParam("toId", toId);
            switch (mode)
            {
                case ModifiedState.Create:
                    cypher = cypher.Merge($"(u1)-[:{relation}]->(u2)");
                    break;
                case ModifiedState.Delete:
                    cypher = cypher.Match($"(u1)-[r:{relation}]->(u2)")
                                   .Delete("r");
                    break;
            }

            await cypher.ExecuteWithoutResultsAsync();
        }

        public async Task<IEnumerable<SuggestUserDto>> GetUsersAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Read
                                .Match($"(u: {UserSchema.USER_LABEL})")
                                .Where($"u.id IN $ids")
                                .WithParam("ids", ids.ToArray())
                                .OptionalMatch($"(u)-[nRel:{RelationValues.HAS_NATIVE_LANGUAGE}]->(ntl:{LangSchema.LANGUAGE_LABEL})")
                                .OptionalMatch($"(u)-[tRel:{RelationValues.HAS_TARGET_LANGUAGE}]->(tgl:{LangSchema.LANGUAGE_LABEL})")
                                .With("u, COLLECT(distinct {level: nRel.level, localeCode: ntl.localeCode, name: ntl.name}) as levelNativeLangs, COLLECT(distinct {level: nRel.level, localeCode: tgl.localeCode, name: tgl.name}) as levelTargetLangs")
                                .With("{user: u, levelNativeLangs: levelNativeLangs, levelTargetLangs: levelTargetLangs } as result");

            var value = await cypher.ReturnAsync<UserCypherResult>("result", cancellationToken);

            return value.Select(c => _mapper.ToUserDto(c));
        }

        public async Task<bool> SetBasicInforAsync(Guid id, UserDto userDto, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Write.Merge($"(u:{UserSchema.USER_LABEL} {{ id: $id }})")
                .WithParam("id", id)
                .OnCreate()
                .Set("u = $u1")
                .WithParam("u1", new
                {
                    id = id,
                    firstName = userDto.FirstName,
                    middleName = userDto.MiddleName,
                    lastName = userDto.LastName,
                    gender = userDto.Gender,
                    introduction = userDto.Introduction,
                    country = userDto.Country,
                    hobbies = userDto.Hobbies,
                    avatar = userDto.Avatar,
                    createdAt = DateTime.UtcNow,
                })
                .OnMatch()
                .Set("u += $u2")
                .WithParam("u2", new
                {
                    firstName = userDto.FirstName,
                    middleName = userDto.MiddleName,
                    lastName = userDto.LastName,
                    gender = userDto.Gender,
                    introduction = userDto.Introduction,
                    country = userDto.Country,
                    hobbies = userDto.Hobbies,
                    avatar = userDto.Avatar,
                    updatedAt = DateTime.UtcNow,
                })
                .With("u");


            //handle relations
            var index = 0;
            var targetLangs = userDto.TargetLanguages;

            cypher = cypher.OptionalMatch($"(u)-[r:{RelationValues.LIVE_IN_COUNTRY}]->(:{CountrySchema.COUNTRY_LABEL})")
                .DetachDelete("r")
                .With("u");
            cypher = cypher.Merge($"(u)-[:{RelationValues.LIVE_IN_COUNTRY}]->(:{CountrySchema.COUNTRY_LABEL} {{countryName: $countryName}})")
                    .WithParam("countryName", userDto.Country)
                    .With("u");

            cypher = cypher.OptionalMatch($"(u)-[rnl:{RelationValues.HAS_NATIVE_LANGUAGE}]->(:{LangSchema.LANGUAGE_LABEL})")
               .DetachDelete("rnl")
               .With("u");
            cypher = cypher.OptionalMatch($"(u)-[rtl:{RelationValues.HAS_TARGET_LANGUAGE}]->(:{LangSchema.LANGUAGE_LABEL})")
               .DetachDelete("rtl")
               .With("u");

            cypher = cypher.Match($"(nl:{LangSchema.LANGUAGE_LABEL} {{name: $name}})")
                    .WithParam("name", userDto.NativeLanguage.Name)
                    .Merge($"(u)-[:{RelationValues.HAS_NATIVE_LANGUAGE} {{level: $level}}]->(nl)")
                    .WithParam("level", userDto.NativeLanguage.Level)
                    .With("u");


            foreach (var lang in targetLangs)
            {
                cypher = cypher.Match($"(tl{index}:{LangSchema.LANGUAGE_LABEL} {{name: $name{index}}})")
                    .WithParam($"name{index}", lang.Name)
                    .Merge($"(u)-[:{RelationValues.HAS_TARGET_LANGUAGE} {{level: $level{index}}}]->(tl{index})")
                    .WithParam($"level{index}", lang.Level)
                    .With("u");
                index++;
            }

            var cypherResult = cypher.Return<UserSchema>("u");
            var userSchema = (await cypherResult.ResultsAsync).FirstOrDefault();

            return userSchema?.CreatedAt != null;
        }

        public Task<IEnumerable<Dictionary<string, object>>> SuggestFriendsAsync(Guid id, string[] naviveLangs, string[] targetLangs, string[] countryCodes, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}
