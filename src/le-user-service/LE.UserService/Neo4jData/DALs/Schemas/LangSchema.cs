using Newtonsoft.Json;

namespace LE.UserService.Neo4jData.DALs.Schemas
{
    public class LangSchema
    {
        public const string LANGUAGE_LABEL = "language";

        [JsonProperty("localeCode")]
        public string LocaleCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
