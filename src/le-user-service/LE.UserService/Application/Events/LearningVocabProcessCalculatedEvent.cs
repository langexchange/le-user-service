using LE.Library.Kernel;
using LE.UserService.Dtos;
using System;
using System.Collections.Generic;

namespace LE.UserService.Application.Events
{
    public class LearningVocabProcessCalculatedEvent : BaseMessage
    {
        public Guid UserId { get; set; }
        public List<PracticeResultDto> Result { get; set; }

        public LearningVocabProcessCalculatedEvent() : base(MessageValue.LEARNING_PROCESS_CALCULATED_EVENT)
        {

        }
    }
}
