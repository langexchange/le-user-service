using LE.UserService.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs
{
    public interface ICountryDAL
    {
        Task<IEnumerable<CountryDto>> GetCountriesAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<CountryDto>> CreateCountryAsync(CountryDto country, CancellationToken cancellationToken = default);
    }
}
