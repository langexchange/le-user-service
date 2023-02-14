using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Groupmember
    {
        public int Userid { get; set; }
        public int Groupid { get; set; }
        public int Roleid { get; set; }
        public BitArray RestrictLevel { get; set; }
        public BitArray IsRemoved { get; set; }

        public virtual Group Group { get; set; }
        public virtual Role Role { get; set; }
        public virtual User User { get; set; }
    }
}
