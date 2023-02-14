using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Comment
    {
        public Comment()
        {
            Cmtinteracts = new HashSet<Cmtinteract>();
            Usrreportcmts = new HashSet<Usrreportcmt>();
        }

        public int Commentid { get; set; }
        public int Userid { get; set; }
        public int Postid { get; set; }
        public string Text { get; set; }
        public BitArray IsImage { get; set; }
        public BitArray IsCorrect { get; set; }
        public BitArray IsAudio { get; set; }
        public BitArray IsRemoved { get; set; }

        public virtual Post Post { get; set; }
        public virtual User User { get; set; }
        public virtual Audiocmt Audiocmt { get; set; }
        public virtual Cmtpunish Cmtpunish { get; set; }
        public virtual Correctcmt Correctcmt { get; set; }
        public virtual Imagecmt Imagecmt { get; set; }
        public virtual ICollection<Cmtinteract> Cmtinteracts { get; set; }
        public virtual ICollection<Usrreportcmt> Usrreportcmts { get; set; }
    }
}
