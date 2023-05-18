using System;
using System.Collections.Generic;

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

        public Guid Roomid { get; set; }
        public Guid Owner { get; set; }
        public bool? IsClosed { get; set; }

        public virtual User OwnerNavigation { get; set; }
        public virtual ICollection<Roompost> Roomposts { get; set; }
        public virtual ICollection<Userinroom> Userinrooms { get; set; }
    }
}
