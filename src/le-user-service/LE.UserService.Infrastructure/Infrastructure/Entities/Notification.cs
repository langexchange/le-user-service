using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Notification
    {
        public Guid Notiid { get; set; }
        public Guid Boxid { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public Guid Type { get; set; }

        public virtual Notibox Box { get; set; }
        public virtual Notitype TypeNavigation { get; set; }
    }
}
