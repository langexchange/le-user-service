using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Entities
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

        public int Groupid { get; set; }
        public string Name { get; set; }
        public string Introduction { get; set; }
        public string Requirement { get; set; }
        public BitArray IsPublic { get; set; }
        public BitArray PostCheck { get; set; }
        public BitArray IsRemoved { get; set; }

        public virtual ICollection<Groupmember> Groupmembers { get; set; }
        public virtual ICollection<Grouppost> Groupposts { get; set; }
        public virtual ICollection<Grouppunish> Grouppunishes { get; set; }
        public virtual ICollection<Joingrpreq> Joingrpreqs { get; set; }
    }
}
