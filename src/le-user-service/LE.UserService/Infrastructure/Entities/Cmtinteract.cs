using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Cmtinteract
    {
        public int Userid { get; set; }
        public int Commentid { get; set; }
        public int InteractType { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual Interaction InteractTypeNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
