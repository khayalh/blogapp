//using Blogge.Core.Repositories;
//using Blogge.Interfaces.Facades.DB;
//using Blogge.Models.DB;
//using Blogge.Models.EntityModels;
//using Moq;
//using NUnit.Framework;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.AspNet.Identity.EntityFramework;

//namespace Blogge.Core.Repositories.Tests
//{
//    [TestFixture]
//    public class UserRepositoryTests
//    {
//        private Mock<IDBContextFacade> _dBContext;
//        private Mock<ApplicationDbContext> _db;
//        private Mock<DbSet<ApplicationUser>> _dbsetMockUser;
//        private Mock<DbSet<IdentityUserRole>> _dbsetMockUserRole;
//        private Mock<DbSet<IdentityRole>> _dbsetMockRole;

//        private List<ApplicationUser> userList;
//        private ApplicationUser User1;
//        private ApplicationUser User2;

//        private List<IdentityRole> roleList;
//        private IdentityRole role1;
//        private IdentityRole role2;

//        private List<IdentityUserRole> userRoleList;
//        private IdentityUserRole userRole1;
//        private IdentityUserRole userRole2;

//        private UserRepository userRepository;
//        [SetUp]
//        public void Setup()
//        {
//            _dBContext = new Mock<IDBContextFacade>(MockBehavior.Strict);
//            _db = new Mock<ApplicationDbContext>(MockBehavior.Strict);
//            _dbsetMockUser = new Mock<DbSet<ApplicationUser>>(MockBehavior.Strict);
//            _dbsetMockRole = new Mock<DbSet<IdentityRole>>(MockBehavior.Strict);
//            _dbsetMockUserRole = new Mock<DbSet<IdentityUserRole>>(MockBehavior.Strict);

//            userList = new List<ApplicationUser>();
//            User1 = new ApplicationUser()
//            {
//                Id = "randomId",
//                UserPosts = new List<Post>()
//            };
//            User1 = new ApplicationUser()
//            {
//                Id = "randomId",
//                UserPosts = new List<Post>()
//            };
//            userList.Add(User1);
//            userList.Add(User2);

//            roleList = new List<IdentityRole>();
//            role1 = new IdentityRole();
//            role2 = new IdentityRole();
//            roleList.Add(role1);
//            roleList.Add(role2);

//            userRoleList = new List<IdentityUserRole>();
//            userRole1 = new IdentityUserRole();
//            userRole2 = new IdentityUserRole();
//            userRoleList.Add(userRole1);
//            userRoleList.Add(userRole2);

//            _dbsetMockUser.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(userList.AsQueryable().Provider);
//            _dbsetMockUser.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(userList.AsQueryable().Expression);
//            _dbsetMockUser.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(userList.AsQueryable().ElementType);
//            _dbsetMockUser.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(userList.AsQueryable().GetEnumerator());

//            _dbsetMockRole.As<IQueryable<IdentityRole>>().Setup(m => m.Provider).Returns(roleList.AsQueryable().Provider);
//            _dbsetMockRole.As<IQueryable<IdentityRole>>().Setup(m => m.Expression).Returns(roleList.AsQueryable().Expression);
//            _dbsetMockRole.As<IQueryable<IdentityRole>>().Setup(m => m.ElementType).Returns(roleList.AsQueryable().ElementType);
//            _dbsetMockRole.As<IQueryable<IdentityRole>>().Setup(m => m.GetEnumerator()).Returns(roleList.AsQueryable().GetEnumerator());

//            _dbsetMockUserRole.As<IQueryable<IdentityUserRole>>().Setup(m => m.Provider).Returns(userRoleList.AsQueryable().Provider);
//            _dbsetMockUserRole.As<IQueryable<IdentityUserRole>>().Setup(m => m.Expression).Returns(userRoleList.AsQueryable().Expression);
//            _dbsetMockUserRole.As<IQueryable<IdentityUserRole>>().Setup(m => m.ElementType).Returns(userRoleList.AsQueryable().ElementType);
//            _dbsetMockUserRole.As<IQueryable<IdentityUserRole>>().Setup(m => m.GetEnumerator()).Returns(userRoleList.AsQueryable().GetEnumerator());

//            _dbsetMockUser.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => userList.AsQueryable().FirstOrDefault(d => d.Id == (string)ids[0]));
//            _dbsetMockUserRole.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => userRoleList.AsQueryable().FirstOrDefault(d => d.RoleId == (string)ids[0]));
//            _dbsetMockRole.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => roleList.AsQueryable().FirstOrDefault(d => d.Id == (string)ids[0]));

//            _db.Setup(x => x.Users).Returns(_dbsetMockUser.Object);
//            _db.Setup(x => x.UserRoles.ToList()).Returns(_dbsetMockUserRole.Object.ToList());
//            _db.Setup(x => x.Roles).Returns(_dbsetMockRole.Object);

//            _dBContext.Setup(x => x.GetDBContext()).Returns(_db.Object);

//            userRepository = new UserRepository(_dBContext.Object);
//        }
//        [Test]
//        public void When_GetUsers_Then_returns_userList()
//        {
//            //given
//            //when
//            var result = userRepository.GetUsers();
//            //then
//            Assert.NotNull(result);
//            Assert.AreEqual(2,result.Count);
//            Assert.AreEqual(true, result.Contains(User1));
//            Assert.AreEqual(true, result.Contains(User2));
//        }
//}
//    }