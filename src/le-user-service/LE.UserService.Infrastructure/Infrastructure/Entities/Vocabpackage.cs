using System;
using System.Collections.Generic;
using System.Collections;

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
        public BitArray IsPublic { get; set; }
        public BitArray IsShared { get; set; }
        public BitArray IsRemoved { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Vocabulary> Vocabularies { get; set; }
    }
}
