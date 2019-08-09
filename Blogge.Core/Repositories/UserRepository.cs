using Blogge.Interfaces.Facades.DB;
using Blogge.Interfaces.Repositories;
using Blogge.Models;
using Blogge.Models.DB;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using Blogge.Models.EntityModels;

namespace Blogge.Core.Repositories
{
   public class UserRepository : IUserRepository
    {
        private readonly IDBContextFacade _dBContext;
        private ApplicationDbContext _db;

        public UserRepository(IDBContextFacade dbContext)
        {
            _dBContext = dbContext;
            _db = _dBContext.GetDBContext();
        }

        public List<ApplicationUser> GetUsers()
        {
            return _db.Users.ToList();
        }

        public ApplicationUser GetUser(string userId)
        {
            return _db.Users.Find(userId);
        }

        public List<IdentityUserRole> GetUserRoles()
        {
            return _db.UserRoles.ToList();
        }

        public List<IdentityRole> GetRoles()
        {
            return _db.Roles.ToList();
        }
        public IdentityRole GetUserRole(string id)
        {
           var roleId = _db.UserRoles.Where(x => x.UserId == id).SingleOrDefault().RoleId;
            return _db.Roles.Find(roleId);
        }
    }
}
