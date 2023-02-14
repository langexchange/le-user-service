using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Posttopic
    {
        public Guid Topicid { get; set; }
        public Guid Postid { get; set; }
        public BitArray IsRemoved { get; set; }

        public virtual Post Post { get; set; }
        public virtual Topic Topic { get; set; }
    }
}
