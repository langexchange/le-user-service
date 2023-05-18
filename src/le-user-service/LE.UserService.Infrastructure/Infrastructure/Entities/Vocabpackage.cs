using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Vocabpackage
    {
        public Vocabpackage()
        {
            Vocabularies = new HashSet<Vocabulary>();
        }

        public Guid Packageid { get; set; }
        public Guid Userid { get; set; }
        public string Name { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsShared { get; set; }
        public bool? IsRemoved { get; set; }
        public string Description { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string VocabularyPairs { get; set; }
        public string Term { get; set; }
        public string Define { get; set; }
        public bool? IsPracticed { get; set; }
        public string Imageurl { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Vocabulary> Vocabularies { get; set; }
    }
}
