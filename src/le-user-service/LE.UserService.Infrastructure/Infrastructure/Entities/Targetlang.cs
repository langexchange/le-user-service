using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Targetlang
    {
        public int Userid { get; set; }
        public int Langid { get; set; }

        public virtual Language Lang { get; set; }
        public virtual User User { get; set; }
    }
}
