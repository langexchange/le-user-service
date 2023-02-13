using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Entities
{
    public partial class Imagemsg
    {
        public Imagemsg()
        {
            Imagemsgurls = new HashSet<Imagemsgurl>();
        }

        public int Messid { get; set; }

        public virtual Message Mess { get; set; }
        public virtual ICollection<Imagemsgurl> Imagemsgurls { get; set; }
    }
}
