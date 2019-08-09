using Blogge.Builders.Administration;
using Blogge.Interfaces.Repositories;
using Blogge.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using Moq;
using NUnit.Framework;
using System.Collections.Generic;
using System.Web.Mvc;
using Blogge.Models.EntityModels;

namespace Blogge.BuildersTests.Administration
{
    class AdminModelBuilderTests
    {
        private Mock<IBlogRepository> _postRepository;
        private Mock<IUserRepository> _userRepository;
        private AdminModelBuilder _adminBuilder;

        private ApplicationUser _user1;
        private ApplicationUser _user2;
        private ApplicationUser _user3;
        private List<ApplicationUser> _userList;

        private IdentityRole _role1;
        private IdentityRole _role2;
        private List<IdentityRole> _roleList;

        private IdentityUserRole _userRole1;
        private IdentityUserRole _userRole2;
        private IdentityUserRole _userRole3;
        private List<IdentityUserRole> _userRoleList;

        private Report _report1;
        private Report _report2;
        private List<Report> _reportList;

        [SetUp]
        public void Setup()
        {
            _postRepository = new Mock<IBlogRepository>(MockBehavior.Strict);
            _userRepository = new Mock<IUserRepository>(MockBehavior.Strict);
            _adminBuilder = new AdminModelBuilder(_userRepository.Object, _postRepository.Object);

            _user1 = new ApplicationUser()
            {
                Id = "1",
                UserName = "user1"
            };
            _user2 = new ApplicationUser()
            {
                Id = "2",
                UserName = "user2"
            };
            _user3 = new ApplicationUser()
            {
                Id = "3",
                UserName = "user1"
            };
            _userList = new List<ApplicationUser>();
            _userList.Add(_user1);
            _userList.Add(_user2);
            _userList.Add(_user3);

            _role1 = new IdentityRole()
            {
                Id = "1",
                Name = "user"
            };
            _role2 = new IdentityRole()
            {
                Id = "1",
                Name = "admin"
            };
            _roleList = new List<IdentityRole>();
            _roleList.Add(_role1);
            _roleList.Add(_role2);

            _userRole1 = new IdentityUserRole()
            {
                UserId = "1",
                RoleId = "1"
            };
            _userRole2 = new IdentityUserRole()
            {
                UserId = "2",
                RoleId = "1"
            };
            _userRole3 = new IdentityUserRole()
            {
                UserId = "3",
                RoleId = "2"
            };
            _userRoleList = new List<IdentityUserRole>();
            _userRoleList.Add(_userRole1);
            _userRoleList.Add(_userRole2);
            _userRoleList.Add(_userRole3);

            _report1 = new Report()
            {
                Id = 1,
                Comment = new Comment(),
                CommentId = 1,
                SenderName = "shrook",
                ReportText = "sie"
            };
            _report2 = new Report()
            {
                Id = 2,
                Comment = new Comment(),
                CommentId = 2,
                SenderName = "shrookster",
                ReportText = "siege"
            };
            _reportList = new List<Report>();
            _reportList.Add(_report1);
            _reportList.Add(_report2);

            _userRepository.Setup(x => x.GetUsers()).Returns(_userList);
            _userRepository.Setup(x => x.GetRoles()).Returns(_roleList);
            _userRepository.Setup(x => x.GetUserRoles()).Returns(_userRoleList);
            _userRepository.Setup(x => x.GetUserRole(_user1.Id)).Returns(_role1);
            _userRepository.Setup(x => x.GetUserRole(_user2.Id)).Returns(_role1);
            _userRepository.Setup(x => x.GetUserRole(_user3.Id)).Returns(_role2);

            _postRepository.Setup(x=>x.GetReports()).Returns(_reportList);
        }

        [Test]
        public void When_BuildUserRoleViewModel_Returns_AllUserRoleViewModel()
        {
            //Given
            //When
            var result = _adminBuilder.BuildUserRoleViewModel();
            //Then
            Assert.NotNull(result);
            Assert.AreEqual (3 ,result.AllUsers.Count);
        }

        [Test]
        public void Given_role_When_BuildAvailableRolesList_Returns_SelectList()
        {
            //Given
            //When
            var result = _adminBuilder.BuildAvailableRolesList(_role1);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<SelectList>(result);
            Assert.AreEqual(_role1.Name,result.SelectedValue);
        }

        [Test]
        public void When_BuildReportManagerViewModel_Returns_ReportManagerViewModel()
        {
            //Given
            //When
            var result = _adminBuilder.BuildReportManagerViewModel();
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<ReportManagerViewModel>(result);
            Assert.AreEqual(2, result.AllReports.Count);
        }
    }
}
