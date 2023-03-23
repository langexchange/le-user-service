using Newtonsoft.Json;

namespace LE.UserService.Neo4jData.DALs.Implements.CypherResults
{
    public class NodeResult<TSchema>
    {
        [JsonProperty("data")]
        public TSchema Data { get; set; }
    }
}
