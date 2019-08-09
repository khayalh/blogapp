using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using System.Web.Routing;
using Blogge.Models.EntityModels;

namespace Blogge.Interfaces.Facades.Identity
{
    public interface IIdentityFacade
    {
        string GetUserName();
        string GetUserId();
        RouteData GetRouteData();
        IAuthenticationManager GetAuthenticationManager();
        bool CheckRole(string role);
        DpapiDataProtectionProvider GetProvider(string arg);
        DataProtectorTokenProvider<ApplicationUser> GetUserTokenProvider(DpapiDataProtectionProvider provider, string arg);
    }
}