using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using LE.UserService.Infrastructure.Infrastructure.Entities;

#nullable disable

namespace LE.UserService.Infrastructure.Infrastructure
{
    public partial class LanggeneralDbContext : DbContext
    {
        public LanggeneralDbContext()
        {
        }

        public LanggeneralDbContext(DbContextOptions<LanggeneralDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Admin> Admins { get; set; }
        public virtual DbSet<Arrangement> Arrangements { get; set; }
        public virtual DbSet<Audiocmt> Audiocmts { get; set; }
        public virtual DbSet<Audiomsg> Audiomsgs { get; set; }
        public virtual DbSet<Audiomsgurl> Audiomsgurls { get; set; }
        public virtual DbSet<Audiopost> Audioposts { get; set; }
        public virtual DbSet<Audioroom> Audiorooms { get; set; }
        public virtual DbSet<Chatconf> Chatconfs { get; set; }
        public virtual DbSet<Cmtinteract> Cmtinteracts { get; set; }
        public virtual DbSet<Cmtpunish> Cmtpunishes { get; set; }
        public virtual DbSet<Cmtreport> Cmtreports { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Correctcmt> Correctcmts { get; set; }
        public virtual DbSet<Correctmsg> Correctmsgs { get; set; }
        public virtual DbSet<FlywaySchemaHistory> FlywaySchemaHistories { get; set; }
        public virtual DbSet<Group> Groups { get; set; }
        public virtual DbSet<Groupmember> Groupmembers { get; set; }
        public virtual DbSet<Groupopreq> Groupopreqs { get; set; }
        public virtual DbSet<Grouppost> Groupposts { get; set; }
        public virtual DbSet<Grouppunish> Grouppunishes { get; set; }
        public virtual DbSet<Hobby> Hobbies { get; set; }
        public virtual DbSet<Imagecmt> Imagecmts { get; set; }
        public virtual DbSet<Imagemsg> Imagemsgs { get; set; }
        public virtual DbSet<Imagemsgurl> Imagemsgurls { get; set; }
        public virtual DbSet<Imagepost> Imageposts { get; set; }
        public virtual DbSet<Interaction> Interactions { get; set; }
        public virtual DbSet<Joingrpreq> Joingrpreqs { get; set; }
        public virtual DbSet<Language> Languages { get; set; }
        public virtual DbSet<Message> Messages { get; set; }
        public virtual DbSet<Notibox> Notiboxes { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<Notitype> Notitypes { get; set; }
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Postpunish> Postpunishes { get; set; }
        public virtual DbSet<Postreport> Postreports { get; set; }
        public virtual DbSet<Posttopic> Posttopics { get; set; }
        public virtual DbSet<Punishment> Punishments { get; set; }
        public virtual DbSet<Relationship> Relationships { get; set; }
        public virtual DbSet<Restrict> Restricts { get; set; }
        public virtual DbSet<Restrictpunish> Restrictpunishes { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<Roompost> Roomposts { get; set; }
        public virtual DbSet<Sharepost> Shareposts { get; set; }
        public virtual DbSet<Targetlang> Targetlangs { get; set; }
        public virtual DbSet<Topic> Topics { get; set; }
        public virtual DbSet<Tutorreq> Tutorreqs { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Userhobby> Userhobbies { get; set; }
        public virtual DbSet<Userinroom> Userinrooms { get; set; }
        public virtual DbSet<Userintpost> Userintposts { get; set; }
        public virtual DbSet<Userpunish> Userpunishes { get; set; }
        public virtual DbSet<Userreportpst> Userreportpsts { get; set; }
        public virtual DbSet<Usrreportcmt> Usrreportcmts { get; set; }
        public virtual DbSet<Videopost> Videoposts { get; set; }
        public virtual DbSet<Vocabgoal> Vocabgoals { get; set; }
        public virtual DbSet<Vocabpackage> Vocabpackages { get; set; }
        public virtual DbSet<Vocabulary> Vocabularies { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasPostgresExtension("uuid-ossp")
                .HasAnnotation("Relational:Collation", "en_US.UTF-8");

            modelBuilder.Entity<Admin>(entity =>
            {
                entity.ToTable("admin");

                entity.Property(e => e.Adminid)
                    .HasColumnName("adminid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("timezone('utc'::text, now())");

                entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("first_name");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("password");

                entity.Property(e => e.RemainName)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("remain_name");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");
            });

            modelBuilder.Entity<Arrangement>(entity =>
            {
                entity.HasKey(e => e.Arrangeid)
                    .HasName("arrangement_pkey");

                entity.ToTable("arrangement");

                entity.Property(e => e.Arrangeid)
                    .HasColumnName("arrangeid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Confid).HasColumnName("confid");

                entity.Property(e => e.IsAccepted)
                    .HasColumnName("is_accepted")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.TimeEnded)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("time_ended");

                entity.Property(e => e.TimeStarted)
                    .HasColumnType("timestamp with time zone")
                    .HasColumnName("time_started");

                entity.HasOne(d => d.Conf)
                    .WithMany(p => p.Arrangements)
                    .HasForeignKey(d => d.Confid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("arrangement_confid_fkey");
            });

            modelBuilder.Entity<Audiocmt>(entity =>
            {
                entity.ToTable("audiocmt");

                entity.Property(e => e.Audiocmtid)
                    .HasColumnName("audiocmtid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Commentid).HasColumnName("commentid");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("url");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.Audiocmts)
                    .HasForeignKey(d => d.Commentid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("audiocmt_commentid_fkey");
            });

            modelBuilder.Entity<Audiomsg>(entity =>
            {
                entity.HasKey(e => e.Messid)
                    .HasName("audiomsg_pkey");

                entity.ToTable("audiomsg");

                entity.Property(e => e.Messid)
                    .HasColumnName("messid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("url");

                entity.HasOne(d => d.Mess)
                    .WithOne(p => p.Audiomsg)
                    .HasForeignKey<Audiomsg>(d => d.Messid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("audiomsg_messid_fkey");
            });

            modelBuilder.Entity<Audiomsgurl>(entity =>
            {
                entity.HasKey(e => e.Urlid)
                    .HasName("audiomsgurl_pkey");

                entity.ToTable("audiomsgurl");

                entity.Property(e => e.Urlid)
                    .HasColumnName("urlid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Messid).HasColumnName("messid");

                entity.Property(e => e.Url)
                    .HasMaxLength(256)
                    .HasColumnName("url");

                entity.HasOne(d => d.Mess)
                    .WithMany(p => p.Audiomsgurls)
                    .HasForeignKey(d => d.Messid)
                    .HasConstraintName("audiomsgurl_messid_fkey");
            });

            modelBuilder.Entity<Audiopost>(entity =>
            {
                entity.ToTable("audiopost");

                entity.Property(e => e.Audiopostid)
                    .HasColumnName("audiopostid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Postid).HasColumnName("postid");

                entity.Property(e => e.Url)
                    .HasMaxLength(256)
                    .HasColumnName("url");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Audioposts)
                    .HasForeignKey(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("audiopost_postid_fkey");
            });

            modelBuilder.Entity<Audioroom>(entity =>
            {
                entity.HasKey(e => e.Roomid)
                    .HasName("audioroom_pkey");

                entity.ToTable("audioroom");

                entity.Property(e => e.Roomid)
                    .HasColumnName("roomid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.IsClosed)
                    .HasColumnName("is_closed")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Owner).HasColumnName("owner");

                entity.HasOne(d => d.OwnerNavigation)
                    .WithMany(p => p.Audiorooms)
                    .HasForeignKey(d => d.Owner)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("audioroom_owner_fkey");
            });

            modelBuilder.Entity<Chatconf>(entity =>
            {
                entity.HasKey(e => e.Confid)
                    .HasName("chatconf_pkey");

                entity.ToTable("chatconf");

                entity.Property(e => e.Confid)
                    .HasColumnName("confid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Isblock)
                    .HasColumnName("isblock")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Ismute)
                    .HasColumnName("ismute")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Receiver).HasColumnName("receiver");

                entity.Property(e => e.Sender).HasColumnName("sender");

                entity.HasOne(d => d.ReceiverNavigation)
                    .WithMany(p => p.ChatconfReceiverNavigations)
                    .HasForeignKey(d => d.Receiver)
                    .HasConstraintName("chatconf_receiver_fkey");

                entity.HasOne(d => d.SenderNavigation)
                    .WithMany(p => p.ChatconfSenderNavigations)
                    .HasForeignKey(d => d.Sender)
                    .HasConstraintName("chatconf_sender_fkey");
            });

            modelBuilder.Entity<Cmtinteract>(entity =>
            {
                entity.HasKey(e => new { e.Userid, e.Commentid })
                    .HasName("cmtinteract_pkey");

                entity.ToTable("cmtinteract");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Commentid).HasColumnName("commentid");

                entity.Property(e => e.InteractType).HasColumnName("interact_type");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.Cmtinteracts)
                    .HasForeignKey(d => d.Commentid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cmtinteract_commentid_fkey");

                entity.HasOne(d => d.InteractTypeNavigation)
                    .WithMany(p => p.Cmtinteracts)
                    .HasForeignKey(d => d.InteractType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cmtinteract_interact_type_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Cmtinteracts)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cmtinteract_userid_fkey");
            });

            modelBuilder.Entity<Cmtpunish>(entity =>
            {
                entity.HasKey(e => e.Commentid)
                    .HasName("cmtpunish_pkey");

                entity.ToTable("cmtpunish");

                entity.Property(e => e.Commentid)
                    .HasColumnName("commentid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Adminid).HasColumnName("adminid");

                entity.Property(e => e.Punishid).HasColumnName("punishid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Cmtpunishes)
                    .HasForeignKey(d => d.Adminid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cmtpunish_adminid_fkey");

                entity.HasOne(d => d.Comment)
                    .WithOne(p => p.Cmtpunish)
                    .HasForeignKey<Cmtpunish>(d => d.Commentid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cmtpunish_commentid_fkey");

                entity.HasOne(d => d.Punish)
                    .WithMany(p => p.Cmtpunishes)
                    .HasForeignKey(d => d.Punishid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("cmtpunish_punishid_fkey");
            });

            modelBuilder.Entity<Cmtreport>(entity =>
            {
                entity.ToTable("cmtreport");

                entity.Property(e => e.Cmtreportid)
                    .HasColumnName("cmtreportid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("reason");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("comment");

                entity.Property(e => e.Commentid)
                    .HasColumnName("commentid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("timezone('utc'::text, now())");

                entity.Property(e => e.IsAudio)
                    .HasColumnName("is_audio")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsCorrect)
                    .HasColumnName("is_correct")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsImage)
                    .HasColumnName("is_image")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsRemoved)
                    .HasColumnName("is_removed")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Postid).HasColumnName("postid");

                entity.Property(e => e.Text)
                    .HasMaxLength(2048)
                    .HasColumnName("text");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comment_postid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("comment_userid_fkey");
            });

            modelBuilder.Entity<Correctcmt>(entity =>
            {
                entity.ToTable("correctcmt");

                entity.Property(e => e.Correctcmtid)
                    .HasColumnName("correctcmtid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Commentid).HasColumnName("commentid");

                entity.Property(e => e.CorrectText)
                    .HasMaxLength(2048)
                    .HasColumnName("correct_text");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.Correctcmts)
                    .HasForeignKey(d => d.Commentid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("correctcmt_commentid_fkey");
            });

            modelBuilder.Entity<Correctmsg>(entity =>
            {
                entity.HasKey(e => e.Messid)
                    .HasName("correctmsg_pkey");

                entity.ToTable("correctmsg");

                entity.Property(e => e.Messid)
                    .HasColumnName("messid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CorrectText)
                    .HasMaxLength(1024)
                    .HasColumnName("correct_text");

                entity.HasOne(d => d.Mess)
                    .WithOne(p => p.Correctmsg)
                    .HasForeignKey<Correctmsg>(d => d.Messid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("correctmsg_messid_fkey");
            });

            modelBuilder.Entity<FlywaySchemaHistory>(entity =>
            {
                entity.HasKey(e => e.InstalledRank)
                    .HasName("flyway_schema_history_pk");

                entity.ToTable("flyway_schema_history");

                entity.HasIndex(e => e.Success, "flyway_schema_history_s_idx");

                entity.Property(e => e.InstalledRank)
                    .ValueGeneratedNever()
                    .HasColumnName("installed_rank");

                entity.Property(e => e.Checksum).HasColumnName("checksum");

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("description");

                entity.Property(e => e.ExecutionTime).HasColumnName("execution_time");

                entity.Property(e => e.InstalledBy)
                    .IsRequired()
                    .HasMaxLength(100)
                    .HasColumnName("installed_by");

                entity.Property(e => e.InstalledOn)
                    .HasColumnName("installed_on")
                    .HasDefaultValueSql("now()");

                entity.Property(e => e.Script)
                    .IsRequired()
                    .HasMaxLength(1000)
                    .HasColumnName("script");

                entity.Property(e => e.Success).HasColumnName("success");

                entity.Property(e => e.Type)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasColumnName("type");

                entity.Property(e => e.Version)
                    .HasMaxLength(50)
                    .HasColumnName("version");
            });

            modelBuilder.Entity<Group>(entity =>
            {
                entity.ToTable("groups");

                entity.Property(e => e.Groupid)
                    .HasColumnName("groupid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Introduction)
                    .HasMaxLength(1024)
                    .HasColumnName("introduction");

                entity.Property(e => e.IsPublic)
                    .HasColumnName("is_public")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.IsRemoved)
                    .HasColumnName("is_removed")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.PostCheck)
                    .HasColumnName("post_check")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Requirement)
                    .HasMaxLength(1024)
                    .HasColumnName("requirement");
            });

            modelBuilder.Entity<Groupmember>(entity =>
            {
                entity.HasKey(e => new { e.Userid, e.Groupid })
                    .HasName("groupmember_pkey");

                entity.ToTable("groupmember");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Groupid).HasColumnName("groupid");

                entity.Property(e => e.IsRemoved)
                    .HasColumnName("is_removed")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.RestrictLevel)
                    .HasColumnType("bit(2)")
                    .HasColumnName("restrict_level")
                    .HasDefaultValueSql("'00'::\"bit\"");

                entity.Property(e => e.Roleid).HasColumnName("roleid");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Groupmembers)
                    .HasForeignKey(d => d.Groupid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("groupmember_groupid_fkey");

                entity.HasOne(d => d.Role)
                    .WithMany(p => p.Groupmembers)
                    .HasForeignKey(d => d.Roleid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("groupmember_roleid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Groupmembers)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("groupmember_userid_fkey");
            });

            modelBuilder.Entity<Groupopreq>(entity =>
            {
                entity.HasKey(e => e.Requestid)
                    .HasName("groupopreq_pkey");

                entity.ToTable("groupopreq");

                entity.Property(e => e.Requestid)
                    .HasColumnName("requestid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.IsQualified)
                    .HasColumnName("is_qualified")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Ownerid).HasColumnName("ownerid");

                entity.Property(e => e.Text)
                    .HasMaxLength(2048)
                    .HasColumnName("text");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Groupopreqs)
                    .HasForeignKey(d => d.Ownerid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("groupopreq_ownerid_fkey");
            });

            modelBuilder.Entity<Grouppost>(entity =>
            {
                entity.HasKey(e => e.Postid)
                    .HasName("grouppost_pkey");

                entity.ToTable("grouppost");

                entity.Property(e => e.Postid)
                    .HasColumnName("postid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Groupid).HasColumnName("groupid");

                entity.Property(e => e.IsHidden)
                    .HasColumnName("is_hidden")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsQualified)
                    .HasColumnName("is_qualified")
                    .HasDefaultValueSql("true");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Groupposts)
                    .HasForeignKey(d => d.Groupid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("grouppost_groupid_fkey");

                entity.HasOne(d => d.Post)
                    .WithOne(p => p.Grouppost)
                    .HasForeignKey<Grouppost>(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("grouppost_postid_fkey");
            });

            modelBuilder.Entity<Grouppunish>(entity =>
            {
                entity.HasKey(e => new { e.Adminid, e.Groupid, e.Punishid })
                    .HasName("grouppunish_pkey");

                entity.ToTable("grouppunish");

                entity.Property(e => e.Adminid).HasColumnName("adminid");

                entity.Property(e => e.Groupid).HasColumnName("groupid");

                entity.Property(e => e.Punishid).HasColumnName("punishid");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Grouppunishes)
                    .HasForeignKey(d => d.Adminid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("grouppunish_adminid_fkey");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Grouppunishes)
                    .HasForeignKey(d => d.Groupid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("grouppunish_groupid_fkey");

                entity.HasOne(d => d.Punish)
                    .WithMany(p => p.Grouppunishes)
                    .HasForeignKey(d => d.Punishid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("grouppunish_punishid_fkey");
            });

            modelBuilder.Entity<Hobby>(entity =>
            {
                entity.ToTable("hobby");

                entity.Property(e => e.Hobbyid)
                    .HasColumnName("hobbyid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Imagecmt>(entity =>
            {
                entity.ToTable("imagecmt");

                entity.Property(e => e.Imagecmtid)
                    .HasColumnName("imagecmtid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Commentid).HasColumnName("commentid");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("url");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.Imagecmts)
                    .HasForeignKey(d => d.Commentid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("imagecmt_commentid_fkey");
            });

            modelBuilder.Entity<Imagemsg>(entity =>
            {
                entity.HasKey(e => e.Messid)
                    .HasName("imagemsg_pkey");

                entity.ToTable("imagemsg");

                entity.Property(e => e.Messid)
                    .HasColumnName("messid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.HasOne(d => d.Mess)
                    .WithOne(p => p.Imagemsg)
                    .HasForeignKey<Imagemsg>(d => d.Messid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("imagemsg_messid_fkey");
            });

            modelBuilder.Entity<Imagemsgurl>(entity =>
            {
                entity.HasKey(e => e.Urlid)
                    .HasName("imagemsgurl_pkey");

                entity.ToTable("imagemsgurl");

                entity.Property(e => e.Urlid)
                    .HasColumnName("urlid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Messid).HasColumnName("messid");

                entity.Property(e => e.Url)
                    .HasMaxLength(256)
                    .HasColumnName("url");

                entity.HasOne(d => d.Mess)
                    .WithMany(p => p.Imagemsgurls)
                    .HasForeignKey(d => d.Messid)
                    .HasConstraintName("imagemsgurl_messid_fkey");
            });

            modelBuilder.Entity<Imagepost>(entity =>
            {
                entity.ToTable("imagepost");

                entity.Property(e => e.Imagepostid)
                    .HasColumnName("imagepostid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Postid).HasColumnName("postid");

                entity.Property(e => e.Url)
                    .HasMaxLength(256)
                    .HasColumnName("url");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Imageposts)
                    .HasForeignKey(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("imagepost_postid_fkey");
            });

            modelBuilder.Entity<Interaction>(entity =>
            {
                entity.HasKey(e => e.Interactid)
                    .HasName("interaction_pkey");

                entity.ToTable("interaction");

                entity.Property(e => e.Interactid)
                    .HasColumnName("interactid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Stringcode)
                    .IsRequired()
                    .HasMaxLength(16)
                    .HasColumnName("stringcode");
            });

            modelBuilder.Entity<Joingrpreq>(entity =>
            {
                entity.HasKey(e => new { e.Userid, e.Groupid })
                    .HasName("joingrpreq_pkey");

                entity.ToTable("joingrpreq");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Groupid).HasColumnName("groupid");

                entity.Property(e => e.IsRemoved)
                    .HasColumnName("is_removed")
                    .HasDefaultValueSql("false");

                entity.HasOne(d => d.Group)
                    .WithMany(p => p.Joingrpreqs)
                    .HasForeignKey(d => d.Groupid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("joingrpreq_groupid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Joingrpreqs)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("joingrpreq_userid_fkey");
            });

            modelBuilder.Entity<Language>(entity =>
            {
                entity.HasKey(e => e.Langid)
                    .HasName("language_pkey");

                entity.ToTable("language");

                entity.Property(e => e.Langid)
                    .HasColumnName("langid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.LocaleCode)
                    .HasMaxLength(15)
                    .HasColumnName("locale_code");

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .HasColumnName("name");
            });

            modelBuilder.Entity<Message>(entity =>
            {
                entity.HasKey(e => e.Messid)
                    .HasName("messages_pkey");

                entity.ToTable("messages");

                entity.Property(e => e.Messid)
                    .HasColumnName("messid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Receiver).HasColumnName("receiver");

                entity.Property(e => e.Sender).HasColumnName("sender");

                entity.Property(e => e.Text)
                    .HasMaxLength(1024)
                    .HasColumnName("text");

                entity.HasOne(d => d.ReceiverNavigation)
                    .WithMany(p => p.MessageReceiverNavigations)
                    .HasForeignKey(d => d.Receiver)
                    .HasConstraintName("messages_receiver_fkey");

                entity.HasOne(d => d.SenderNavigation)
                    .WithMany(p => p.MessageSenderNavigations)
                    .HasForeignKey(d => d.Sender)
                    .HasConstraintName("messages_sender_fkey");
            });

            modelBuilder.Entity<Notibox>(entity =>
            {
                entity.HasKey(e => e.Boxid)
                    .HasName("notibox_pkey");

                entity.ToTable("notibox");

                entity.Property(e => e.Boxid)
                    .HasColumnName("boxid")
                    .HasDefaultValueSql("uuid_generate_v4()");
            });

            modelBuilder.Entity<Notification>(entity =>
            {
                entity.HasKey(e => e.Notiid)
                    .HasName("notifications_pkey");

                entity.ToTable("notifications");

                entity.Property(e => e.Notiid)
                    .HasColumnName("notiid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Boxid).HasColumnName("boxid");

                entity.Property(e => e.Text)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnName("text");

                entity.Property(e => e.Type).HasColumnName("type");

                entity.Property(e => e.Url)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("url");

                entity.HasOne(d => d.Box)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.Boxid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("notifications_boxid_fkey");

                entity.HasOne(d => d.TypeNavigation)
                    .WithMany(p => p.Notifications)
                    .HasForeignKey(d => d.Type)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("notifications_type_fkey");
            });

            modelBuilder.Entity<Notitype>(entity =>
            {
                entity.HasKey(e => e.Typeid)
                    .HasName("notitype_pkey");

                entity.ToTable("notitype");

                entity.Property(e => e.Typeid)
                    .HasColumnName("typeid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sample)
                    .IsRequired()
                    .HasMaxLength(512)
                    .HasColumnName("sample");

                entity.Property(e => e.Typestring)
                    .IsRequired()
                    .HasMaxLength(32)
                    .HasColumnName("typestring");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("post");

                entity.Property(e => e.Postid)
                    .HasColumnName("postid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("timezone('utc'::text, now())");

                entity.Property(e => e.IsAudio)
                    .HasColumnName("is_audio")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsGroup)
                    .HasColumnName("is_group")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsImage)
                    .HasColumnName("is_image")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsPublic)
                    .HasColumnName("is_public")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.IsRemoved)
                    .HasColumnName("is_removed")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsRoom)
                    .HasColumnName("is_room")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsShare)
                    .HasColumnName("is_share")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsVideo)
                    .HasColumnName("is_video")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Label)
                    .HasMaxLength(100)
                    .HasColumnName("label");

                entity.Property(e => e.Langid).HasColumnName("langid");

                entity.Property(e => e.RestrictBits)
                    .HasColumnType("bit(3)")
                    .HasColumnName("restrict_bits")
                    .HasDefaultValueSql("'000'::\"bit\"");

                entity.Property(e => e.Text)
                    .HasMaxLength(4096)
                    .HasColumnName("text");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.Langid)
                    .HasConstraintName("post_langid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.Userid)
                    .HasConstraintName("post_userid_fkey");
            });

            modelBuilder.Entity<Postpunish>(entity =>
            {
                entity.HasKey(e => e.Postid)
                    .HasName("postpunish_pkey");

                entity.ToTable("postpunish");

                entity.Property(e => e.Postid)
                    .HasColumnName("postid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Adminid).HasColumnName("adminid");

                entity.Property(e => e.Punishid).HasColumnName("punishid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Postpunishes)
                    .HasForeignKey(d => d.Adminid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("postpunish_adminid_fkey");

                entity.HasOne(d => d.Post)
                    .WithOne(p => p.Postpunish)
                    .HasForeignKey<Postpunish>(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("postpunish_postid_fkey");

                entity.HasOne(d => d.Punish)
                    .WithMany(p => p.Postpunishes)
                    .HasForeignKey(d => d.Punishid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("postpunish_punishid_fkey");
            });

            modelBuilder.Entity<Postreport>(entity =>
            {
                entity.ToTable("postreport");

                entity.Property(e => e.Postreportid)
                    .HasColumnName("postreportid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(256)
                    .HasColumnName("reason");
            });

            modelBuilder.Entity<Posttopic>(entity =>
            {
                entity.HasKey(e => new { e.Topicid, e.Postid })
                    .HasName("posttopic_pkey");

                entity.ToTable("posttopic");

                entity.Property(e => e.Topicid).HasColumnName("topicid");

                entity.Property(e => e.Postid).HasColumnName("postid");

                entity.Property(e => e.IsRemoved)
                    .HasColumnName("is_removed")
                    .HasDefaultValueSql("false");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Posttopics)
                    .HasForeignKey(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("posttopic_postid_fkey");

                entity.HasOne(d => d.Topic)
                    .WithMany(p => p.Posttopics)
                    .HasForeignKey(d => d.Topicid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("posttopic_topicid_fkey");
            });

            modelBuilder.Entity<Punishment>(entity =>
            {
                entity.HasKey(e => e.Punishid)
                    .HasName("punishment_pkey");

                entity.ToTable("punishment");

                entity.Property(e => e.Punishid)
                    .HasColumnName("punishid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.IsRestrict)
                    .HasColumnName("is_restrict")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Relapse)
                    .HasColumnName("relapse")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Type)
                    .HasMaxLength(32)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Relationship>(entity =>
            {
                entity.HasKey(e => new { e.User1, e.User2 })
                    .HasName("relationship_pkey");

                entity.ToTable("relationship");

                entity.Property(e => e.User1).HasColumnName("user_1");

                entity.Property(e => e.User2).HasColumnName("user_2");

                entity.Property(e => e.Action)
                    .HasMaxLength(15)
                    .HasColumnName("action");

                entity.Property(e => e.Type)
                    .HasColumnName("type")
                    .HasDefaultValueSql("false");

                entity.HasOne(d => d.User1Navigation)
                    .WithMany(p => p.RelationshipUser1Navigations)
                    .HasForeignKey(d => d.User1)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("relationship_user_1_fkey");

                entity.HasOne(d => d.User2Navigation)
                    .WithMany(p => p.RelationshipUser2Navigations)
                    .HasForeignKey(d => d.User2)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("relationship_user_2_fkey");
            });

            modelBuilder.Entity<Restrict>(entity =>
            {
                entity.ToTable("restrict");

                entity.Property(e => e.Restrictid)
                    .HasColumnName("restrictid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Days).HasColumnName("days");

                entity.Property(e => e.Description)
                    .HasMaxLength(256)
                    .HasColumnName("description");

                entity.Property(e => e.RestrictCode)
                    .HasMaxLength(32)
                    .HasColumnName("restrict_code");
            });

            modelBuilder.Entity<Restrictpunish>(entity =>
            {
                entity.HasKey(e => new { e.Punishid, e.Restrictid })
                    .HasName("restrictpunish_pkey");

                entity.ToTable("restrictpunish");

                entity.Property(e => e.Punishid).HasColumnName("punishid");

                entity.Property(e => e.Restrictid).HasColumnName("restrictid");

                entity.HasOne(d => d.Punish)
                    .WithMany(p => p.Restrictpunishes)
                    .HasForeignKey(d => d.Punishid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("restrictpunish_punishid_fkey");

                entity.HasOne(d => d.Restrict)
                    .WithMany(p => p.Restrictpunishes)
                    .HasForeignKey(d => d.Restrictid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("restrictpunish_restrictid_fkey");
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.ToTable("role");

                entity.Property(e => e.Roleid)
                    .HasColumnName("roleid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Strrole)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasColumnName("strrole");
            });

            modelBuilder.Entity<Roompost>(entity =>
            {
                entity.HasKey(e => e.Postid)
                    .HasName("roompost_pkey");

                entity.ToTable("roompost");

                entity.Property(e => e.Postid)
                    .HasColumnName("postid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Roomid).HasColumnName("roomid");

                entity.HasOne(d => d.Post)
                    .WithOne(p => p.Roompost)
                    .HasForeignKey<Roompost>(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("roompost_postid_fkey");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Roomposts)
                    .HasForeignKey(d => d.Roomid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("roompost_roomid_fkey");
            });

            modelBuilder.Entity<Sharepost>(entity =>
            {
                entity.HasKey(e => e.Postid)
                    .HasName("sharepost_pkey");

                entity.ToTable("sharepost");

                entity.Property(e => e.Postid)
                    .HasColumnName("postid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Sharedpst).HasColumnName("sharedpst");

                entity.HasOne(d => d.Post)
                    .WithOne(p => p.SharepostPost)
                    .HasForeignKey<Sharepost>(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sharepost_postid_fkey");

                entity.HasOne(d => d.SharedpstNavigation)
                    .WithMany(p => p.SharepostSharedpstNavigations)
                    .HasForeignKey(d => d.Sharedpst)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("sharepost_sharedpst_fkey");
            });

            modelBuilder.Entity<Targetlang>(entity =>
            {
                entity.HasKey(e => new { e.Userid, e.Langid })
                    .HasName("targetlang_pkey");

                entity.ToTable("targetlang");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Langid).HasColumnName("langid");

                entity.Property(e => e.TargetLevel)
                    .HasColumnName("target_level")
                    .HasDefaultValueSql("1");

                entity.HasOne(d => d.Lang)
                    .WithMany(p => p.Targetlangs)
                    .HasForeignKey(d => d.Langid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("targetlang_langid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Targetlangs)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("targetlang_userid_fkey");
            });

            modelBuilder.Entity<Topic>(entity =>
            {
                entity.ToTable("topic");

                entity.Property(e => e.Topicid)
                    .HasColumnName("topicid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.IsPublic)
                    .HasColumnName("is_public")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.IsRemoved)
                    .HasColumnName("is_removed")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Name)
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Topics)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("topic_userid_fkey");
            });

            modelBuilder.Entity<Tutorreq>(entity =>
            {
                entity.HasKey(e => e.Requestid)
                    .HasName("tutorreq_pkey");

                entity.ToTable("tutorreq");

                entity.Property(e => e.Requestid)
                    .HasColumnName("requestid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.IsQualified)
                    .HasColumnName("is_qualified")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.Ownerid).HasColumnName("ownerid");

                entity.Property(e => e.Text)
                    .HasMaxLength(2048)
                    .HasColumnName("text");

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.Tutorreqs)
                    .HasForeignKey(d => d.Ownerid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("tutorreq_ownerid_fkey");
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.ToTable("users");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Avartar)
                    .HasMaxLength(256)
                    .HasColumnName("avartar");

                entity.Property(e => e.Country)
                    .HasMaxLength(50)
                    .HasColumnName("country");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("timezone('utc'::text, now())");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(255)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .HasMaxLength(64)
                    .HasColumnName("first_name");

                entity.Property(e => e.Gender)
                    .HasMaxLength(7)
                    .HasColumnName("gender");

                entity.Property(e => e.IncreateId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("increate_id");

                entity.Property(e => e.Introduction)
                    .HasMaxLength(1024)
                    .HasColumnName("introduction");

                entity.Property(e => e.IsRemoved)
                    .HasColumnName("is_removed")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsRestrict)
                    .HasColumnName("is_restrict")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsTutor)
                    .HasColumnName("is_tutor")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.LastName)
                    .HasMaxLength(64)
                    .HasColumnName("last_name");

                entity.Property(e => e.Latt)
                    .HasPrecision(8, 6)
                    .HasColumnName("latt");

                entity.Property(e => e.Longtt)
                    .HasPrecision(9, 6)
                    .HasColumnName("longtt");

                entity.Property(e => e.MiddleName)
                    .HasMaxLength(64)
                    .HasColumnName("middle_name");

                entity.Property(e => e.NativeLang).HasColumnName("native_lang");

                entity.Property(e => e.NativeLevel)
                    .HasColumnName("native_level")
                    .HasDefaultValueSql("1");

                entity.Property(e => e.Notibox).HasColumnName("notibox");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("password");

                entity.Property(e => e.TempToken)
                    .HasMaxLength(512)
                    .HasColumnName("temp_token");

                entity.Property(e => e.TokenIat).HasColumnName("token_iat");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.UserName)
                    .HasMaxLength(32)
                    .HasColumnName("user_name");

                entity.HasOne(d => d.NativeLangNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.NativeLang)
                    .HasConstraintName("users_native_lang_fkey");

                entity.HasOne(d => d.NotiboxNavigation)
                    .WithMany(p => p.Users)
                    .HasForeignKey(d => d.Notibox)
                    .HasConstraintName("users_notibox_fkey");
            });

            modelBuilder.Entity<Userhobby>(entity =>
            {
                entity.HasKey(e => new { e.Userid, e.Hobbyid })
                    .HasName("userhobby_pkey");

                entity.ToTable("userhobby");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Hobbyid).HasColumnName("hobbyid");

                entity.HasOne(d => d.Hobby)
                    .WithMany(p => p.Userhobbies)
                    .HasForeignKey(d => d.Hobbyid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userhobby_hobbyid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userhobbies)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userhobby_userid_fkey");
            });

            modelBuilder.Entity<Userinroom>(entity =>
            {
                entity.HasKey(e => e.Userid)
                    .HasName("userinroom_pkey");

                entity.ToTable("userinroom");

                entity.Property(e => e.Userid)
                    .HasColumnName("userid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Roomid).HasColumnName("roomid");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Userinrooms)
                    .HasForeignKey(d => d.Roomid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userinroom_roomid_fkey");

                entity.HasOne(d => d.User)
                    .WithOne(p => p.Userinroom)
                    .HasForeignKey<Userinroom>(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userinroom_userid_fkey");
            });

            modelBuilder.Entity<Userintpost>(entity =>
            {
                entity.HasKey(e => new { e.Postid, e.Userid })
                    .HasName("userintpost_pkey");

                entity.ToTable("userintpost");

                entity.Property(e => e.Postid).HasColumnName("postid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.InteractType).HasColumnName("interact_type");

                entity.HasOne(d => d.InteractTypeNavigation)
                    .WithMany(p => p.Userintposts)
                    .HasForeignKey(d => d.InteractType)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userintpost_interact_type_fkey");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Userintposts)
                    .HasForeignKey(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userintpost_postid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userintposts)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userintpost_userid_fkey");
            });

            modelBuilder.Entity<Userpunish>(entity =>
            {
                entity.HasKey(e => new { e.Adminid, e.Userid, e.Punishid })
                    .HasName("userpunish_pkey");

                entity.ToTable("userpunish");

                entity.Property(e => e.Adminid).HasColumnName("adminid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Punishid).HasColumnName("punishid");

                entity.HasOne(d => d.Admin)
                    .WithMany(p => p.Userpunishes)
                    .HasForeignKey(d => d.Adminid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userpunish_adminid_fkey");

                entity.HasOne(d => d.Punish)
                    .WithMany(p => p.Userpunishes)
                    .HasForeignKey(d => d.Punishid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userpunish_punishid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userpunishes)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userpunish_userid_fkey");
            });

            modelBuilder.Entity<Userreportpst>(entity =>
            {
                entity.HasKey(e => new { e.Postreportid, e.Postid, e.Userid })
                    .HasName("userreportpst_pkey");

                entity.ToTable("userreportpst");

                entity.Property(e => e.Postreportid).HasColumnName("postreportid");

                entity.Property(e => e.Postid).HasColumnName("postid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Statement)
                    .HasMaxLength(512)
                    .HasColumnName("statement");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Userreportpsts)
                    .HasForeignKey(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userreportpst_postid_fkey");

                entity.HasOne(d => d.Postreport)
                    .WithMany(p => p.Userreportpsts)
                    .HasForeignKey(d => d.Postreportid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userreportpst_postreportid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Userreportpsts)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("userreportpst_userid_fkey");
            });

            modelBuilder.Entity<Usrreportcmt>(entity =>
            {
                entity.HasKey(e => new { e.Cmtreportid, e.Userid, e.Commentid })
                    .HasName("usrreportcmt_pkey");

                entity.ToTable("usrreportcmt");

                entity.Property(e => e.Cmtreportid).HasColumnName("cmtreportid");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.Commentid).HasColumnName("commentid");

                entity.Property(e => e.Statement)
                    .HasMaxLength(512)
                    .HasColumnName("statement");

                entity.HasOne(d => d.Cmtreport)
                    .WithMany(p => p.Usrreportcmts)
                    .HasForeignKey(d => d.Cmtreportid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("usrreportcmt_cmtreportid_fkey");

                entity.HasOne(d => d.Comment)
                    .WithMany(p => p.Usrreportcmts)
                    .HasForeignKey(d => d.Commentid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("usrreportcmt_commentid_fkey");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Usrreportcmts)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("usrreportcmt_userid_fkey");
            });

            modelBuilder.Entity<Videopost>(entity =>
            {
                entity.ToTable("videopost");

                entity.Property(e => e.Videopostid)
                    .HasColumnName("videopostid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Postid).HasColumnName("postid");

                entity.Property(e => e.Url)
                    .HasMaxLength(256)
                    .HasColumnName("url");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Videoposts)
                    .HasForeignKey(d => d.Postid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("videopost_postid_fkey");
            });

            modelBuilder.Entity<Vocabgoal>(entity =>
            {
                entity.HasKey(e => e.Goalid)
                    .HasName("vocabgoal_pkey");

                entity.ToTable("vocabgoal");

                entity.Property(e => e.Goalid)
                    .HasColumnName("goalid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.Amount)
                    .HasColumnName("amount")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Type)
                    .HasMaxLength(20)
                    .HasColumnName("type");
            });

            modelBuilder.Entity<Vocabpackage>(entity =>
            {
                entity.HasKey(e => e.Packageid)
                    .HasName("vocabpackage_pkey");

                entity.ToTable("vocabpackage");

                entity.Property(e => e.Packageid)
                    .HasColumnName("packageid")
                    .HasDefaultValueSql("uuid_generate_v4()");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("timezone('utc'::text, now())");

                entity.Property(e => e.Define)
                    .HasMaxLength(15)
                    .HasColumnName("define");

                entity.Property(e => e.DeletedAt).HasColumnName("deleted_at");

                entity.Property(e => e.Description)
                    .HasMaxLength(512)
                    .HasColumnName("description");

                entity.Property(e => e.Imageurl)
                    .HasMaxLength(256)
                    .HasColumnName("imageurl");

                entity.Property(e => e.IsPracticed)
                    .HasColumnName("is_practiced")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsPublic)
                    .HasColumnName("is_public")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.IsRemoved)
                    .HasColumnName("is_removed")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.IsShared)
                    .HasColumnName("is_shared")
                    .HasDefaultValueSql("true");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(128)
                    .HasColumnName("name");

                entity.Property(e => e.Term)
                    .HasMaxLength(15)
                    .HasColumnName("term");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.Property(e => e.Userid).HasColumnName("userid");

                entity.Property(e => e.VocabularyPairs).HasColumnName("vocabulary_pairs");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Vocabpackages)
                    .HasForeignKey(d => d.Userid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vocabpackage_userid_fkey");
            });

            modelBuilder.Entity<Vocabulary>(entity =>
            {
                entity.HasKey(e => new { e.Packageid, e.Vocabid })
                    .HasName("vocabulary_pkey");

                entity.ToTable("vocabulary");

                entity.Property(e => e.Packageid).HasColumnName("packageid");

                entity.Property(e => e.Vocabid).HasColumnName("vocabid");

                entity.Property(e => e.Back)
                    .HasMaxLength(128)
                    .HasColumnName("back");

                entity.Property(e => e.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql("timezone('utc'::text, now())");

                entity.Property(e => e.Easiness)
                    .HasPrecision(5, 1)
                    .HasColumnName("easiness")
                    .HasDefaultValueSql("2.5");

                entity.Property(e => e.Front)
                    .HasMaxLength(128)
                    .HasColumnName("front");

                entity.Property(e => e.Imageurl)
                    .HasMaxLength(256)
                    .HasColumnName("imageurl");

                entity.Property(e => e.Interval)
                    .HasColumnName("interval")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.IsRemoved)
                    .HasColumnName("is_removed")
                    .HasDefaultValueSql("false");

                entity.Property(e => e.LastLearned).HasColumnName("last_learned");

                entity.Property(e => e.NextLearned).HasColumnName("next_learned");

                entity.Property(e => e.Repetitions)
                    .HasColumnName("repetitions")
                    .HasDefaultValueSql("0");

                entity.Property(e => e.Type)
                    .HasMaxLength(32)
                    .HasColumnName("type")
                    .HasDefaultValueSql("'slide'::character varying");

                entity.Property(e => e.UpdatedAt).HasColumnName("updated_at");

                entity.HasOne(d => d.Package)
                    .WithMany(p => p.Vocabularies)
                    .HasForeignKey(d => d.Packageid)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("vocabulary_packageid_fkey");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
