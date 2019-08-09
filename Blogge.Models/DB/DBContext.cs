
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;
using System.Diagnostics.CodeAnalysis;
using Blogge.Models.EntityModels;

namespace Blogge.Models.DB
{
    [ExcludeFromCodeCoverage]
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<PostPicture> PostPictures { get; set; }
        public virtual DbSet<Like> PostLikes { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public DbSet<IdentityUserRole> UserRoles { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<ApplicationUser>()
                .HasOptional(c1 => c1.Avatar).
                WithRequired(c2 => c2.User);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(u => u.UserPosts)
                .WithRequired(u => u.User)
                .HasForeignKey(u => u.AuthorId)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<Post>()
                .HasOptional(c1 => c1.postPicture)
                .WithRequired(c2 => c2.Post)
                .WillCascadeOnDelete(true);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.UserLikes)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.UserID);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(x => x.UserComments)
                .WithRequired(x => x.User)
                .HasForeignKey(x => x.AuthorId)
            .WillCascadeOnDelete(false);

            modelBuilder.Entity<Post>()
               .HasMany(x => x.Likes)
               .WithRequired(x => x.Post)
               .HasForeignKey(x => x.PostId).
               WillCascadeOnDelete(false);

            //modelBuilder.Entity<Post>()
            //   .HasMany(x => x.Comments)
            //   .WithRequired(x => x.Post)
            //   .HasForeignKey(x => x.PostId);

            modelBuilder.Entity<Comment>()
               .HasRequired(x => x.Post)
               .WithMany(x => x.Comments)
               .HasForeignKey(X => X.PostId);

            modelBuilder.Entity<Report>()
              .HasRequired(x => x.Comment)
              .WithMany(X => X.Reports)
              .HasForeignKey(x => x.CommentId).
              WillCascadeOnDelete(false);
  
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
