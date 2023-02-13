using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Entities
{
    public partial class Language
    {
        public Language()
        {
            Posts = new HashSet<Post>();
            Targetlangs = new HashSet<Targetlang>();
            Users = new HashSet<User>();
        }

        public int Langid { get; set; }
        public string Name { get; set; }

        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Targetlang> Targetlangs { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}
