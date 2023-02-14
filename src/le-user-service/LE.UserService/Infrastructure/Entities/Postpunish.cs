using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Postpunish
    {
        public int Postid { get; set; }
        public int Adminid { get; set; }
        public int Punishid { get; set; }
        public int? Userid { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Post Post { get; set; }
        public virtual Punishment Punish { get; set; }
    }
}
