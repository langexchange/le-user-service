using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Sharepost
    {
        public Guid Postid { get; set; }
        public Guid Sharedpst { get; set; }

        public virtual Post Post { get; set; }
        public virtual Post SharedpstNavigation { get; set; }
    }
}
