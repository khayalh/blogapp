using Blogge.Models.EntityModels;

namespace Blogge.Models.Migrations
{
    using Microsoft.AspNet.Identity;
    using Microsoft.AspNet.Identity.EntityFramework;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Diagnostics.CodeAnalysis;
    using System.Linq;

    [ExcludeFromCodeCoverage]
    internal sealed class Configuration : DbMigrationsConfiguration<Blogge.Models.DB.ApplicationDbContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(Blogge.Models.DB.ApplicationDbContext context)
        {
            //context.Roles.AddOrUpdate(
            //    new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "admin" },
            //    new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "moderator" },
            //    new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "redactor" },
            //    new IdentityRole { Id = Guid.NewGuid().ToString(), Name = "user" }
            //);

            //var userStore = new UserStore<ApplicationUser>(context);
            //var userManager = new UserManager<ApplicationUser>(userStore);
            //var userToInsert = new ApplicationUser { UserName = "Admin", Email = "admin@test.com" };
            //userManager.Create(userToInsert, "123456");
        }
    }
}
