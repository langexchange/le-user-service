using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Audioroom
    {
        public Audioroom()
        {
            Roomposts = new HashSet<Roompost>();
            Userinrooms = new HashSet<Userinroom>();
        }

        public int Roomid { get; set; }
        public int Owner { get; set; }
        public BitArray IsClosed { get; set; }

        public virtual User OwnerNavigation { get; set; }
        public virtual ICollection<Roompost> Roomposts { get; set; }
        public virtual ICollection<Userinroom> Userinrooms { get; set; }
    }
}
