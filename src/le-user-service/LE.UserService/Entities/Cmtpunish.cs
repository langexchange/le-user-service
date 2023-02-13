using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Entities
{
    public partial class Cmtpunish
    {
        public int Commentid { get; set; }
        public int Adminid { get; set; }
        public int Punishid { get; set; }
        public int? Userid { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual Punishment Punish { get; set; }
    }
}
