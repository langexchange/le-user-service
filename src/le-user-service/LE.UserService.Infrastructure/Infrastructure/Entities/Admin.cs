using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Admin
    {
        public Admin()
        {
            Cmtpunishes = new HashSet<Cmtpunish>();
            Grouppunishes = new HashSet<Grouppunish>();
            Postpunishes = new HashSet<Postpunish>();
            Userpunishes = new HashSet<Userpunish>();
        }

        public Guid Adminid { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string RemainName { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public virtual ICollection<Cmtpunish> Cmtpunishes { get; set; }
        public virtual ICollection<Grouppunish> Grouppunishes { get; set; }
        public virtual ICollection<Postpunish> Postpunishes { get; set; }
        public virtual ICollection<Userpunish> Userpunishes { get; set; }
    }
}
