using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Entities
{
    public partial class Grouppunish
    {
        public int Adminid { get; set; }
        public int Groupid { get; set; }
        public int Punishid { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Group Group { get; set; }
        public virtual Punishment Punish { get; set; }
    }
}
