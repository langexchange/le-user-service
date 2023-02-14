using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Userpunish
    {
        public Guid Adminid { get; set; }
        public Guid Userid { get; set; }
        public Guid Punishid { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Punishment Punish { get; set; }
        public virtual User User { get; set; }
    }
}
