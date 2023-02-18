using System;
using System.Collections.Generic;

namespace LE.UserService.Models.Requests
{
    public class BasicInfoRequest
    {
        public Language NativeLanguage { get; set; }
        public List<Language> TargetLanguages { get; set; }
        public UserInfo UserInfo { get; set; }
    }

    public class UserInfo
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
    }
    public class Language
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
    }
}
