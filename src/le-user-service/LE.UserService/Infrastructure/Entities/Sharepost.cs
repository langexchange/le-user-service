using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Sharepost
    {
        public int Postid { get; set; }
        public int Sharedpst { get; set; }

        public virtual Post Post { get; set; }
        public virtual Post SharedpstNavigation { get; set; }
    }
}
