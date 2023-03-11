using Newtonsoft.Json;

namespace LE.UserService.Neo4jData.DALs.Schemas
{
    public class CountrySchema
    {
        public const string COUNTRY_LABEL = "country";

        [JsonProperty("countryName")]
        public string Name { get; set; }

        [JsonProperty("countryCode")]
        public string Code { get; set; }

        //[JsonProperty("isActive")]
        //public bool IsActive { get; set; }
    }
}
