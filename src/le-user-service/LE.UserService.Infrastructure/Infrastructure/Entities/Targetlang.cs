using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Targetlang
    {
        public Guid Userid { get; set; }
        public Guid Langid { get; set; }

        public virtual Language Lang { get; set; }
        public virtual User User { get; set; }
    }
}
