using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Notibox
    {
        public Notibox()
        {
            Commentnotifications = new HashSet<Commentnotification>();
            Friendnotifications = new HashSet<Friendnotification>();
            Notiboxsharings = new HashSet<Notiboxsharing>();
            Postnotifications = new HashSet<Postnotification>();
            Users = new HashSet<User>();
            Vocabpackagenotifications = new HashSet<Vocabpackagenotification>();
        }

        public Guid Boxid { get; set; }
        public Guid? Userid { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<Commentnotification> Commentnotifications { get; set; }
        public virtual ICollection<Friendnotification> Friendnotifications { get; set; }
        public virtual ICollection<Notiboxsharing> Notiboxsharings { get; set; }
        public virtual ICollection<Postnotification> Postnotifications { get; set; }
        public virtual ICollection<User> Users { get; set; }
        public virtual ICollection<Vocabpackagenotification> Vocabpackagenotifications { get; set; }
    }
}
