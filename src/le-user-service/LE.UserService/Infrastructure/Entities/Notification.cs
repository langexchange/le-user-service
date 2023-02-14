using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Entities
{
    public partial class Notification
    {
        public int Notiid { get; set; }
        public int Boxid { get; set; }
        public string Text { get; set; }
        public string Url { get; set; }
        public int Type { get; set; }

        public virtual Notibox Box { get; set; }
        public virtual Notitype TypeNavigation { get; set; }
    }
}
