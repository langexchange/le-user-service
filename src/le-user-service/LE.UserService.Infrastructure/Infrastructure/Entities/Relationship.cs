using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Relationship
    {
        public Guid User1 { get; set; }
        public Guid User2 { get; set; }
        public BitArray Type { get; set; }

        public virtual User User1Navigation { get; set; }
        public virtual User User2Navigation { get; set; }
    }
}
