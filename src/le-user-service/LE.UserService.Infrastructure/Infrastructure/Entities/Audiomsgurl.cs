using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Audiomsgurl
    {
        public int Urlid { get; set; }
        public int? Messid { get; set; }
        public string Url { get; set; }

        public virtual Audiomsg Mess { get; set; }
    }
}
