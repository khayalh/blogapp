using Blogge.Models.DB;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Blogge.Interfaces.Facades.DB
{
    public interface IDBContextFacade
    {
        ApplicationDbContext GetDBContext();
        IdentityDbContext GetIdentityDBContext();
    }
}
