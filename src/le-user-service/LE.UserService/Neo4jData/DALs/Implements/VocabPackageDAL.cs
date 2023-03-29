using AutoMapper;
using LE.UserService.Dtos;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Neo4jData.DALs.Implements
{
    public class VocabPackageDAL : BaseDAL, IVocabPackageDAL
    {
        private readonly ILogger<VocabPackageDAL> _logger;
        public VocabPackageDAL(Neo4jContext context, ILogger<VocabPackageDAL> logger, IMapper mapper) : base(context, mapper)
        {
            _logger = logger;
        }

        public Task<bool> ConfigVocabPackageAsync(Guid postId, bool? isPublish, bool? isDelete, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<bool> CreateOrUpdatePostAsync(VocabularyPackageDto vocabPackageDto, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Guid>> FilterVocabByLocaleAsync(string termLocale, string defineLocale, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<List<Guid>> SuggestVocabAsync(Guid id, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
