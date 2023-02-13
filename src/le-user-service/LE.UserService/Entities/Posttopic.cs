using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Entities
{
    public partial class Posttopic
    {
        public int Topicid { get; set; }
        public int Postid { get; set; }
        public BitArray IsRemoved { get; set; }

        public virtual Post Post { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
