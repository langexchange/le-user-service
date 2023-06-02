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
            if (VocabTrackings == null || VocabTrackings.Any(x => (x.Quality < 0 || x.Quality > 2) ))
                return false;
            return true;
        }
    }

    public class PracticeVocabTracking
    {
        public Guid VocabularyId { get; set; }
        public int Quality { get; set; }
    }
}
