using System;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Repositories;
using Blogge.Interfaces.Validators;
using Blogge.Models.Configs;
using Blogge.Models.EntityModels;
using Blogge.Models.ViewModels;
using Blogge.Web.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using Moq;
using NUnit.Framework;

namespace Blogge.WebTests.Controllers
{
    [TestFixture]
    public class ManageControllerTests
    {
        private Mock<ApplicationUserManager> _userManager;
        private ManageController _manageController;
        private Mock<IImageRepository> _imageRepository;
        private Mock<IIdentityFacade> _identityFacade;
        private Mock<IFileTypeValidator> _fileValidator;

        [SetUp]
        public void Setup()
        {
            _identityFacade = new Mock<IIdentityFacade>(MockBehavior.Strict);
            _fileValidator = new Mock<IFileTypeValidator>();
            var userStore = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);
            _userManager = new Mock<ApplicationUserManager>(userStore.Object);
            var authenticationManager = new Mock<IAuthenticationManager>(MockBehavior.Strict);
            var signInManager = new Mock<ApplicationSignInManager>(_userManager.Object, authenticationManager.Object);

            _imageRepository = new Mock<IImageRepository>(MockBehavior.Strict);
            _manageController = new ManageController(
                _userManager.Object, signInManager.Object, _fileValidator.Object, _identityFacade.Object, _imageRepository.Object);

            _userManager.Setup(x => x.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { Id = "1" });

            _identityFacade.Setup(x=>x.GetUserId()).Returns("UserId");
            
        }
        [Test]
        public void When_Index_Returns_IndexView()
        {
            //Given

            //When
            var result = _manageController.Index() as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsNull(result.Model);

        }

        [Test]
        public void When_ChangeAvatar_Returns_ChangeAvatarView()
        {
            //Given

            //When
            var result = _manageController.ChangeAvatar() as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsNull(result.Model);

        }

        [Test]
        public void Given_Valid_ChangeAvatarViewModel_When_ChangeAvatar_Returns_IndexView()
        {
            //Given
            var model = new ChangeAvatarViewModel();
            model.file = new Mock<HttpPostedFileBase>().Object;
            _fileValidator.Setup(x => x.IsValidFiletype(It.IsAny<HttpPostedFileBase>())).Returns(true);

            _imageRepository.Setup(x => x.AddImageToDb("UserId", It.IsAny<HttpPostedFileBase>()));

            //When
            var result = _manageController.ChangeAvatar(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Manage", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());

        }

        [Test]
        public void Given_inValid_ChangeAvatarViewModel_When_ChangeAvatar_Returns_IndexView()
        {
            //Given
            var model = new ChangeAvatarViewModel();
            model.file = new Mock<HttpPostedFileBase>().Object;
            _fileValidator.Setup(x => x.IsValidFiletype(It.IsAny<HttpPostedFileBase>())).Returns(false);

            _imageRepository.Setup(x => x.AddImageToDb("UserId", It.IsAny<HttpPostedFileBase>()));

            //When
            var result = _manageController.ChangeAvatar(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsInstanceOf<ChangeAvatarViewModel>(result.Model);

        }

        [Test]
        public void When_ChangePassword_Returns_ChangePasswordView()
        {
            //Given

            //When
            var result = _manageController.ChangePassword() as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsNull(result.Model);

        }

        [Test]
        public async Task Given_Valid_ChangePasswordModel_When_ChangePassword_Returns_index()
        {
            //Given
            ChangePasswordViewModel model = new ChangePasswordViewModel()
            {
                OldPassword = "oldPass",
                NewPassword = "newPass",
                ConfirmPassword = "newPass"
            };

            //When
            var result = await _manageController.ChangePassword(model) as RedirectToRouteResult ;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());

        }

        [Test]
        public async Task Given_inValid_ChangePasswordModel_When_ChangePassword_Returns_ChangePasswordView()
        {
            //Given
            ChangePasswordViewModel model = new ChangePasswordViewModel()
            {
                
            };
            _manageController.ModelState.AddModelError(string.Empty, "There is something wrong with model.");

            //When
            var result = await _manageController.ChangePassword(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsInstanceOf<ChangePasswordViewModel>(result.Model);

        }

        [Test]
        public async Task Given_userManager_error_When_ChangePassword_Returns_ChangePasswordView()
        {
            //Given
            ChangePasswordViewModel model = new ChangePasswordViewModel()
            {
            };

            _userManager.Setup(x => x.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            //When
            var result = await _manageController.ChangePassword(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsInstanceOf<ChangePasswordViewModel>(result.Model);

        }
    }
}