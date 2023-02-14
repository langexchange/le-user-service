using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Audiopost
    {
        public int Postid { get; set; }
        public string Url { get; set; }

        public virtual Post Post { get; set; }
    }
}
