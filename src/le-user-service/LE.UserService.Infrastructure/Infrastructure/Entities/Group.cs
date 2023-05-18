using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Group
    {
        public Group()
        {
            Groupmembers = new HashSet<Groupmember>();
            Groupposts = new HashSet<Grouppost>();
            Grouppunishes = new HashSet<Grouppunish>();
            Joingrpreqs = new HashSet<Joingrpreq>();
        }

        public Guid Groupid { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Requirement { get; set; }
        public bool? IsPublic { get; set; }
        public bool? PostCheck { get; set; }
        public bool? IsRemoved { get; set; }

        public virtual ICollection<Groupmember> Groupmembers { get; set; }
        public virtual ICollection<Grouppost> Groupposts { get; set; }
        public virtual ICollection<Grouppunish> Grouppunishes { get; set; }
        public virtual ICollection<Joingrpreq> Joingrpreqs { get; set; }
    }
}
