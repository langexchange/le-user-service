using LE.UserService.Dtos;
using LE.UserService.Neo4jData.DALs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.Services.Implements
{
    public class LanguageService : ILanguageService
    {
        private readonly ILangDAL _langDAL;
        public LanguageService(ILangDAL langDAL)
        {
            _langDAL = langDAL;
        }

        public async Task<IEnumerable<Neo4jLangDto>> GetLangsAsync(CancellationToken cancellationToken = default)
        {
            var dtos = await _langDAL.GetLangsAsync(cancellationToken);
            return dtos;
        }

        public async Task<bool> SeedDataAsync(CancellationToken cancellationToken = default)
        {
            var filename = "Jsonfiles/language.json";
            var text = File.ReadAllText(filename);
            var langdictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
            var languages = langdictionary.Select(x => new Neo4jLangDto { LocaleCode = x.Key.ToUpper(), Name = x.Value }).ToList();

            await _langDAL.CreateLangsAsync(languages, cancellationToken);
            return true;
        }
    }
}
