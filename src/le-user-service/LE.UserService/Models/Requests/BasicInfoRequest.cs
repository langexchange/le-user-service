using System;
using System.Collections.Generic;

namespace LE.UserService.Models.Requests
{
    public class BasicInfoRequest
    {
        public LanguageRequest NativeLanguage { get; set; }
        public List<LanguageRequest> TargetLanguages { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class UserInfo
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public string Country { get; set; }
        public string[] Hobbies { get; set; }
    }
    public class LanguageRequest
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
    }
}
