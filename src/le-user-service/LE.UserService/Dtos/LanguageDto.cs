using System;

namespace LE.UserService.Dtos
{
    public class LanguageDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public string LocaleCode { get; set; }
        public int Level { get; set; }
    }
    public class Neo4jLangDto
    {
        public string Name { get; set; }
        public string LocaleCode { get; set; }
    }
}
