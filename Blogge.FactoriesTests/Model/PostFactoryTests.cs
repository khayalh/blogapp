using Blogge.Factories.Model;
using Blogge.Interfaces.Converters;
using Blogge.Interfaces.Facades.Identity;
using Blogge.Models.EntityModels;
using Blogge.Models.Enums;
using Blogge.Models.ViewModels;
using Moq;
using NUnit.Framework;

namespace Blogge.FactoriesTests.Model
{
    [TestFixture()]
    public class PostFactoryTests
    {
        private Mock<IIdentityFacade> _identityFacade;
        private Mock<IDataConverter> _dataConverter;
        private PostFactory _postFactory;

        [SetUp]
        public void Setup()
        {
            _identityFacade = new Mock<IIdentityFacade>();
            _dataConverter = new Mock<IDataConverter>();
            _postFactory = new PostFactory(_identityFacade.Object, _dataConverter.Object);


            _identityFacade.Setup(x => x.GetUserName()).Returns("Shrook");
            _identityFacade.Setup(x => x.GetUserId()).Returns("22");
        }

        [Test]
        public void Given_CreatePostViewModel_without_file_When_CreatePost_Returns_Post()
        {
            //Given
            var model = new CreatePostViewModel()
            {
                Title = "koks",
                Content = "Tas ir koks",
                SubTitle = "Koki ir visur",
            };
            //When
            var result = _postFactory.CreatePost(model);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<Post>(result);
            Assert.AreEqual("Shrook", result.Author);
            Assert.AreEqual("22", result.AuthorId);
            Assert.AreEqual(model.Title, result.Title);
            Assert.AreEqual(model.SubTitle, result.SubTitle);
            Assert.AreEqual(model.Content, result.Content);
        }

        [Test]
        public void Given_postId_userId_rank_When_CreateLike_Returns_Like()
        {
            //Given
            var postId = 1;
            var userId = "2";
            var rank = RatingType.Positive;

        //When
        var result = _postFactory.CreateLike(postId, userId, rank);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<Like>(result);
            Assert.AreEqual(postId, result.PostId);
            Assert.AreEqual(rank, result.RatingType);
            Assert.AreEqual(userId, result.UserID);
        }

        [Test]
        public void Given_CreateCommentViewModel_When_CreateComment_Returns_Comment()
        {
            //Given
            var model = new AddCommentViewModel()
            {
                Content = "Sobaka",
                PostId = 1
            };

            //When
            var result = _postFactory.CreateComment(model);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<Comment>(result);
            Assert.AreEqual("Shrook", result.Author);
            Assert.AreEqual("22", result.AuthorId);
            Assert.AreEqual(model.Content, result.Content);
        }

        [Test]
        public void Given_CreateReportViewModel_comment_When_CreateReport_Returns_Report()
        {
            //Given
            var model = new ReportCommentViewModel()
            {
                Content = "Sobaka",
                CommentId = 1,
                CallbackPostId = 1
            };

            var comment = new Comment()
            {
                Author = "Shrook",
                AuthorId = "22",
                Content ="sosiska"
            };

            //When
            var result = _postFactory.CreateReport(model, comment);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<Report>(result);
            Assert.AreEqual("Shrook", result.SenderName);
            Assert.AreEqual(comment, result.Comment);
            Assert.AreEqual(model.Content, result.ReportText);
        }
    }
}