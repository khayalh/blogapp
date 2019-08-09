using Blogge.Core.Validators;
using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Repositories;
using Blogge.Models.EntityModels;
using Moq;
using NUnit.Framework;

namespace Blogge.CoreTests.Validators
{
    [TestFixture]
    public class AccessValidatorTests
    {
        private  Mock<IIdentityFacade> _identityFacade;
        private  Mock<IBlogRepository> _postRepository;
        private AccessValidator _accessValidator;
        private ApplicationUser _user1;
        private Post _post1;
        private Comment _comment1;

        [SetUp]
        public void Setup()
        {
            _identityFacade = new Mock<IIdentityFacade>();
            _postRepository = new Mock<IBlogRepository>();
            _accessValidator = new AccessValidator(_identityFacade.Object, _postRepository.Object);

            _user1 = new ApplicationUser()
            {
                Id = "1",
                UserName = "user1"
            };
            _post1 = new Post()
            {
                Id = 1,
                Author = "Shrek",
                AuthorId = "1",
                Content = "lol",
                SubTitle = "ultralol"
            };
            _comment1 = new Comment()
            {
                Id = 1,
                AuthorId = "1"
            };

            _postRepository.Setup(x=>x.GetPost(It.IsAny<int>())).Returns(_post1);
            _postRepository.Setup(x => x.GetComment(It.IsAny<int>())).Returns(_comment1);
            _identityFacade.Setup(x => x.GetUserId()).Returns(_user1.Id);
            //getcomment
            //getuserid
        }

        [Test]
        public void Given_PostId_When_owner_tries_CanAccessPost_Returns_true()
        {
            //given
            var postId = 1;
            //when
            var result = _accessValidator.CanAccessPost(postId);
            //then
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_PostId_When_administration_tries_CanAccessPost_Returns_true()
        {
            //given
            var postId = 1;
            _identityFacade.Setup(x => x.GetUserId()).Returns("notOwner");
            _identityFacade.Setup(x=>x.CheckRole(It.IsAny<string>())).Returns(true);
            //when
            var result = _accessValidator.CanAccessPost(postId);
            //then
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_PostId_When_unprivileged_user_tries_CanAccessPost_Returns_false()
        {
            //given
            var postId = 1;
            _identityFacade.Setup(x => x.GetUserId()).Returns("notOwner");
            _identityFacade.Setup(x => x.CheckRole(It.IsAny<string>())).Returns(false);
            //when
            var result = _accessValidator.CanAccessPost(postId);
            //then
            Assert.IsFalse(result);
        }
        [Test]
        public void Given_commentId_When_owner_tries_CanAccessComment_Returns_true()
        {
            //given
            var commentId = 1;
            //when
            var result = _accessValidator.CanAccessComment(commentId);
            //then
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_commentId_When_administration_tries_CanAccessComment_Returns_true()
        {
            //given
            var commentId = 1;
            _identityFacade.Setup(x => x.GetUserId()).Returns("notOwner");
            _identityFacade.Setup(x => x.CheckRole(It.IsAny<string>())).Returns(true);
            //when
            var result = _accessValidator.CanAccessComment(commentId);
            //then
            Assert.IsTrue(result);
        }

        [Test]
        public void Given_commentId_When_unprivileged_user_tries_CanAccesscomment_Returns_false()
        {
            //given
            var commentId = 1;
            _identityFacade.Setup(x => x.GetUserId()).Returns("notOwner");
            _identityFacade.Setup(x => x.CheckRole(It.IsAny<string>())).Returns(false);
            //when
            var result = _accessValidator.CanAccessComment(commentId);
            //then
            Assert.IsFalse(result);
        }
    }
}