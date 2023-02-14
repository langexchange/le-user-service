using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Imagemsg
    {
        public Imagemsg()
        {
            Imagemsgurls = new HashSet<Imagemsgurl>();
        }

        public Guid Messid { get; set; }

        public virtual Message Mess { get; set; }
        public virtual ICollection<Imagemsgurl> Imagemsgurls { get; set; }
    }
}
