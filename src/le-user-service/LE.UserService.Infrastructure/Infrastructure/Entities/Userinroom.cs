using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Userinroom
    {
        public Guid Userid { get; set; }
        public Guid Roomid { get; set; }

        public virtual Audioroom Room { get; set; }
        public virtual User User { get; set; }
    }
}
