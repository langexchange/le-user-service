using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Neo4jData.DALs.Schemas;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;

namespace LE.UserService.Neo4jData.DALs.Implements.CypherResults
{
    public class VocabPackagesCypherResult
    {
        [JsonProperty("vocabularyPackages")]
        public IEnumerable<VocabPackageSchema> VocabPackageResult { get; set; }

        [JsonProperty("userInfo")]
        public NodeResult<UserSchema> UserInfo { get; set; }
    }

    public static class VocabPackagesExtension
    {
        public static UserVocabPackageDto ToVocabPackageDto(this IMapper mapper, VocabPackagesCypherResult source)
        {
            var vocabularyPackageDtos = mapper.Map<List<VocabularyPackageDto>>(source.VocabPackageResult);
            UserInfo userInfo = null;
            var userData = source.UserInfo?.Data;
            if (userData != null)
                userInfo = new UserInfo { Id = userData.Id, Avatar = userData.Avatar, FirstName = userData.FirstName, LastName = userData.LastName };
            
            return new UserVocabPackageDto { UserInfo = userInfo, vocabularyPackageDtos = vocabularyPackageDtos};
        }
    }
}
