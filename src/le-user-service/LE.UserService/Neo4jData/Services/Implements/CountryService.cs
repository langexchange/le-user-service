using LE.UserService.Dtos;
using LE.UserService.Neo4jData.DALs;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.Services.Implements
{
    class CountryService : ICountryService
    {
        private readonly ICountryDAL _countryDAL;

        public CountryService(ICountryDAL countryDAL)
        {
            _countryDAL = countryDAL;
        }

        public async Task<IEnumerable<CountryDto>> CreateCountryAsync(CountryDto country, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            var countryDto = await _countryDAL.CreateCountryAsync(country);
            return countryDto;
        }

        public async Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken cancellationToken = default)
        {
            var dtos = await _countryDAL.GetCountriesAsync(cancellationToken);
            return dtos;
        }

        public async Task<bool> SeedDataAsync(CancellationToken cancellationToken = default)
        {

            var seedDefaultCountryJson = "Jsonfiles/countrys.json";

            if (File.Exists(seedDefaultCountryJson))
            {
                using (var stream = new StreamReader(seedDefaultCountryJson))
                {
                    var fileContent = stream.ReadToEnd();
                    var initDefaultCountries = JsonConvert.DeserializeObject<List<CountryDto>>(fileContent);

                    var initCountryTasks = new List<Task>();
                    foreach (var initDefaultCountry in initDefaultCountries)
                    {
                        initCountryTasks.Add(CreateCountryAsync(initDefaultCountry));
                    }
                    await Task.WhenAll(initCountryTasks);
                }
                return true;
            }
            return false;
        }
    }
}
