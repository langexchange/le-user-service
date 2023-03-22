using Newtonsoft.Json;
using System.Collections.Generic;

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

    public class LevelLangScheme
    {
        [JsonProperty("level")]
        public string Level { get; set; }

        [JsonProperty("localeCode")]
        public string LocaleCode { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
