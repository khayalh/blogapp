using Blogge.Core.Validators;
using Blogge.Interfaces.Repositories;
using Blogge.Models.EntityModels;
using Moq;
using NUnit.Framework;

namespace Blogge.CoreTests.Validators
{
    [TestFixture()]
    public class DisplayValidatorTests
    {
        private Mock<IBlogRepository> _postRepository;
        private DisplayValidator _displayValidator;
        private Post _post1;
        private Comment _comment1;

        [SetUp]
        public void Setup()
        {
            _postRepository = new Mock<IBlogRepository>();
            _displayValidator = new DisplayValidator(_postRepository.Object);

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
                AuthorId = "1",
            };

            _postRepository.Setup(x => x.GetPost(It.IsAny<int>())).Returns(_post1);
            _postRepository.Setup(x => x.GetComment(It.IsAny<int>())).Returns(_comment1);

        }

        [Test]
        public void Given_live_commentID_When_IsAvailableComment_returns_true()
        {
            //Given
            var commentId = 1;
            //When
            var result = _displayValidator.IsAvailableComment(commentId);
            //Then
            Assert.IsTrue(result);
        }
        [Test]
        public void Given_blocked_commentID_When_IsAvailableComment_returns_false()
        {
            //Given
            var commentId = 1;
            _comment1.Blocked = true;
            //When
            var result = _displayValidator.IsAvailableComment(commentId);
            //Then
            Assert.IsFalse(result);
        }
        [Test]
        public void Given_deleted_commentID_When_IsAvailableComment_returns_false()
        {
            //Given
            var commentId = 1;
            _comment1.IsDeleted = true;
            //When
            var result = _displayValidator.IsAvailableComment(commentId);
            //Then
            Assert.IsFalse(result);
        }
        [Test]
        public void Given_deletedAndBlocked_commentID_When_IsAvailableComment_returns_false()
        {
            //Given
            var commentId = 1;
            _comment1.IsDeleted = true;
            _comment1.Blocked = true;
            //When
            var result = _displayValidator.IsAvailableComment(commentId);
            //Then
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_live_comment_When_IsAvailableComment_returns_true()
        {
            //Given

            //When
            var result = _displayValidator.IsAvailableComment(_comment1);
            //Then
            Assert.IsTrue(result);
        }
        [Test]
        public void Given_blocked_comment_When_IsAvailableComment_returns_false()
        {
            //Given

            _comment1.Blocked = true;
            //When
            var result = _displayValidator.IsAvailableComment(_comment1);
            //Then
            Assert.IsFalse(result);
        }
        [Test]
        public void Given_deleted_comment_When_IsAvailableComment_returns_false()
        {
            //Given
            _comment1.IsDeleted = true;
            //When
            var result = _displayValidator.IsAvailableComment(_comment1);
            //Then
            Assert.IsFalse(result);
        }
        [Test]
        public void Given_deletedAndBlocked_comment_When_IsAvailableComment_returns_false()
        {
            //Given
            _comment1.IsDeleted = true;
            _comment1.Blocked = true;
            //When
            var result = _displayValidator.IsAvailableComment(_comment1);
            //Then
            Assert.IsFalse(result);
        }


        [Test]
        public void Given_live_postId_When_IsAvailablePost_returns_true()
        {
            //Given
            var postId = 1;
            //When
            var result = _displayValidator.IsAvailablePost(postId);
            //Then
            Assert.IsTrue(result);
        }
        [Test]
        public void Given_blocked_postId_When_IsAvailablePost_returns_false()
        {
            //Given
            var postId = 1;
            _post1.Blocked = true;
            //When
            var result = _displayValidator.IsAvailablePost(postId);
            //Then
            Assert.IsFalse(result);
        }
        [Test]
        public void Given_deleted_postId_When_IsAvailablePost_returns_false()
        {
            //Given
            var postId = 1;
            _post1.IsDeleted = true;
            //When
            var result = _displayValidator.IsAvailablePost(postId);
            //Then
            Assert.IsFalse(result);
        }
        [Test]
        public void Given_blockedAndDeleted_postId_When_IsAvailablePost_returns_false()
        {
            //Given
            _post1.Blocked = true;
            _post1.IsDeleted = true;
            //When
            var result = _displayValidator.IsAvailablePost(_post1);
            //Then
            Assert.IsFalse(result);
        }

        [Test]
        public void Given_live_post_When_IsAvailablePost_returns_true()
        {
            //Given

            //When
            var result = _displayValidator.IsAvailablePost(_post1);
            //Then
            Assert.IsTrue(result);
        }
        [Test]
        public void Given_blocked_post_When_IsAvailablePost_returns_false()
        {
            //Given
            _post1.Blocked = true;
            //When
            var result = _displayValidator.IsAvailablePost(_post1);
            //Then
            Assert.IsFalse(result);
        }
        [Test]
        public void Given_deleted_post_When_IsAvailablePost_returns_false()
        {
            //Given
            _post1.IsDeleted = true;
            //When
            var result = _displayValidator.IsAvailablePost(_post1);
            //Then
            Assert.IsFalse(result);
        }
        [Test]
        public void Given_blockedAndDeleted_post_When_IsAvailablePost_returns_false()
        {
            //Given
            _post1.Blocked = true;
            _post1.IsDeleted = true;
            //When
            var result = _displayValidator.IsAvailablePost(_post1);
            //Then
            Assert.IsFalse(result);
        }
    }
}