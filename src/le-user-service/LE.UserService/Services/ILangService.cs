using LE.UserService.Dtos;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services
{
    public interface ILangService
    {
        Task InitLanguage();
        Task<List<LanguageDto>> GetLanguages(CancellationToken cancellationToken);
    }
}
