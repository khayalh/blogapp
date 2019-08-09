using System;
using System.Threading.Tasks;
using System.Web.Mvc;
using Blogge.Interfaces.Builders.Administration;
using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Repositories;
using Blogge.Models.EntityModels;
using Blogge.Models.ViewModels;
using Blogge.Web.Controllers;
using Microsoft.AspNet.Identity;
using Moq;
using NUnit.Framework;

namespace Blogge.WebTests.Controllers
{
    [TestFixture]
    public class AdminControllerTests
    {

        private Mock<IAdminModelBuilder> _adminModelBuilder;
        private Mock<IUserRepository> _userRepository;
        private Mock<IBlogRepository> _postRepository;
        private Mock<IIdentityFacade> _identityFacade;
        private Mock<IUserManagerFacade> _userManager;
        private AdminController _adminController;
        private ApplicationUser user;


        [SetUp]
        public void Setup()
        {
            _adminModelBuilder = new Mock<IAdminModelBuilder>(MockBehavior.Strict);
            _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            _postRepository = new Mock<IBlogRepository>(MockBehavior.Strict);
            _identityFacade = new Mock<IIdentityFacade>(MockBehavior.Strict);
            _userManager = new Mock<IUserManagerFacade>(MockBehavior.Strict);

            _adminController = new AdminController(
                _adminModelBuilder.Object,
                _userRepository.Object,
                _postRepository.Object,
                _userManager.Object);

            user = new ApplicationUser()
            {
                Id = "1"
            };

            _postRepository.Setup(x => x.BlockComment(It.IsAny<int>()));
            _postRepository.Setup(x => x.DeleteReport(It.IsAny<int>()));
        }

        [Test]
        public void When_Index_Returns_AllUserRoleViewModel()
        {
            //Given
            var model = new AllUserRoleViewModel();
            _adminModelBuilder.Setup(x => x.BuildUserRoleViewModel()).Returns(model);
            //When
            var result = _adminController.ManageUserRoles() as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsInstanceOf<AllUserRoleViewModel>(result.Model);
        }

        [Test]
        public void When_ManageReports_Returns_ReportManagerViewModel()
        {
            //Given
            var model = new ReportManagerViewModel();
            _adminModelBuilder.Setup(x => x.BuildReportManagerViewModel()).Returns(model);
            //When
            var result = _adminController.ManageReports() as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.IsInstanceOf<ReportManagerViewModel>(result.Model);
        }

        [Test]
        public async Task Given_UserRoleViewModel_When_ChangeRole_Returns_ReportManagerViewModel()
        {
            //Given
            var model = new UserRoleViewModel();

            _userManager.Setup(x => x.FindByIdAsync(It.IsAny<string>())).Returns(Task.FromResult(user));
            _userManager.Setup(x => x.GetRole(It.IsAny<string>())).Returns("user");
            _userManager.Setup(x => x.RemoveFromRoleAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.AddToRoleAsync(It.IsAny<string>(), It.IsAny<string>())).ReturnsAsync(IdentityResult.Success);
            _userManager.Setup(x => x.UpdateAsync(It.IsAny<ApplicationUser>())).ReturnsAsync(IdentityResult.Success);

            //When
            var result = await _adminController.ChangeRole(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("ManageUserRoles", result.RouteValues["action"].ToString());
        }
    

        [Test]
        public void Given_commentId_When_BlockComment_Returns_ManageReports()
        {
            //Given
            int commentId = 1;

            //When
            var result = _adminController.BlockComment(commentId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("ManageReports", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_reportId_When_DeleteReport_Returns_ManageReports()
        {
            //Given
            int reportId = 1;

            //When
            var result = _adminController.DeleteReport(reportId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("ManageReports", result.RouteValues["action"].ToString());
        }

    }
}
