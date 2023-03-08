using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Comment
    {
        public Comment()
        {
            Audiocmts = new HashSet<Audiocmt>();
            Cmtinteracts = new HashSet<Cmtinteract>();
            Correctcmts = new HashSet<Correctcmt>();
            Imagecmts = new HashSet<Imagecmt>();
            Usrreportcmts = new HashSet<Usrreportcmt>();
        }

        public Guid Commentid { get; set; }
        public Guid Userid { get; set; }
        public Guid Postid { get; set; }
        public string Text { get; set; }
        public bool? IsImage { get; set; }
        public bool? IsCorrect { get; set; }
        public bool? IsAudio { get; set; }
        public bool? IsRemoved { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual Cmtpunish Cmtpunish { get; set; }
        public virtual ICollection<Audiocmt> Audiocmts { get; set; }
        public virtual ICollection<Cmtinteract> Cmtinteracts { get; set; }
        public virtual ICollection<Correctcmt> Correctcmts { get; set; }
        public virtual ICollection<Imagecmt> Imagecmts { get; set; }
        public virtual ICollection<Usrreportcmt> Usrreportcmts { get; set; }
    }
}
