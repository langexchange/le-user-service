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
            Audioposts = new HashSet<Audiopost>();
            Comments = new HashSet<Comment>();
            Imageposts = new HashSet<Imagepost>();
            Posttopics = new HashSet<Posttopic>();
            SharepostSharedpstNavigations = new HashSet<Sharepost>();
            Userintposts = new HashSet<Userintpost>();
            Userreportpsts = new HashSet<Userreportpst>();
            Videoposts = new HashSet<Videopost>();
        }

        public Guid Postid { get; set; }
        public Guid? Userid { get; set; }
        public string Text { get; set; }
        public Guid? Langid { get; set; }
        public BitArray RestrictBits { get; set; }
        public string Label { get; set; }
        public bool? IsAudio { get; set; }
        public bool? IsImage { get; set; }
        public bool? IsGroup { get; set; }
        public bool? IsRoom { get; set; }
        public bool? IsShare { get; set; }
        public bool? IsPublic { get; set; }
        public bool? IsVideo { get; set; }
        public bool? IsRemoved { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Language Lang { get; set; }
        public virtual User User { get; set; }
        public virtual Grouppost Grouppost { get; set; }
        public virtual Postpunish Postpunish { get; set; }
        public virtual Roompost Roompost { get; set; }
        public virtual Sharepost SharepostPost { get; set; }
        public virtual ICollection<Audiopost> Audioposts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Imagepost> Imageposts { get; set; }
        public virtual ICollection<Posttopic> Posttopics { get; set; }
        public virtual ICollection<Sharepost> SharepostSharedpstNavigations { get; set; }
        public virtual ICollection<Userintpost> Userintposts { get; set; }
        public virtual ICollection<Userreportpst> Userreportpsts { get; set; }
        public virtual ICollection<Videopost> Videoposts { get; set; }
    }
}
