using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Restrictpunish
    {
        public Guid Punishid { get; set; }
        public Guid Restrictid { get; set; }

        public virtual Punishment Punish { get; set; }
        public virtual Restrict Restrict { get; set; }
    }
}
