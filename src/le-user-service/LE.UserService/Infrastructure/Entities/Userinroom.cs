using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Userinroom
    {
        public int Userid { get; set; }
        public int Roomid { get; set; }

        public virtual Audioroom Room { get; set; }
        public virtual User User { get; set; }
    }
}
