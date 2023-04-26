using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Sharingnotification
    {
        public Sharingnotification()
        {
            Notiboxsharings = new HashSet<Notiboxsharing>();
        }

        public Guid Notiid { get; set; }
        public string NotifiKey { get; set; }
        public string NotifyData { get; set; }
        public Guid? Postid { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual ICollection<Notiboxsharing> Notiboxsharings { get; set; }
    }
}
