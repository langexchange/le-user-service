﻿using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Neo4jData.DALs.Schemas;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace LE.UserService.Neo4jData.DALs.Implements.CypherResults
{
    public class UserCypherResult
    {
        [JsonProperty("user")]
        public NodeResult<UserSchema> UserResult { get; set; }

        [JsonProperty("levelNativeLangs")]
        public IEnumerable<LevelLangScheme> NativeLangResult { get; set; }

        [JsonProperty("levelTargetLangs")]
        public IEnumerable<LevelLangScheme> TargetLangsResult { get; set; }
    }

    public static class UserExtension
    {
        public static SuggestUserDto ToUserDto(this IMapper mapper, UserCypherResult source)
        {
            var result = mapper.Map<SuggestUserDto>(source.UserResult?.Data);
            result.NativeLanguage = mapper.Map<LevelNeo4jLangDto>(source.NativeLangResult.FirstOrDefault());
            result.TargetLanguages = mapper.Map<List<LevelNeo4jLangDto>>(source.TargetLangsResult);
            return result;
        }
    }
}
