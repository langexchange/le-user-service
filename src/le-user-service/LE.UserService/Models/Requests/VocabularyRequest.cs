using System.Collections.Generic;

namespace LE.UserService.Models.Requests
{
    public class VocabularyRequest
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string TermLocale { get; set; }
        public string DefineLocale { get; set; }
        public string ImageUrl { get; set; }

        public List<VocabularyPair> vocabularyPairs { get; set; }
    }

    public class VocabularyPair
    {
        public string Term { get; set; }
        public string Define { get; set; }
        public string ImageUrl { get; set; }
    }
}
