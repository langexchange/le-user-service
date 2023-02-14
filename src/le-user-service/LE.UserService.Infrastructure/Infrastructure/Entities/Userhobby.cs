using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Userhobby
    {
        public int Userid { get; set; }
        public int Hobbyid { get; set; }

        public virtual Hobby Hobby { get; set; }
        public virtual User User { get; set; }
    }
}
