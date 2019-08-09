using Blogge.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Web.Mvc;

namespace Blogge.Interfaces.Builders.Administration
{
   public interface IAdminModelBuilder
    {
        AllUserRoleViewModel BuildUserRoleViewModel();
        SelectList BuildAvailableRolesList(IdentityRole Role);
        ReportManagerViewModel BuildReportManagerViewModel();
    }
}
