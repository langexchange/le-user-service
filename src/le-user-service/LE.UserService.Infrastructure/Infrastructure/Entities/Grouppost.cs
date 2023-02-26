using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Grouppost
    {
        public Guid Postid { get; set; }
        public Guid Groupid { get; set; }
        public bool? IsQualified { get; set; }
        public bool? IsHidden { get; set; }

        public virtual Group Group { get; set; }
        public virtual Post Post { get; set; }
    }
}
