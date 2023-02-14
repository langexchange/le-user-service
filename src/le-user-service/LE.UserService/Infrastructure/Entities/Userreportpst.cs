using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Userreportpst
    {
        public int Postreportid { get; set; }
        public int Postid { get; set; }
        public int Userid { get; set; }
        public string Statement { get; set; }

        public virtual Post Post { get; set; }
        public virtual Postreport Postreport { get; set; }
        public virtual User User { get; set; }
    }
}
