using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Entities
{
    public partial class Cmtreport
    {
        public Cmtreport()
        {
            Usrreportcmts = new HashSet<Usrreportcmt>();
        }

        public int Cmtreportid { get; set; }
        public string Reason { get; set; }

        public virtual ICollection<Usrreportcmt> Usrreportcmts { get; set; }
    }
}
