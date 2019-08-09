using System.Threading.Tasks;
using Blogge.Models;
using Blogge.Models.EntityModels;
using Microsoft.AspNet.Identity;

namespace Blogge.Interfaces.Facades.Identity
{
    public interface IUserManagerFacade
    {
        Task<IdentityResult> AddToRoleAsync(string userId, string selectedRoleName);
        Task<ApplicationUser> FindByIdAsync(string userId);
        string GetRole(string userId);
        Task<IdentityResult> RemoveFromRoleAsync(string userId, string currentRoleName);
        Task<IdentityResult> UpdateAsync(ApplicationUser user);
    }
}