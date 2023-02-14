using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Punishment
    {
        public Punishment()
        {
            Cmtpunishes = new HashSet<Cmtpunish>();
            Grouppunishes = new HashSet<Grouppunish>();
            Postpunishes = new HashSet<Postpunish>();
            Restrictpunishes = new HashSet<Restrictpunish>();
            Userpunishes = new HashSet<Userpunish>();
        }

        public int Punishid { get; set; }
        public BitArray IsRestrict { get; set; }
        public int? Relapse { get; set; }
        public string Type { get; set; }

        public virtual ICollection<Cmtpunish> Cmtpunishes { get; set; }
        public virtual ICollection<Grouppunish> Grouppunishes { get; set; }
        public virtual ICollection<Postpunish> Postpunishes { get; set; }
        public virtual ICollection<Restrictpunish> Restrictpunishes { get; set; }
        public virtual ICollection<Userpunish> Userpunishes { get; set; }
    }
}
