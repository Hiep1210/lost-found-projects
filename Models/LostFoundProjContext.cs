using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace _2._LostFoundProj.Models
{
    public partial class LostFoundProjContext : DbContext
    {
        public LostFoundProjContext()
        {
        }

        public LostFoundProjContext(DbContextOptions<LostFoundProjContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; } = null!;
        public virtual DbSet<Chat> Chats { get; set; } = null!;
        public virtual DbSet<Cmessage> Cmessages { get; set; } = null!;
        public virtual DbSet<Comment> Comments { get; set; } = null!;
        public virtual DbSet<Imag> Imags { get; set; } = null!;
        public virtual DbSet<ItemType> ItemTypes { get; set; } = null!;
        public virtual DbSet<Pending> Pendings { get; set; } = null!;
        public virtual DbSet<Post> Posts { get; set; } = null!;

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Data Source=DESKTOP-HT298IF\\SQLEXPRESS;Initial Catalog=LostFoundProj;User ID=sa;Password=123456");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Account>(entity =>
            {
                entity.ToTable("Account");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Dob)
                    .HasColumnType("date")
                    .HasColumnName("dob");

                entity.Property(e => e.Pass)
                    .HasMaxLength(50)
                    .HasColumnName("pass");

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .HasColumnName("phone");

                entity.Property(e => e.Username)
                    .HasMaxLength(50)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<Chat>(entity =>
            {
                entity.ToTable("Chat");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.User1).HasColumnName("user_1");

                entity.Property(e => e.User2).HasColumnName("user_2");

                entity.HasOne(d => d.User1Navigation)
                    .WithMany(p => p.ChatUser1Navigations)
                    .HasForeignKey(d => d.User1)
                    .HasConstraintName("FK__Chat__user_1__30F848ED");

                entity.HasOne(d => d.User2Navigation)
                    .WithMany(p => p.ChatUser2Navigations)
                    .HasForeignKey(d => d.User2)
                    .HasConstraintName("FK__Chat__user_2__31EC6D26");
            });

            modelBuilder.Entity<Cmessage>(entity =>
            {
                entity.ToTable("CMessage");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Mess)
                    .HasMaxLength(100)
                    .HasColumnName("mess");

                entity.Property(e => e.RoomId).HasColumnName("room_id");

                entity.Property(e => e.Sdate)
                    .HasColumnType("datetime")
                    .HasColumnName("sdate");

                entity.Property(e => e.SenderId).HasColumnName("sender_id");

                entity.HasOne(d => d.Room)
                    .WithMany(p => p.Cmessages)
                    .HasForeignKey(d => d.RoomId)
                    .HasConstraintName("FK__CMessage__room_i__35BCFE0A");

                entity.HasOne(d => d.Sender)
                    .WithMany(p => p.Cmessages)
                    .HasForeignKey(d => d.SenderId)
                    .HasConstraintName("FK__CMessage__sender__34C8D9D1");
            });

            modelBuilder.Entity<Comment>(entity =>
            {
                entity.ToTable("Comment");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Com)
                    .HasMaxLength(50)
                    .HasColumnName("com");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__Comment__post_id__398D8EEE");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.Comments)
                    .HasForeignKey(d => d.UserId)
                    .HasConstraintName("FK__Comment__user_id__38996AB5");
            });

            modelBuilder.Entity<Imag>(entity =>
            {
                entity.ToTable("Imag");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ImgName)
                    .HasMaxLength(50)
                    .HasColumnName("img_name");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.HasOne(d => d.Post)
                    .WithMany(p => p.Imags)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__Imag__post_id__2B3F6F97");
            });

            modelBuilder.Entity<ItemType>(entity =>
            {
                entity.ToTable("ItemType");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Itype)
                    .HasMaxLength(100)
                    .HasColumnName("itype");
            });

            modelBuilder.Entity<Pending>(entity =>
            {
                entity.ToTable("Pending");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Caption).HasColumnName("caption");

                entity.Property(e => e.Lostorfound).HasColumnName("lostorfound");

                entity.Property(e => e.Postdate)
                    .HasColumnType("datetime")
                    .HasColumnName("postdate");

                entity.Property(e => e.Poster).HasColumnName("poster");

                entity.Property(e => e.Processed).HasColumnName("processed");

                entity.HasOne(d => d.PosterNavigation)
                    .WithMany(p => p.Pendings)
                    .HasForeignKey(d => d.Poster)
                    .HasConstraintName("FK__Pending__poster__286302EC");
            });

            modelBuilder.Entity<Post>(entity =>
            {
                entity.ToTable("Post");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.PostId).HasColumnName("post_id");

                entity.HasOne(d => d.PostNavigation)
                    .WithMany(p => p.Posts)
                    .HasForeignKey(d => d.PostId)
                    .HasConstraintName("FK__Post__post_id__2E1BDC42");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
