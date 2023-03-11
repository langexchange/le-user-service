using LE.UserService.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.Services
{
    public interface ICountryService
    {
        Task<bool> SeedDataAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<CountryDto>> CreateCountryAsync(CountryDto country, CancellationToken cancellationToken = default);
    }
}
