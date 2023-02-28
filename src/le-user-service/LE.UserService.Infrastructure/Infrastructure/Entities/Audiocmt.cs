using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Audiocmt
    {
        public Guid Audiocmtid { get; set; }
        public Guid Commentid { get; set; }
        public string Url { get; set; }

        public virtual Comment Comment { get; set; }
    }
}
