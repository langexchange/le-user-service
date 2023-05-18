using LE.Library.Kernel;
using LE.UserService.Services;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace LE.UserService.Application.Events
{
    public class StatisticalSignal : BaseMessage
    {
        public StatisticalSignal() : base(MessageValue.STATISTICAL_SIGNAL)
        {

        }
    }

    public class StatisticalSignalHandler : IAsyncHandler<StatisticalSignal>
    {
        private readonly IVocabService _vocabService;
        public StatisticalSignalHandler(IVocabService vocabService)
        {
            _vocabService = vocabService;
        }

        public async Task HandleAsync(IHandlerContext<StatisticalSignal> Context, CancellationToken cancellationToken = default)
        {
            await _vocabService.StatisticVocabLearningProcessAsync(cancellationToken);
        }
    }
}
