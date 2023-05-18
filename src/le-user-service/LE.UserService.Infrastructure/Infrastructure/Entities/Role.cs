using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Role
    {
        public Role()
        {
            Groupmembers = new HashSet<Groupmember>();
        }

        public Guid Roleid { get; set; }
        public string Strrole { get; set; }

        public virtual ICollection<Groupmember> Groupmembers { get; set; }
    }
}
