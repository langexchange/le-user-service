using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Usrreportcmt
    {
        public int Cmtreportid { get; set; }
        public int Commentid { get; set; }
        public int Userid { get; set; }
        public string Statement { get; set; }

        public virtual Cmtreport Cmtreport { get; set; }
        public virtual Comment Comment { get; set; }
        public virtual User User { get; set; }
    }
}
