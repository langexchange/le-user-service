using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Userintpost
    {
        public int Postid { get; set; }
        public int Userid { get; set; }
        public int InteractType { get; set; }

        public virtual Interaction InteractTypeNavigation { get; set; }
        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
    }
}
