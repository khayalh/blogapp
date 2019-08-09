using Blogge.Interfaces.Facades.Identity;
using Blogge.Models.Configs;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Blogge.Models.EntityModels;

namespace Blogge.Facades.Identity
{
    [ExcludeFromCodeCoverage]
    public class UserManagerFacade : IUserManagerFacade
    {
        private ApplicationUserManager _userManager;

        public UserManagerFacade()
        {
            var httpContext = HttpContext.Current;
            _userManager = httpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
        }

        public string GetRole(string userId)
        {
            return _userManager.GetRolesAsync(userId).Result.Single();
        }

        public async Task<ApplicationUser> FindByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(string userId, string currentRoleName)
        {
            return await _userManager.RemoveFromRoleAsync(userId, currentRoleName);
        }

        public async Task<IdentityResult> AddToRoleAsync(string userId, string selectedRoleName)
        {
            return await _userManager.AddToRoleAsync(userId, selectedRoleName);
        }

        public async Task<IdentityResult> UpdateAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }
    }
}
