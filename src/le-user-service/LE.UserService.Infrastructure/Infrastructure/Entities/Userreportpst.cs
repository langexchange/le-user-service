using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Userreportpst
    {
        public Guid Postreportid { get; set; }
        public Guid Postid { get; set; }
        public Guid Userid { get; set; }
        public string Statement { get; set; }

        public virtual Post Post { get; set; }
        public virtual Postreport Postreport { get; set; }
        public virtual User User { get; set; }
    }
}
