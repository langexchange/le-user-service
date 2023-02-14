using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Audiocmt
    {
        public int Commentid { get; set; }
        public string Url { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
