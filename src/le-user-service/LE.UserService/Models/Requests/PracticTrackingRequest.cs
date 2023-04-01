using System;
using System.Collections.Generic;
using System.Linq;

namespace LE.UserService.Models.Requests
{
    public class PracticeTrackingRequest
    {
        public List<PracticeVocabTracking> VocabTrackings { get; set; }
        public bool IsValid()
        {
            if (VocabTrackings == null || VocabTrackings.Any(x => Env.DifficultyLevels.Contains(x.DifficultyLevel)))
                return false;
            return true;
        }
    }

    public class PracticeVocabTracking
    {
        public Guid VocabularyId { get; set; }
        public string DifficultyLevel { get; set; }
    }
}
