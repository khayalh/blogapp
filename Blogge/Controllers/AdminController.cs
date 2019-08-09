
using Blogge.Interfaces.Builders.Administration;
using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Repositories;
using Blogge.Models.ViewModels;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Blogge.Web.Controllers
{
    [Authorize(Roles = "admin,moderator")]
    [HandleError]
    public class AdminController : Controller
    {
        private readonly IAdminModelBuilder _adminModelBuilder;
        private readonly IUserRepository _userRepository;
        private readonly IBlogRepository _postRepository;
        private readonly IUserManagerFacade _userManager;

        public AdminController(IAdminModelBuilder adminModelBuilder, IUserRepository userRepository, IBlogRepository postRepository, IUserManagerFacade userManagerFacade)
        {
            _adminModelBuilder = adminModelBuilder;
            _userRepository = userRepository;
            _postRepository = postRepository;
            _userManager = userManagerFacade;
        }

        public ActionResult ManageUserRoles()
        {
            var model = _adminModelBuilder.BuildUserRoleViewModel();

            return View(model);
        }

        public ActionResult ManageReports()
        {
            var model = _adminModelBuilder.BuildReportManagerViewModel();

            return View(model);
        }

        [Authorize(Roles = "admin")]
        [HttpPost]
        public async Task<ActionResult> ChangeRole(UserRoleViewModel model)
        {
            var currentRole = _userManager.GetRole(model.UserId);

            if (currentRole != model.SelectedRole)
            {
                var currentUser = await _userManager.FindByIdAsync(model.UserId);

                await _userManager.RemoveFromRoleAsync(model.UserId, currentRole);
                await _userManager.AddToRoleAsync(model.UserId, model.SelectedRole);
                await _userManager.UpdateAsync(currentUser);
            }
            return RedirectToAction("ManageUserRoles");
        }

        [Authorize(Roles = "admin,moderator")]
        public ActionResult BlockComment(int commentId)
        {
            _postRepository.BlockComment(commentId);
            return RedirectToAction("ManageReports");
        }

        [Authorize(Roles = "admin")]
        public ActionResult DeleteReport(int reportId)
        {
            _postRepository.DeleteReport(reportId);
            return RedirectToAction("ManageReports");
        }
    }
}