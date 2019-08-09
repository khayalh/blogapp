
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Web.Mvc;
using Blogge.Models.EntityModels;

namespace Blogge.Models.ViewModels
{
    [ExcludeFromCodeCoverage]
    public class UserRoleViewModel
    {
        public string UserId { get; set; }
        public string Username { get; set; }
        public string Role { get; set; }

        public SelectList AvailableRoles { set; get; }
        public string SelectedRole { set; get; }
    }

    [ExcludeFromCodeCoverage]
    public class AllUserRoleViewModel
    {
        public List<UserRoleViewModel> AllUsers { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class ReportManagerViewModel
    {
        public List<SingleReportViewModel> AllReports { get; set; }
    }

    [ExcludeFromCodeCoverage]
    public class SingleReportViewModel
    {
        public int Id { get; set; }
        public Comment Comment { get; set; }
        public string ReportText { get; set; }
        public string Status { get; set; }
        public string LifeStatus { get; set; }
        public string SenderName { get; set; }
    }
}
