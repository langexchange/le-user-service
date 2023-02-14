using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Postreport
    {
        public Postreport()
        {
            Userreportpsts = new HashSet<Userreportpst>();
        }

        public Guid Postreportid { get; set; }
        public string Reason { get; set; }

        public virtual ICollection<Userreportpst> Userreportpsts { get; set; }
    }
}
