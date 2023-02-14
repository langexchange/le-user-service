using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Userhobby
    {
        public Guid Userid { get; set; }
        public Guid Hobbyid { get; set; }

        public virtual Hobby Hobby { get; set; }
        public virtual User User { get; set; }
    }
}
