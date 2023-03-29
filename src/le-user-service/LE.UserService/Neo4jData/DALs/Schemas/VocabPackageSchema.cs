using System;

namespace LE.UserService.Neo4jData.DALs.Schemas
{
    public class VocabPackageSchema
    {
        public const string VOCAB_PACKAGE_LABEL = "vocabpackage";

        public Guid Packageid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public bool IsPublic { get; set; }
        public string TermLocale { get; set; }
        public string DefineLocale { get; set; }
        public string Vocabularies { get; set; }
    }
}
