using System.Collections.Generic;
using Blogge.Models.EntityModels;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blogge.Interfaces.Repositories
{
   public interface IUserRepository
    {
        List<IdentityRole> GetRoles();
        List<IdentityUserRole> GetUserRoles();
        List<ApplicationUser> GetUsers();
        ApplicationUser GetUser(string userId);
        IdentityRole GetUserRole(string id);
    }
}