using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Notiboxsharing
    {
        public Guid Boxid { get; set; }
        public Guid Notiid { get; set; }

        public virtual Notibox Box { get; set; }
        public virtual Sharingnotification Noti { get; set; }
    }
}
