using System;
using System.Collections.Generic;

namespace LE.UserService.Dtos
{
    public class VocabularyPackageDto
    {
        public Guid PackageId { get; set; } = Guid.NewGuid();
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string TermLocale { get; set; }
        public string DefineLocale { get; set; }

        public List<VocabularyDto> vocabularies { get; set; }
    }

    public class VocabularyDto
    {
        public string Term { get; set; }
        public string Define { get; set; }
        public string ImageUrl { get; set; }
    }
}
