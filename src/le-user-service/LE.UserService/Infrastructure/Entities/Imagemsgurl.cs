using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Imagemsgurl
    {
        public int Urlid { get; set; }
        public int? Messid { get; set; }
        public string Url { get; set; }

        public virtual Imagemsg Mess { get; set; }
    }
}
