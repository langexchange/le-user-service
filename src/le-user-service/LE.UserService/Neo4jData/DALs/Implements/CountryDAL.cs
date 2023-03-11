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
    public class CountryDAL : ICountryDAL
    {
        private readonly Neo4jContext _context;
        private readonly ILogger<CountryDAL> _logger;
        private readonly IMapper _mapper;
        public CountryDAL(Neo4jContext context, ILogger<CountryDAL> logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CountryDto>> CreateCountryAsync(CountryDto country, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("CreateCountry...country: {0}", JsonConvert.SerializeObject(country));

            var countryCode = country.Code.ToLower();
            var cypher = _context.Cypher.Write.Merge($"(c:{CountrySchema.COUNTRY_LABEL} {{ countryCode: $code }})")
                .WithParam("code", countryCode)
                .OnCreate()
                .Set("c = $country1")
                .WithParam("country1", new
                {
                    countryName = country.Name,
                    countryCode = countryCode,
                    isActive = true
                })
                .OnMatch()
                .Set("c += $country2")
                .WithParam("country2", new
                {
                    countryName = country.Name,
                    countryCode = countryCode,
                    isActive = true
                })
                .Return<CountrySchema>("c");

            var countrySchema = await cypher.ResultsAsync;

            return _mapper.Map<IEnumerable<CountryDto>>(countrySchema);
        }

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken cancellationToken = default)
        {
            var cypher = _context.Cypher.Read.Match($"(c:{CountrySchema.COUNTRY_LABEL})")
                .Return<CountrySchema>("c");

            var countrySchema = await cypher.ResultsAsync;

            return _mapper.Map<IEnumerable<CountryDto>>(countrySchema);
        }
    }
}
