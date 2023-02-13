using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Entities
{
    public partial class Vocabpackage
    {
        public Vocabpackage()
        {
            Vocabularies = new HashSet<Vocabulary>();
        }

        public int Packageid { get; set; }
        public int Userid { get; set; }
        public string Name { get; set; }
        public BitArray IsPublic { get; set; }
        public BitArray IsShared { get; set; }
        public BitArray IsRemoved { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Vocabulary> Vocabularies { get; set; }
    }
}
