using System;
using System.Collections.Generic;

namespace LE.UserService.Dtos
{
    public class VocabularyPackageDto
    {
        public VocabularyPackageDto()
        {
            VocabularyDtos = new List<VocabularyDto>();
        }
        public Guid PackageId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string TermLocale { get; set; }
        public string DefineLocale { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<VocabularyDto> VocabularyDtos { get; set; }
    }

    public class VocabularyDto
    {
        public string Term { get; set; }
        public string Define { get; set; }
        public string ImageUrl { get; set; }
    }

    public class UserVocabPackageDto
    {
        public UserInfo UserInfo { get; set; }
        public IEnumerable<VocabularyPackageDto> vocabularyPackageDtos { get; set; }
    }
}
