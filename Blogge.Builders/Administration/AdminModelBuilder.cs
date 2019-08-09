using Blogge.Interfaces.Builders.Administration;
using Blogge.Interfaces.Repositories;
using Blogge.Models.ViewModels;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Blogge.Builders.Administration
{
    public class AdminModelBuilder : IAdminModelBuilder
    {
        private readonly IUserRepository _userRepository;
        private readonly IBlogRepository _postRepository;

        public AdminModelBuilder(IUserRepository userRepository, IBlogRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        public AllUserRoleViewModel BuildUserRoleViewModel()
        {
            AllUserRoleViewModel allUsersModel = new AllUserRoleViewModel()
            {
                AllUsers = new List<UserRoleViewModel>(),
            };

            var users = _userRepository.GetUsers();
            var userRoles = _userRepository.GetUserRoles();
            var allRoles = _userRepository.GetRoles();

            foreach (var user in users)
            {
                var currentRole = _userRepository.GetUserRole(user.Id);

                var UserRoleModel = new UserRoleViewModel()
                {
                    UserId = user.Id,
                    Role = allRoles.Find(x => x.Id == currentRole.Id).Name,
                    Username = user.UserName,
                    AvailableRoles = BuildAvailableRolesList(currentRole)
                };

                allUsersModel.AllUsers.Add(UserRoleModel);
            }
            return allUsersModel;
        }

        public SelectList BuildAvailableRolesList(IdentityRole Role)
        {

            var allRoles = _userRepository.GetRoles();
            var availableUserRoles = new SelectList(allRoles, "Name", "Name", Role.Name);

            return availableUserRoles;
        }

        public ReportManagerViewModel BuildReportManagerViewModel()
        {
            var model = new ReportManagerViewModel()
            {
                AllReports = new List<SingleReportViewModel>()
            };

            var reports = _postRepository.GetReports().Where(x=>x.IsDeleted == false);

            foreach (var report in reports)
            {
                var singleReport = new SingleReportViewModel()
                {
                    Comment = report.Comment,
                    ReportText = report.ReportText,
                    Id = report.Id,
                    SenderName = report.SenderName,
                    Status = (report.Comment.Blocked) ? "Blocked" : "Not Blocked",
                    LifeStatus = (report.Comment.IsDeleted)? "Deleted" : "Not Deleted",
                };
                model.AllReports.Add(singleReport);
            };

            return model;
        }
    }
}
