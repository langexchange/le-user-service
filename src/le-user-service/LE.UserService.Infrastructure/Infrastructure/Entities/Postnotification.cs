using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Postnotification
    {
        public Guid Notiid { get; set; }
        public Guid Boxid { get; set; }
        public string NotifiKey { get; set; }
        public string NotifyData { get; set; }
        public Guid? Postid { get; set; }
        public string Type { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Notibox Box { get; set; }
    }
}
