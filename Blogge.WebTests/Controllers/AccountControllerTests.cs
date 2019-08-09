using System;
using System.IO;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blogge.Interfaces.Builders.Account;
using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Facades.Systems;
using Blogge.Interfaces.Repositories;
using Blogge.Models.Configs;
using Blogge.Models.EntityModels;
using Blogge.Models.ViewModels;
using Blogge.Web.Controllers;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using Microsoft.Owin.Security.DataProtection;
using Moq;
using NUnit.Framework;

namespace Blogge.WebTests.Controllers
{
    [TestFixture]
    public class AccountControllerTests
    {
        private Mock<ApplicationUserManager> _userManager;
        private AccountController _accountController;
        private Mock<IImageRepository> _imageRepository;
        private Mock<IIdentityFacade> _identityFacade;
        private Mock<IImageFacade> _imageFacade;
        private Mock<ApplicationSignInManager> _signInManager;
        private Mock<IAuthenticationManager> _authenticationManager;
        private Mock<IUserRepository> _userRepository;
        private Mock<IAccountModelBuilder> _accountBuilder;

        [SetUp]
        public void Setup()
        {
            _identityFacade = new Mock<IIdentityFacade>(MockBehavior.Strict);
            _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            _imageFacade = new Mock<IImageFacade>(MockBehavior.Strict);
            var userStore = new Mock<IUserStore<ApplicationUser>>(MockBehavior.Strict);
            _userManager = new Mock<ApplicationUserManager>(userStore.Object);
            _authenticationManager = new Mock<IAuthenticationManager>(MockBehavior.Strict);
            _signInManager = new Mock<ApplicationSignInManager>(_userManager.Object, _authenticationManager.Object);
            _accountBuilder = new Mock<IAccountModelBuilder>();

            _imageRepository = new Mock<IImageRepository>(MockBehavior.Strict);
            _accountController = new AccountController(
                _userManager.Object, _signInManager.Object, _identityFacade.Object, _userRepository.Object, _accountBuilder.Object);

            _userManager.Setup(x => x.ChangePasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).ReturnsAsync(new ApplicationUser() { Id = "1" });


            _identityFacade.Setup(x => x.GetUserId()).Returns("UserId");
            _imageFacade.Setup(x => x.ImageFromStream(It.IsAny<Stream>()));

        }

        [Test]
        public void When_Login_Returns_LoginView()
        {
            //Given

            //When
            var result = _accountController.Login() as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsNull(result.Model);
        }

        [Test]
        public async Task Given_Valid_LoginViewModel_When_Login_Returns_IndexView()
        {
            //Given
            LoginViewModel model = new LoginViewModel()
            {

            };

            //When
            var result = await _accountController.Login(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());

        }
        [Test]
        public async Task Given_inValid_LoginViewModel_When_Login_Returns_LoginView()
        {
            //Given
            LoginViewModel model = new LoginViewModel()
            {

            };
            _accountController.ModelState.AddModelError(string.Empty, "There is something wrong with model.");
            //When
            var result = await _accountController.Login(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsInstanceOf<LoginViewModel>(result.Model);

        }

        [Test]
        public async Task Given_failure_status_When_Login_Returns_LoginView()
        {
            //Given
            LoginViewModel model = new LoginViewModel()
            {

            };

            _signInManager.Setup(x => x.PasswordSignInAsync(It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<bool>(),
                It.IsAny<bool>()))
                .ReturnsAsync(SignInStatus.Failure);

            //When
            var result = await _accountController.Login(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsInstanceOf<LoginViewModel>(result.Model);

        }

        [Test]
        public void When_Register_Returns_RegisterView()
        {
            //Given
            var model = new RegisterViewModel();
            _accountBuilder.Setup(X => X.BuildRegisterViewModel()).Returns(model);
            //When
            var result = _accountController.Register() as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsInstanceOf<RegisterViewModel>(result.Model);
        }

        [Test]
        public async Task Given_valid_registerViewModel_When_register_Returns_Index()
        {
            //Given
            RegisterViewModel model = new RegisterViewModel()
            {
                Nick = "koks",
                Email = "koks@koks.koks"
            };

            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);

            //When
            var result = await _accountController.Register(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());

        }

        [Test]
        public async Task Given_CreateAsync_failure_When_register_Returns_Index()
        {
            //Given
            RegisterViewModel model = new RegisterViewModel()
            {
                Nick = "koks",
                Email = "koks@koks.koks"
            };

            _userManager.Setup(x => x.CreateAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Failed());

            //When
            var result = await _accountController.Register(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsInstanceOf<RegisterViewModel>(result.Model);
        }

        [Test]
        public void When_ForgotPassword_Returns_ForgotPasswordView()
        {
            //Given

            //When
            var result = _accountController.ForgotPassword() as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsNull(result.Model);
        }

        [Test]
        public async Task Given_valid_ForgotPasswordViewModel_When_ForgotPassword_Returns_ResetPasswordView()
        {
            //Given
            ApplicationUser user = new ApplicationUser()
            {
                Id = "5",
                Email = "kok",
                UserName = "kok",
                SecretAnswer = "kok!",
                SecretQuestion = "kok?"
            };

            var model = new ForgotPasswordViewModel()
            {
                Email = "kok",
                Nickname = "kok",
                SecurityAnswer="kok!",
                 SelectedSecurityQuestion= "kok?"
            };

            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

            //When
            var result = await _accountController.ForgotPassword(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("5", result.RouteValues["userId"].ToString());
            Assert.AreEqual("ResetPassword", result.RouteValues["action"].ToString());

        }

        [Test]
        public async Task Given_inValid_ForgotPasswordViewModel_When_ForgotPassword_Returns_ForgotPasswordView()
        {
            var model = new ForgotPasswordViewModel()
            {
                Email = "kok",
                Nickname = "kok",
                SecurityAnswer = "kok!",
                SelectedSecurityQuestion = "kok?"
            };

            

            _accountController.ModelState.AddModelError(string.Empty, "There is something wrong with model.");

            //When
            var result = await _accountController.ForgotPassword(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOf<ForgotPasswordViewModel>(result.Model);
        }

        [Test]
        public async Task Given_inValid_userInfo_When_ForgotPassword_Returns_ForgotPasswordView()
        {
            var model = new ForgotPasswordViewModel()
            {
                Email = "kok",
                Nickname = "kok",
                SecurityAnswer = "kok",
                SelectedSecurityQuestion = "kok"
            };

            ApplicationUser user = new ApplicationUser()
            {
                Id = "5",
                Email = "kok",
                UserName = "kok",
                SecretAnswer = "kok!",
                SecretQuestion = "kok?"
            };

            _userManager.Setup(x => x.FindByNameAsync(It.IsAny<string>())).ReturnsAsync(user);

            _accountController.ModelState.AddModelError(string.Empty, "There is something wrong with model.");

            //When
            var result = await _accountController.ForgotPassword(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOf<ForgotPasswordViewModel>(result.Model);
        }

        [Test]
        public async Task Given_valid_ResetPasswordViewModel_When_ResetPassword_Returns_ResetPasswordConfirmationView()
        {
            //Given
            ApplicationUser user = new ApplicationUser()
            {
                Id = "5"
            };

            var model = new ResetPasswordViewModel()
            {
                UserId = "5"
            };

            _userRepository.Setup(x => x.GetUser(It.IsAny<string>())).Returns(user);

            DpapiDataProtectionProvider Null = null;
            DataProtectorTokenProvider<ApplicationUser> Null2 = null;

            _identityFacade.Setup(x=>x.GetProvider(It.IsAny<string>())).Returns(Null);
            _identityFacade.Setup(x => x.GetUserTokenProvider(Null, It.IsAny<string>())).Returns(Null2);

            _userManager.Setup(x=>x.GeneratePasswordResetTokenAsync(user.Id)).ReturnsAsync(string.Empty);

            _userManager.Setup(x => x.ResetPasswordAsync(It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>())).
                ReturnsAsync(IdentityResult.Success);

            //When
            var result = await _accountController.ResetPassword(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Account", result.RouteValues["controller"].ToString());
            Assert.AreEqual("ResetPasswordConfirmation", result.RouteValues["action"].ToString());
        }

        [Test]
        public async Task Given_inValid_ResetPasswordViewModel_When_ResetPassword_Returns_ResetPasswordView()
        {
            //Given
            ApplicationUser user = new ApplicationUser()
            {
                Id = "5"
            };

            var model = new ResetPasswordViewModel()
            {
                UserId = "5"
            };

            _accountController.ModelState.AddModelError(string.Empty, "There is something wrong with model.");

            //When
            var result = await _accountController.ResetPassword(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(string.Empty, result.ViewName);
            Assert.IsInstanceOf<ResetPasswordViewModel>(result.Model);
        }

        [Test]
        public async Task Given_null_userId_When_ResetPassword_Returns_Home_Index()
        {
            //Given

            var model = new ResetPasswordViewModel()
            {
            };
            
            //When
            var result = await _accountController.ResetPassword(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
        }

      
        [Test]
        public async Task Given_failure_identityResult_When_ResetPassword_Returns_ResetPasswordConfirmationView()
        {
            //Given
            ApplicationUser user = new ApplicationUser()
            {
                Id = "5"
            };

            var model = new ResetPasswordViewModel()
            {
                UserId = "5"
            };

            _userRepository.Setup(x => x.GetUser(It.IsAny<string>())).Returns(user);

            DpapiDataProtectionProvider Null = null;
            DataProtectorTokenProvider<ApplicationUser> Null2 = null;

            _identityFacade.Setup(x => x.GetProvider(It.IsAny<string>())).Returns(Null);
            _identityFacade.Setup(x => x.GetUserTokenProvider(Null, It.IsAny<string>())).Returns(Null2);

            _userManager.Setup(x => x.GeneratePasswordResetTokenAsync(user.Id)).ReturnsAsync(string.Empty);

            _userManager.Setup(x => x.ResetPasswordAsync(It.IsAny<string>(),
                It.IsAny<string>(),
                It.IsAny<string>())).
                ReturnsAsync(IdentityResult.Failed());

            //When
            var result = await _accountController.ResetPassword(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsInstanceOf<ResetPasswordViewModel>(result.Model);
        }

        [Test]
        public void When_ResetPasswordConfirmation_Returns_ResetPasswordConfirmationView()
        {
            //Given

            //When
            var result = _accountController.ResetPasswordConfirmation() as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsNull(result.Model);
        }

        [Test]
        public void When_SignOut_Returns_Index()
        {
            //Given
            _authenticationManager.Setup(x => x.SignOut(It.IsAny<string>()));
            _identityFacade.Setup(x => x.GetAuthenticationManager()).Returns(_authenticationManager.Object);
            //When
            var result = _accountController.LogOff() as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
        }
    }
}