using Blogge.Models;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Data.Entity;


namespace Blogge.Core.DB
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Post> Posts { get; set; }
        public virtual DbSet<Picture> Pictures { get; set; }
        public virtual DbSet<PostPicture> PostPictures { get; set; }

        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

            modelBuilder.Entity<IdentityUserLogin>().HasKey<string>(l => l.UserId);
            modelBuilder.Entity<IdentityRole>().HasKey<string>(r => r.Id);
            modelBuilder.Entity<IdentityUserRole>().HasKey(r => new { r.RoleId, r.UserId });

            modelBuilder.Entity<ApplicationUser>().HasOptional(c1 => c1.Avatar).WithRequired(c2 => c2.User);
            modelBuilder.Entity<ApplicationUser>().HasMany(u => u.UserPosts).WithRequired(u => u.User).HasForeignKey(u => u.AuthorId);
            modelBuilder.Entity<Post>().HasOptional(c1 => c1.PostPicture).WithRequired(c2 => c2.Post);
        }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
