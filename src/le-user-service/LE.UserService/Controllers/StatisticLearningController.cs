using LE.Library.Kernel;
using LE.UserService.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Controllers
{
    [Route("api/statistics")]
    [ApiController]
    public class StatisticLearningController : ControllerBase
    {
        private readonly IRequestHeader _requestHeader;
        private readonly IStatisticLearningService _statisticLearningService;

        public StatisticLearningController(IRequestHeader requestHeader, IStatisticLearningService statisticLearningService)
        {
            _requestHeader = requestHeader;
            _statisticLearningService = statisticLearningService;
        }

        [HttpGet("learning-process")]
        public async Task<IActionResult> GetStatisticsAsync(CancellationToken cancellationToken = default)
        {
            var uuid = _requestHeader.GetOwnerId();
            var result =  await _statisticLearningService.GetStatisticLearningProcessAsync(uuid, cancellationToken);
            return Ok(result);
        }
    }
}
