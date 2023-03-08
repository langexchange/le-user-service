using System;
using System.Collections.Generic;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure.Entities
{
    public partial class User
    {
        public User()
        {
            Audiorooms = new HashSet<Audioroom>();
            ChatconfReceiverNavigations = new HashSet<Chatconf>();
            ChatconfSenderNavigations = new HashSet<Chatconf>();
            Cmtinteracts = new HashSet<Cmtinteract>();
            Comments = new HashSet<Comment>();
            Groupmembers = new HashSet<Groupmember>();
            Groupopreqs = new HashSet<Groupopreq>();
            Joingrpreqs = new HashSet<Joingrpreq>();
            MessageReceiverNavigations = new HashSet<Message>();
            MessageSenderNavigations = new HashSet<Message>();
            Posts = new HashSet<Post>();
            RelationshipUser1Navigations = new HashSet<Relationship>();
            RelationshipUser2Navigations = new HashSet<Relationship>();
            Targetlangs = new HashSet<Targetlang>();
            Topics = new HashSet<Topic>();
            Tutorreqs = new HashSet<Tutorreq>();
            Userhobbies = new HashSet<Userhobby>();
            Userintposts = new HashSet<Userintpost>();
            Userpunishes = new HashSet<Userpunish>();
            Userreportpsts = new HashSet<Userreportpst>();
            Usrreportcmts = new HashSet<Usrreportcmt>();
            Vocabpackages = new HashSet<Vocabpackage>();
        }

        public Guid Userid { get; set; }
        public decimal? Longtt { get; set; }
        public decimal? Latt { get; set; }
        public Guid? NativeLang { get; set; }
        public Guid? Notibox { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Gender { get; set; }
        public string Introduction { get; set; }
        public int? NativeLevel { get; set; }
        public int IncreateId { get; set; }
        public bool? IsTutor { get; set; }
        public bool? IsRestrict { get; set; }
        public bool? IsRemoved { get; set; }
        public string TempToken { get; set; }
        public DateTime? TokenIat { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public virtual Language NativeLangNavigation { get; set; }
        public virtual Notibox NotiboxNavigation { get; set; }
        public virtual Userinroom Userinroom { get; set; }
        public virtual ICollection<Audioroom> Audiorooms { get; set; }
        public virtual ICollection<Chatconf> ChatconfReceiverNavigations { get; set; }
        public virtual ICollection<Chatconf> ChatconfSenderNavigations { get; set; }
        public virtual ICollection<Cmtinteract> Cmtinteracts { get; set; }
        public virtual ICollection<Comment> Comments { get; set; }
        public virtual ICollection<Groupmember> Groupmembers { get; set; }
        public virtual ICollection<Groupopreq> Groupopreqs { get; set; }
        public virtual ICollection<Joingrpreq> Joingrpreqs { get; set; }
        public virtual ICollection<Message> MessageReceiverNavigations { get; set; }
        public virtual ICollection<Message> MessageSenderNavigations { get; set; }
        public virtual ICollection<Post> Posts { get; set; }
        public virtual ICollection<Relationship> RelationshipUser1Navigations { get; set; }
        public virtual ICollection<Relationship> RelationshipUser2Navigations { get; set; }
        public virtual ICollection<Targetlang> Targetlangs { get; set; }
        public virtual ICollection<Topic> Topics { get; set; }
        public virtual ICollection<Tutorreq> Tutorreqs { get; set; }
        public virtual ICollection<Userhobby> Userhobbies { get; set; }
        public virtual ICollection<Userintpost> Userintposts { get; set; }
        public virtual ICollection<Userpunish> Userpunishes { get; set; }
        public virtual ICollection<Userreportpst> Userreportpsts { get; set; }
        public virtual ICollection<Usrreportcmt> Usrreportcmts { get; set; }
        public virtual ICollection<Vocabpackage> Vocabpackages { get; set; }
    }
}
