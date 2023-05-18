using System;
using System.Collections.Generic;

namespace LE.UserService.Dtos
{
    public class SuggestUserDto
    {
        public SuggestUserDto()
        {
            NativeLanguage = new LevelNeo4jLangDto();
            TargetLanguages = new List<LevelNeo4jLangDto>();
        }
        public Guid Id { get;set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string Country { get; set; }
        public string[] Hobbies { get; set; }
        public string Avatar { get; set; }
        public bool IsFriend { get; set; } = false;

        public LevelNeo4jLangDto NativeLanguage { get; set; }
        public List<LevelNeo4jLangDto> TargetLanguages { get; set; }
    }
}
