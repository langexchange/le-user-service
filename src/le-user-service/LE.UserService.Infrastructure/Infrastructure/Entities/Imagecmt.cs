using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Imagecmt
    {
        public int Commentid { get; set; }
        public string Url { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
