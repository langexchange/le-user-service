using Newtonsoft.Json;
using System;

namespace LE.UserService.Neo4jData.DALs.Schemas
{
    public class VocabPackageSchema
    {
        public const string VOCAB_PACKAGE_LABEL = "vocabpackage";

        [JsonProperty("id")]
        public Guid Packageid { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("isPublic")]
        public bool IsPublic { get; set; }

        [JsonProperty("termLocale")]
        public string TermLocale { get; set; }

        [JsonProperty("defineLocale")]
        public string DefineLocale { get; set; }

        [JsonProperty("vocabularies")]
        public string Vocabularies { get; set; }

        [JsonProperty("createdAt")]
        public DateTime? CreatedAt { get; set; }

        [JsonProperty("updatedAt")]
        public DateTime? UpdatedAt { get; set; }

        [JsonProperty("deletedAt")]
        public DateTime? DeletedAt { get; set; }
    }
}
