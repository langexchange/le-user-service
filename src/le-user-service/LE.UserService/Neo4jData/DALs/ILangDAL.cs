using LE.UserService.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs
{
    public interface ILangDAL
    {
        Task<IEnumerable<Neo4jLangDto>> GetLangsAsync(CancellationToken cancellationToken = default);
        Task CreateLangsAsync(IEnumerable<Neo4jLangDto> langs, CancellationToken cancellationToken = default);
    }
}
