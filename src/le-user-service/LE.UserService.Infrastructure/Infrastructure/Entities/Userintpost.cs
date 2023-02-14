using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Userintpost
    {
        public Guid Postid { get; set; }
        public Guid Userid { get; set; }
        public Guid InteractType { get; set; }

        public virtual Interaction InteractTypeNavigation { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
