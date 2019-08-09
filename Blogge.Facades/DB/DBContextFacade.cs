using System.Diagnostics.CodeAnalysis;
using Blogge.Interfaces.Facades.DB;
using Blogge.Models.DB;
using Microsoft.AspNet.Identity.EntityFramework;


namespace Blogge.Facades.DB
{
    [ExcludeFromCodeCoverage]
    public class DBContextFacade : IDBContextFacade
    {
        public ApplicationDbContext GetDBContext()
        {
            return new ApplicationDbContext();
        }
        public IdentityDbContext GetIdentityDBContext()
        {
            return new IdentityDbContext();
        }
    }
}
