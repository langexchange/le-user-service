using LE.UserService.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.Services
{
    public interface ILanguageService
    {
        Task<bool> SeedDataAsync(CancellationToken cancellationToken = default);
        Task<IEnumerable<Neo4jLangDto>> GetLangsAsync(CancellationToken cancellationToken = default);
    }
}
