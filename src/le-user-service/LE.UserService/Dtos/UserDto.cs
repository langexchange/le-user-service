using System;
using System.Collections.Generic;

namespace LE.UserService.Dtos
{
    public class UserDto
    {
        public UserDto()
        {
            NativeLanguage = new LanguageDto();
            TargetLanguages = new List<LanguageDto>();
        }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string Country { get; set; }
        public string[] Hobbies { get; set; }
        public int NumOfPosts { get; set; }
        public int NumOfPartners { get; set; }
        public string Avatar { get; set; }

        public LanguageDto NativeLanguage { get; set; }
        public List<LanguageDto> TargetLanguages { get; set; }
    }
}
