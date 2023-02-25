using System;
using System.Collections.Generic;
using System.Collections;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class Post
    {
        public Post()
        {
            Comments = new HashSet<Comment>();
            Posttopics = new HashSet<Posttopic>();
            SharepostSharedpstNavigations = new HashSet<Sharepost>();
            Userintposts = new HashSet<Userintpost>();
            Userreportpsts = new HashSet<Userreportpst>();
        }

        public Guid Postid { get; set; }
        public Guid? Userid { get; set; }
        public string Text { get; set; }
        public Guid? Langid { get; set; }
        public BitArray RestrictBits { get; set; }
        public BitArray IsAudio { get; set; }
        public BitArray IsImage { get; set; }
        public BitArray IsGroup { get; set; }
        public BitArray IsRoom { get; set; }
        public BitArray IsShare { get; set; }
        public BitArray IsPublic { get; set; }
        public BitArray IsRemoved { get; set; }
        public BitArray IsVideo { get; set; }
        public string Label { get; set; }

        public virtual Language Lang { get; set; }
        public virtual User User { get; set; }
        public virtual Audiopost Audiopost { get; set; }
        public virtual Grouppost Grouppost { get; set; }
        public virtual Imagepost Imagepost { get; set; }
        public virtual Postpunish Postpunish { get; set; }
        public virtual Roompost Roompost { get; set; }
        public virtual Sharepost SharepostPost { get; set; }
        public virtual Videopost Videopost { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Posttopic> Posttopics { get; set; }
        public virtual ICollection<Sharepost> SharepostSharedpstNavigations { get; set; }
        public virtual ICollection<Userintpost> Userintposts { get; set; }
        public virtual ICollection<Userreportpst> Userreportpsts { get; set; }
    }
}
