using LE.UserService.Dtos;
using LE.UserService.Enums;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services
{
    public interface IVocabService
    {
        Task<Guid> CreateOrUpdateVocabularyPackageAsync(VocabularyPackageDto dto, CancellationToken cancellationToken = default);
        Task SetVocabularyPackageState(Guid packageId, VocabPackageState state, CancellationToken cancellationToken = default);
        Task<bool> IsBelongToUser(Guid packageId, Guid userId, CancellationToken cancellationToken = default);
        Task<Guid> CloneVocabularyPackageAsync(Guid packageId, Guid userId, CancellationToken cancellationToken = default);
        Task<UserVocabPackageDto> GetVocabularyPackageAsync(Guid packageId, CancellationToken cancellationToken = default);
        Task<UserVocabPackageDto> GetVocabularyPackagesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<UserVocabPackageDto> SuggestVocabularyPackagesAsync(Guid userId, string termLocale, string defineLocale, CancellationToken cancellationToken = default);
    }
}
