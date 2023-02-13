using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Entities
{
    public partial class Userpunish
    {
        public int Adminid { get; set; }
        public int Userid { get; set; }
        public int Punishid { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Punishment Punish { get; set; }
        public virtual User User { get; set; }
    }
}
