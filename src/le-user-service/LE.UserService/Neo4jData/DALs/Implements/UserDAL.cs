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

        public async Task<List<SuggestUserDto>> GetUsersAsync(List<Guid> ids, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Read
                                .Match($"(u: {UserSchema.USER_LABEL})")
                                .Where($"u.id IN $ids")
                                .WithParam("ids", ids.ToArray())
                                .OptionalMatch($"(u)-[nRel:{RelationValues.HAS_NATIVE_LANGUAGE}]->(ntl:{LangSchema.LANGUAGE_LABEL})")
                                .OptionalMatch($"(u)-[tRel:{RelationValues.HAS_TARGET_LANGUAGE}]->(tgl:{LangSchema.LANGUAGE_LABEL})")
                                .With("u, COLLECT(distinct {level: nRel.level, localeCode: ntl.localeCode, name: ntl.name}) as levelNativeLangs, COLLECT(distinct {level: tRel.level, localeCode: tgl.localeCode, name: tgl.name}) as levelTargetLangs")
                                .With("{user: u, levelNativeLangs: levelNativeLangs, levelTargetLangs: levelTargetLangs } as result");

            var value = await cypher.ReturnAsync<UserCypherResult>("result", cancellationToken);

            return value.Select(c => _mapper.ToUserDto(c)).ToList();
        }

        public async Task<List<SuggestUserDto>> GetUsersAsync(Guid urequestId, List<Guid> ids, CancellationToken cancellationToken = default)
        {
            var result = await GetUsersAsync(ids, cancellationToken);
            //result.ForEach(async x =>
            //{
            //    x.IsFriend = await IsFriendAsync(urequestId, x.Id, cancellationToken);
            //});
            foreach(var user in result)
            {
                user.IsFriend = await IsFriendAsync(urequestId, user.Id, cancellationToken);
            }

            return result;
        }

        private async Task<bool> IsFriendAsync(Guid id1, Guid id2, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Read
                        .Match($"(u1: {UserSchema.USER_LABEL} {{ id: $fromId}} )")
                        .WithParam("fromId", id1)
                        .Match($"(u2: {UserSchema.USER_LABEL} {{ id: $toId}} )")
                        .WithParam("toId", id2)
                        .Return<bool>($"EXISTS ( (u1)-[:{RelationValues.HAS_FRIEND}]-(u2) )");

            var result = await cypher.ResultsAsync;

            if (result == null)
                return false;
            return result.FirstOrDefault();
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
            cypher = cypher.Match($"(c:{CountrySchema.COUNTRY_LABEL} {{countryCode: $countryCode}})")
                    .WithParam("countryCode", userDto.Country)
                    .Merge($"(u)-[:{RelationValues.LIVE_IN_COUNTRY}]->(c)")
                    .With("u");

            cypher = cypher.OptionalMatch($"(u)-[rnl:{RelationValues.HAS_NATIVE_LANGUAGE}]->(:{LangSchema.LANGUAGE_LABEL})")
               .DetachDelete("rnl")
               .With("u");
            cypher = cypher.OptionalMatch($"(u)-[rtl:{RelationValues.HAS_TARGET_LANGUAGE}]->(:{LangSchema.LANGUAGE_LABEL})")
               .DetachDelete("rtl")
               .With("u");

            cypher = cypher.Match($"(nl:{LangSchema.LANGUAGE_LABEL} {{id: $nlId}})")
                    .WithParam("nlId", userDto.NativeLanguage.Id)
                    .Merge($"(u)-[:{RelationValues.HAS_NATIVE_LANGUAGE} {{level: $level}}]->(nl)")
                    .WithParam("level", userDto.NativeLanguage.Level)
                    .With("u");


            foreach (var lang in targetLangs)
            {
                cypher = cypher.Match($"(tl{index}:{LangSchema.LANGUAGE_LABEL} {{id: $tlId{index}}})")
                    .WithParam($"tlId{index}", lang.Id)
                    .Merge($"(u)-[:{RelationValues.HAS_TARGET_LANGUAGE} {{level: $level{index}}}]->(tl{index})")
                    .WithParam($"level{index}", lang.Level)
                    .With("u");
                index++;
            }

            var cypherResult = cypher.Return<UserSchema>("u");
            var userSchema = (await cypherResult.ResultsAsync).FirstOrDefault();

            return userSchema?.CreatedAt != null;
        }

        public async Task<IEnumerable<SuggestUserDto>> SuggestFriendsAsync(Guid id, string[] naviveLangs, string[] targetLangs, string[] countryCodes, CancellationToken cancellationToken)
        {
            var userCypher = _context.Cypher.Read.Match($"(u:{UserSchema.USER_LABEL} {{ id: $id}})")
                             .WithParam("id", id);
                             
            var userSchema = (await userCypher.Return<UserSchema>("u").ResultsAsync).FirstOrDefault();

            userCypher = userCypher.Match($"(u)-[:{RelationValues.HAS_TARGET_LANGUAGE}]->(l:{LangSchema.LANGUAGE_LABEL})")
                                   .With($"collect(l.localeCode) as targetLangs");
            
            var userTargetLangs = (await userCypher.ReturnDistinct<string[]>("targetLangs").ResultsAsync)?.FirstOrDefault();

            var ids = new List<Guid>();
            var cypher = _context.Cypher.Read;
            IEnumerable<SuggestUserDto> result = null;

            if (naviveLangs.Count() > 0 || targetLangs.Count() > 0 || countryCodes.Count() > 0)
            {
                if (naviveLangs.Count() != 0)
                {
                    var nlangcypher = cypher.Match($"(u:{UserSchema.USER_LABEL})-[:{RelationValues.HAS_NATIVE_LANGUAGE}]->(l:{LangSchema.LANGUAGE_LABEL})")
                            .Where("l.localeCode in $localeCodes")
                            .WithParam("localeCodes", naviveLangs);
                    var cypherResult = (await nlangcypher.Return<Guid>("u.id").ResultsAsync).ToList();
                    ids.AddRange(cypherResult);
                }
                if (targetLangs.Count() != 0)
                {
                    var tlangcypher = cypher.Match($"(u:{UserSchema.USER_LABEL})-[:{RelationValues.HAS_TARGET_LANGUAGE}]->(l:{LangSchema.LANGUAGE_LABEL})")
                            .Where("l.localeCode in $localeCodes")
                            .WithParam("localeCodes", targetLangs);
                    var cypherResult = (await tlangcypher.Return<Guid>("u.id").ResultsAsync).ToList();
                    ids.AddRange(cypherResult);
                }
                if (countryCodes.Count() != 0)
                {
                    var ccypher = cypher.Match($"(u:{UserSchema.USER_LABEL})-[:{RelationValues.LIVE_IN_COUNTRY}]->(c:{CountrySchema.COUNTRY_LABEL})")
                            .Where("c.countryCode in $countryCodes")
                            .WithParam("countryCodes", countryCodes);
                    var cypherResult = (await ccypher.Return<Guid>("u.id").ResultsAsync).ToList();
                    ids.AddRange(cypherResult);
                }

            }
            else
            {
                cypher = cypher.Match($"(u:{UserSchema.USER_LABEL})-[:{RelationValues.LIVE_IN_COUNTRY}]->(:{CountrySchema.COUNTRY_LABEL} {{countryCode: $countryCode}})")
                        .WithParam("countryCode", userSchema.Country);
                var cypherResult = (await cypher.Return<Guid>("u.id").ResultsAsync).ToList();
                ids.AddRange(cypherResult);

                if(userTargetLangs != null && userTargetLangs.Length != 0)
                {
                    cypher = cypher.Match($"(u)-[:{RelationValues.HAS_TARGET_LANGUAGE}]->(l:{LangSchema.LANGUAGE_LABEL})")
                            .Where("l.localeCode in $localeCodes")
                            .WithParam("localeCodes", userTargetLangs);
                    var tlCypherResult = (await cypher.Return<Guid>("u.id").ResultsAsync).ToList();
                    ids.AddRange(tlCypherResult);
                }
            }

            ids = ids.Distinct().ToList();
            ids.Remove(id);
            foreach(var uid in ids.ToList())
            {
                if (await IsFriendAsync(id, uid, cancellationToken))
                    ids.Remove(uid);
            }

            result = await GetUsersAsync(id, ids, cancellationToken);
            return result;
        }
    }
}
