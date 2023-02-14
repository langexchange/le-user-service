using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Cmtinteract
    {
        public Guid Userid { get; set; }
        public Guid Commentid { get; set; }
        public Guid InteractType { get; set; }

        public virtual Comment Comment { get; set; }
        public virtual Interaction InteractTypeNavigation { get; set; }
        public virtual User User { get; set; }
    }
}
