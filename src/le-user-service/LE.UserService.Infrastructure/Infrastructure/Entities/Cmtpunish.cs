using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Cmtpunish
    {
        public Guid Commentid { get; set; }
        public Guid Adminid { get; set; }
        public Guid Punishid { get; set; }
        public int? Userid { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual Punishment Punish { get; set; }
    }
}
