using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Audiomsg
    {
        public Audiomsg()
        {
            Audiomsgurls = new HashSet<Audiomsgurl>();
        }

        public int Messid { get; set; }
        public string Url { get; set; }

        public virtual Message Mess { get; set; }
        public virtual ICollection<Audiomsgurl> Audiomsgurls { get; set; }
    }
}
