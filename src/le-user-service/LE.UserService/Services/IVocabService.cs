﻿using LE.UserService.Dtos;
using LE.UserService.Enums;
using LE.UserService.Models.Requests;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services
{
    public interface IVocabService
    {
        Task<Guid> CreateOrUpdateVocabularyPackageAsync(VocabularyPackageDto dto, CancellationToken cancellationToken = default);
        Task DeleteVocabularyPackageAsync(Guid packageId, CancellationToken cancellationToken = default);
        Task SetVocabularyPackageState(Guid packageId, VocabPackageState state, CancellationToken cancellationToken = default);
        Task<bool> IsBelongToUser(Guid packageId, Guid userId, CancellationToken cancellationToken = default);
        Task<Guid> CloneVocabularyPackageAsync(Guid packageId, Guid userId, CancellationToken cancellationToken = default);
        Task<UserVocabPackageDto> GetVocabularyPackageAsync(Guid packageId, CancellationToken cancellationToken = default);
        Task<UserVocabPackageDto> GetVocabularyPackagesAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<List<UserVocabPackageDto>> SuggestVocabularyPackagesAsync(Guid userId, string[] termLocale, string[] defineLocale, CancellationToken cancellationToken = default);
        Task PutInPracticeListAsync(Guid packageId, Guid userId, CancellationToken cancellationToken = default);
        Task PutOutPracticeListAsync(Guid packageId, Guid userId, CancellationToken cancellationToken = default);
        Task TrackingPracticeAsync(Guid packageId, List<PracticeVocabTracking> vocabTrackings, CancellationToken cancellationToken = default);
        Task<List<PracticeResultDto>> GetPracticeResultsAsync(Guid userId, CancellationToken cancellationToken = default);
        Task<PracticeVocabulariesDto> GetPracticeVocabulariesAsync(Guid packageId, CancellationToken cancellationToken = default);

        Task StatisticVocabLearningProcessAsync(CancellationToken cancellationToken = default);
    }
}
