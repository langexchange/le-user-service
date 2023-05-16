using LE.UserService.Dtos;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services
{
    public interface IStatisticLearningService
    {
        Task<List<StatisticLearningDto>> GetStatisticLearningProcessAsync(Guid userId, CancellationToken cancellationToken = default);
    }
}
