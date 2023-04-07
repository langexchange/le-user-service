using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure;
using LE.UserService.Neo4jData.DALs;
using Microsoft.EntityFrameworkCore;
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
        private readonly LanggeneralDbContext _context;
        private readonly IMapper _mapper;
        public LanguageService(ILangDAL langDAL, LanggeneralDbContext context, IMapper mapper)
        {
            _langDAL = langDAL;
            _context = context;
            _mapper = mapper;
        }

        public async Task<IEnumerable<Neo4jLangDto>> GetLangsAsync(CancellationToken cancellationToken = default)
        {
            var dtos = await _langDAL.GetLangsAsync(cancellationToken);
            return dtos;
        }

        public async Task<bool> SeedDataAsync(CancellationToken cancellationToken = default)
        {
            var languages = new List<Neo4jLangDto>();
            var langEntites = await _context.Languages.ToListAsync();
            if (!langEntites.Any())
            {
                var filename = "Jsonfiles/language.json";
                var text = File.ReadAllText(filename);
                var langdictionary = JsonConvert.DeserializeObject<Dictionary<string, string>>(text);
                languages = langdictionary.Select(x => new Neo4jLangDto { Id =  System.Guid.NewGuid(), LocaleCode = x.Key.ToUpper(), Name = x.Value }).ToList();
            }
            else
            {
                languages = _mapper.Map<List<Neo4jLangDto>>(langEntites);
            }

            await _langDAL.CreateLangsAsync(languages, cancellationToken);
            return true;
        }
    }
}
