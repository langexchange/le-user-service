using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Cmtreport
    {
        public Cmtreport()
        {
            Usrreportcmts = new HashSet<Usrreportcmt>();
        }

        public Guid Cmtreportid { get; set; }
        public string Reason { get; set; }

        public virtual ICollection<Usrreportcmt> Usrreportcmts { get; set; }
    }
}
