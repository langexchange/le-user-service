using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Neo4jData.DALs.Schemas;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs.Implements
{
    public class LangDAL : ILangDAL
    {
        private readonly Neo4jContext _context;
        private readonly ILogger<LangDAL> _logger;
        private readonly IMapper _mapper;
        public LangDAL(Neo4jContext context, ILogger<LangDAL> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task CreateLangsAsync(IEnumerable<Neo4jLangDto> langs, CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Write;
            var index = 0;
            foreach (var lang in langs)
            {
                cypher = cypher.Merge($"(l{index}:{LangSchema.LANGUAGE_LABEL} {{ localeCode: $code{index} }})")
                .WithParam($"code{index}", lang.LocaleCode)
                .OnCreate()
                .Set($"l{index} = $l1{index}")
                .WithParam($"l1{index}", new
                {
                    id = lang.Id,
                    localeCode = lang.LocaleCode,
                    name = lang.Name,
                })
                .OnMatch()
                .Set($"l{index} += $l2{index}")
                .WithParam($"l2{index}", new
                {
                    id = lang.Id,
                    localeCode = lang.LocaleCode,
                    name = lang.Name,
                });
                index ++;
            }

            await cypher.ExecuteWithoutResultsAsync();
        }

        public async Task<IEnumerable<Neo4jLangDto>> GetLangsAsync(CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Read.Match($"(l:{LangSchema.LANGUAGE_LABEL})")
               .Return<LangSchema>("l");

            var langSchema = await cypher.ResultsAsync;

            return _mapper.Map<IEnumerable<Neo4jLangDto>>(langSchema);
        }
    }
}
