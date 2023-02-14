using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Notitype
    {
        public Notitype()
        {
            Notifications = new HashSet<Notification>();
        }

        public int Typeid { get; set; }
        public string Typestring { get; set; }
        public string Sample { get; set; }

        public virtual ICollection<Notification> Notifications { get; set; }
    }
}
