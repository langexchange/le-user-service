using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Enums;
using LE.UserService.Infrastructure.Infrastructure;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services.Implements
{
    public class VocabService : IVocabService
    {
        private LanggeneralDbContext _context;
        private readonly IMapper _mapper;

        public VocabService(LanggeneralDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Guid> CloneVocabularyPackageAsync(Guid packageId, Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<Guid> CreateOrUpdateVocabularyPackageAsync(VocabularyPackageDto dtos, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<VocabularyPackageDto> GetVocabularyPackageAsync(Guid packageId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VocabularyPackageDto>> GetVocabularyPackagesAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<bool> IsBelongToUser(Guid packageId, Guid userId, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task SetVocabularyPackageState(Guid packageId, VocabPackageState state, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public async Task<List<VocabularyPackageDto>> SuggestVocabularyPackagesAsync(Guid userId, string termLocale, string defineLocale, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
