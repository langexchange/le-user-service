using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Grouppunish
    {
        public Guid Adminid { get; set; }
        public Guid Groupid { get; set; }
        public Guid Punishid { get; set; }

        public virtual Admin Admin { get; set; }
        public virtual Group Group { get; set; }
        public virtual Punishment Punish { get; set; }
    }
}
