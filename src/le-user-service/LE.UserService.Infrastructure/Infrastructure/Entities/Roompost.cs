using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Roompost
    {
        public Guid Postid { get; set; }
        public Guid Roomid { get; set; }

        public virtual Post Post { get; set; }
        public virtual Audioroom Room { get; set; }
    }
}
