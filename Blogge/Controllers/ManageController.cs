
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Repositories;
using Blogge.Interfaces.Validators;
using Blogge.Models.Configs;
using Blogge.Models.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace Blogge.Web.Controllers
{
    [Authorize]
    [HandleError]
    public class ManageController : Controller
    {
        private ApplicationSignInManager _signInManager;
        private ApplicationUserManager _userManager;
        private readonly IImageRepository _imageRepository;
        private readonly IIdentityFacade _identityFacade;
        private readonly IFileTypeValidator _fileValidator;

        public ManageController(ApplicationUserManager userManager, ApplicationSignInManager signInManager, IFileTypeValidator FileValidator, IIdentityFacade identityFacade, IImageRepository ImageRepository)
        {
            UserManager = userManager;
            SignInManager = signInManager;
            _imageRepository = ImageRepository;
            _identityFacade = identityFacade;
            _fileValidator = FileValidator;
        }

        public ApplicationSignInManager SignInManager
        {
            get
            {
                return _signInManager ?? HttpContext.GetOwinContext().Get<ApplicationSignInManager>();
            }
            private set
            {
                _signInManager = value;
            }
        }

        public ApplicationUserManager UserManager
        {
            get
            {
                return _userManager ?? HttpContext.GetOwinContext().GetUserManager<ApplicationUserManager>();
            }
            private set
            {
                _userManager = value;
            }
        }

        //
        // GET: /Manage/Index
        public ActionResult Index()
        {

            return View();
        }


        // GET: /Manage/ChangeAvatar
        public ActionResult ChangeAvatar()
        {
            return View();
        }

        // GET: /Manage/ChangeAvatar
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ChangeAvatar(ChangeAvatarViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userId = _identityFacade.GetUserId();
                if (!_fileValidator.IsValidFiletype(model.file))
                {
                    ModelState.AddModelError("", "Invalid file format!");
                    return View(model);
                }
                _imageRepository.AddImageToDb(userId, model.file);
                return RedirectToAction("Index", "Manage");
            }

            return View();
        }

        public ActionResult ChangePassword()
        {
            return View();
        }

        //
        // POST: /Manage/ChangePassword
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ChangePassword(ChangePasswordViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var result = await UserManager.ChangePasswordAsync(_identityFacade.GetUserId(), model.OldPassword, model.NewPassword);
            if (result.Succeeded)
            {
                var user = await UserManager.FindByIdAsync(_identityFacade.GetUserId());
                if (user != null)
                {
                    await SignInManager.SignInAsync(user, isPersistent: false, rememberBrowser: false);
                }
                return RedirectToAction("Index", new { Message = ManageMessageId.ChangePasswordSuccess });
            }
            AddErrors(result);
            return View(model);
        }

        #region Helpers
        // Used for XSRF protection when adding external logins
        private const string XsrfKey = "XsrfId";

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error);
            }
        }

        private bool HasPassword()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PasswordHash != null;
            }
            return false;
        }

        private bool HasPhoneNumber()
        {
            var user = UserManager.FindById(User.Identity.GetUserId());
            if (user != null)
            {
                return user.PhoneNumber != null;
            }
            return false;
        }

        public enum ManageMessageId
        {
            AddPhoneSuccess,
            ChangePasswordSuccess,
            SetTwoFactorSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
            RemovePhoneSuccess,
            Error
        }

        #endregion
    }
}