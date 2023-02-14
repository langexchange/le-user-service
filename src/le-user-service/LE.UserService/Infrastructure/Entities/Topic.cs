using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Topic
    {
        public Topic()
        {
            Posttopics = new HashSet<Posttopic>();
        }

        public int Topicid { get; set; }
        public int Userid { get; set; }
        public string Name { get; set; }
        public BitArray IsPublic { get; set; }
        public BitArray IsRemoved { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Posttopic> Posttopics { get; set; }
    }
}
