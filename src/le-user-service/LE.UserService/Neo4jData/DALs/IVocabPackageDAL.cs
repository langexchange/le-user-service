using LE.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs
{
    public interface IVocabPackageDAL
    {
        Task<bool> CreateOrUpdateVocabPackageAsync(VocabularyPackageDto vocabPackageDto, CancellationToken cancellationToken = default);
        Task<bool> ConfigVocabPackageAsync(Guid vocabPackageId, bool? isPublish, bool? isDelete, CancellationToken cancellationToken = default);
        Task<List<UserVocabPackageDto>> FilterVocabByLocaleAsync(string termLocale, string defineLocale, CancellationToken cancellationToken = default);
        Task<List<UserVocabPackageDto>> SuggestVocabAsync(Guid id, CancellationToken cancellationToken = default);
        Task<UserVocabPackageDto> GetVocabularyPackageAsync(Guid packageId, CancellationToken cancellationToken = default);
        Task<UserVocabPackageDto> GetVocabularyPackageByUserAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
