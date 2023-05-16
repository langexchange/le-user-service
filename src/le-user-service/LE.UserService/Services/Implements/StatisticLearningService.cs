using AutoMapper;
using LE.UserService.Dtos;
using LE.UserService.Infrastructure.Infrastructure;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Services.Implements
{
    public class StatisticLearningService : IStatisticLearningService
    {
        private LanggeneralDbContext _context;

        public StatisticLearningService(LanggeneralDbContext context)
        {
            _context = context;
        }

        public async Task<List<StatisticLearningDto>> GetStatisticLearningProcessAsync(Guid userId, CancellationToken cancellationToken = default)
        {
            var statisticLearningEntity = await _context.Statisticallearningprocesses.Where(x => x.Userid == userId).ToListAsync();
            if (!statisticLearningEntity.Any())
                return null;

            var statisticLearningsOfThisYear = statisticLearningEntity.Where(x => x.UpdatedAt.Value.Year == DateTime.UtcNow.Year).ToList();

            var result = new List<StatisticLearningDto>();
            foreach(var x in statisticLearningsOfThisYear)
            {
                result.Add(new StatisticLearningDto
                {
                    Month = x.UpdatedAt.Value.Month,
                    Percent = x.Percent,
                    Totalvocabs = x.Totalvocabs,
                    Currentvocabs = x.Currentvocabs,
                    CreatedAt = x.CreatedAt,
                    UpdatedAt = x.UpdatedAt
                });
            }

            return result;
        }
    }
}
