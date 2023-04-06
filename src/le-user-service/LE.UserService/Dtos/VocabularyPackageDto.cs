﻿using System;
using System.Collections.Generic;

namespace LE.UserService.Dtos
{
    public class VocabularyPackageDto
    {
        public VocabularyPackageDto()
        {
            VocabularyDtos = new List<VocabularyDto>();
            PracticeResultDto = new PracticeResult();
        }
        public Guid PackageId { get; set; }
        public Guid UserId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string TermLocale { get; set; }
        public string DefineLocale { get; set; }
        public string ImageUrl { get; set; }

        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public List<VocabularyDto> VocabularyDtos { get; set; }
        public PracticeResult PracticeResultDto { get; set; }

    }

    public class PracticeResult
    {
        public bool IsPracticed { get; set; } = false;
        public int TotalVocabs { get; set; }
        public int CurrentNumOfVocab { get; set; }
    }

    public class VocabularyDto
    {
        public string Term { get; set; }
        public string Define { get; set; }
        public string ImageUrl { get; set; }
    }

    public class PracticeVocabularyDto
    {
        public Guid VocabId { get; set; }
        public string Term { get; set; }
        public string Define { get; set; }
        public string ImageUrl { get; set; }
    }

    public class PracticeVocabulariesDto
    {
        public Guid PackageId { get; set; }
        public IEnumerable<PracticeVocabularyDto> PracticeVocabularies { get; set; }
    }

    public class UserVocabPackageDto
    {
        public UserInfo UserInfo { get; set; }
        public List<VocabularyPackageDto> vocabularyPackageDtos { get; set; }
    }
}
