using LE.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs
{
    public interface IVocabPackageDAL
    {
        Task<bool> CreateOrUpdatePostAsync(VocabularyPackageDto vocabPackageDto, CancellationToken cancellationToken = default);
        Task<bool> ConfigVocabPackageAsync(Guid postId, bool? isPublish, bool? isDelete, CancellationToken cancellationToken = default);
        Task<List<Guid>> FilterVocabByLocaleAsync(string termLocale, string defineLocale, CancellationToken cancellationToken = default);
        Task<List<Guid>> SuggestVocabAsync(Guid id, CancellationToken cancellationToken = default);
    }
}
