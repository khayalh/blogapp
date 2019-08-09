//namespace Blogge.Builders.jj
//{
//    using System;
//    using System.Data.Entity;
//    using System.ComponentModel.DataAnnotations.Schema;
//    using System.Linq;

//    public partial class Model1 : DbContext
//    {
//        public Model1()
//            : base("name=Model11")
//        {
//        }

//        public virtual DbSet<C__MigrationHistory> C__MigrationHistory { get; set; }
//        public virtual DbSet<ApplicationUser> ApplicationUsers { get; set; }
//        public virtual DbSet<Comment> Comments { get; set; }
//        public virtual DbSet<IdentityRole> IdentityRoles { get; set; }
//        public virtual DbSet<IdentityUserClaim> IdentityUserClaims { get; set; }
//        public virtual DbSet<IdentityUserLogin> IdentityUserLogins { get; set; }
//        public virtual DbSet<IdentityUserRole> IdentityUserRoles { get; set; }
//        public virtual DbSet<Like> Likes { get; set; }
//        public virtual DbSet<Picture> Pictures { get; set; }
//        public virtual DbSet<PostPicture> PostPictures { get; set; }
//        public virtual DbSet<Post> Posts { get; set; }

//        protected override void OnModelCreating(DbModelBuilder modelBuilder)
//        {
//            modelBuilder.Entity<ApplicationUser>()
//                .HasMany(e => e.Comments)
//                .WithRequired(e => e.ApplicationUser)
//                .HasForeignKey(e => e.AuthorId)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<ApplicationUser>()
//                .HasMany(e => e.IdentityUserClaims)
//                .WithOptional(e => e.ApplicationUser)
//                .HasForeignKey(e => e.ApplicationUser_Id);

//            modelBuilder.Entity<ApplicationUser>()
//                .HasMany(e => e.IdentityUserLogins)
//                .WithOptional(e => e.ApplicationUser)
//                .HasForeignKey(e => e.ApplicationUser_Id);

//            modelBuilder.Entity<ApplicationUser>()
//                .HasMany(e => e.IdentityUserRoles)
//                .WithOptional(e => e.ApplicationUser)
//                .HasForeignKey(e => e.ApplicationUser_Id);

//            modelBuilder.Entity<ApplicationUser>()
//                .HasMany(e => e.Likes)
//                .WithRequired(e => e.ApplicationUser)
//                .HasForeignKey(e => e.UserID)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<ApplicationUser>()
//                .HasMany(e => e.Pictures)
//                .WithRequired(e => e.ApplicationUser)
//                .HasForeignKey(e => e.User_Id)
//                .WillCascadeOnDelete(false);

//            modelBuilder.Entity<ApplicationUser>()
//                .HasMany(e => e.Posts)
//                .WithRequired(e => e.ApplicationUser)
//                .HasForeignKey(e => e.AuthorId);

//            modelBuilder.Entity<IdentityRole>()
//                .HasMany(e => e.IdentityUserRoles)
//                .WithOptional(e => e.IdentityRole)
//                .HasForeignKey(e => e.IdentityRole_Id);

//            modelBuilder.Entity<Post>()
//                .HasOptional(e => e.PostPicture)
//                .WithRequired(e => e.Post)
//                .WillCascadeOnDelete();
//        }
//    }
//}
