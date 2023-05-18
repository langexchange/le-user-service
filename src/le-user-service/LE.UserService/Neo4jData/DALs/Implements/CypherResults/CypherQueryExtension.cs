using Neo4jClient.Cypher;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs.Implements.CypherResults
{
    public static class CypherQueryExtension
    {
        public static async Task<IEnumerable<T>> ReturnAsync<T>(this ICypherFluentQuery cypher, string identity, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            cypher = cypher.With($"{{ payload: {identity} }} as value");

            var returnCypher = cypher.ReturnDistinct<string>("value");

            var jsonResult = await returnCypher.ResultsAsync;

            if (jsonResult == null)
                return Enumerable.Empty<T>();

            var nodesResult = jsonResult//.Select(c => JsonConvert.DeserializeObject<NodeResult<T>>(c)).Select(c => c.Data);
                .Select(c =>
                {
                    var payloadResult = JsonConvert.DeserializeObject<Dictionary<string, JObject>>(c);

                    if (!payloadResult.TryGetValue("payload", out var value))
                    {
                        return default;
                    }

                    //if (typeof(CatalogDynamic).IsAssignableFrom(typeof(T)))
                    //{
                    //    var nodeResult = JsonConvert.DeserializeObject<NodeResult<T>>(value.ToString());
                    //    return nodeResult == null ? default(T) : nodeResult.Data;
                    //}
                    return JsonConvert.DeserializeObject<T>(value.ToString());
                }).Where(c => c != null);//.Select(c => c.Data);

            return nodesResult;
        }
    }
}
