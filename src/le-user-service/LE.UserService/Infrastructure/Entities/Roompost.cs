using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Roompost
    {
        public int Postid { get; set; }
        public int Roomid { get; set; }

        public virtual Post Post { get; set; }
        public virtual Audioroom Room { get; set; }
    }
}
