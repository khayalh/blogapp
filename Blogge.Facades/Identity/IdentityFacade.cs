
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Blogge.Interfaces.Facades.Identity;
using Microsoft.Owin.Security.DataProtection;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Diagnostics.CodeAnalysis;
using Blogge.Models.EntityModels;

namespace Blogge.Facades.Identity
{
    [ExcludeFromCodeCoverage]
    public class IdentityFacade : Controller, IIdentityFacade
    {
        HttpContext _httpContext;

        public IdentityFacade()
        {
            _httpContext = System.Web.HttpContext.Current;
        }

        public string GetUserName()
        {
            return _httpContext.User.Identity.GetUserName();
        }

        public bool CheckRole(string role)
        {
            return _httpContext.User.IsInRole(role);
        } 
        public DataProtectorTokenProvider<ApplicationUser> GetUserTokenProvider(DpapiDataProtectionProvider provider, string arg)
        {
            return new DataProtectorTokenProvider<ApplicationUser>(provider.Create(arg));
        }
        
        public DpapiDataProtectionProvider GetProvider(string arg)
        {
            return new DpapiDataProtectionProvider(arg);
        }

        public string GetUserId()
        {
            return _httpContext.User.Identity.GetUserId();
        }
        public RouteData GetRouteData()
        {
            return _httpContext.Request.RequestContext.RouteData;
        }
        public IAuthenticationManager GetAuthenticationManager()
        {
            return _httpContext.GetOwinContext().Authentication;
        }
    }
}
