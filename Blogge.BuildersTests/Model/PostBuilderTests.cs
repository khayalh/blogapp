using System.Collections.Generic;
using Blogge.Builders.Model;
using Blogge.Interfaces.Converters;
using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Repositories;
using Blogge.Interfaces.Validators;
using Blogge.Models.EntityModels;
using Blogge.Models.ViewModels;
using Moq;
using NUnit.Framework;

namespace Blogge.BuildersTests.Model
{
    [TestFixture()]
    public class PostBuilderTests
    {

        private  Mock<IBlogRepository> _postRepository;
        private  Mock<IIdentityFacade> _identityFacade;
        private  Mock<IDisplayValidator> _displayValidator;
        private Mock<IImageRepository> _imageRepository;
        private Mock<IDataConverter> _dataConverter;
        private PostBuilder _postBuilder;
        private Post _post1;
        private Post _post2;
        private List<Post> _postList;
        private Comment _comment1;
        private Comment _comment2;
        private List<Comment> _commentList;

        [SetUp]
        public void Setup()
        {
            _postRepository = new Mock<IBlogRepository>(MockBehavior.Strict);
            _identityFacade = new Mock<IIdentityFacade>(MockBehavior.Strict);
            _displayValidator = new Mock<IDisplayValidator>(MockBehavior.Strict);
            _imageRepository = new Mock<IImageRepository>(MockBehavior.Strict);
            _dataConverter = new Mock<IDataConverter>(MockBehavior.Strict);

            _postBuilder = new PostBuilder(_postRepository.Object,
                _identityFacade.Object,
                _displayValidator.Object,
                _imageRepository.Object,
                _dataConverter.Object);

            _imageRepository.Setup(X=>X.GetImageInString(It.IsAny<string>())).Returns("koks");

            _dataConverter.Setup(x=>x.ConvertToString(It.IsAny<byte[]>())).Returns("KUK");

            _displayValidator.Setup(x=>x.IsAvailableComment(It.IsAny<Comment>())).Returns(true);

            _comment1 = new Comment()
            {
                Id = 1
            };
            _comment2 = new Comment()
            {
                Id = 2
            };
            _commentList = new List<Comment>();
            _commentList.Add(_comment1);
            _commentList.Add(_comment2);

            _post1 = new Post()
            {
                Id = 1,
                Author = "Shrek",
                AuthorId = "1",
                Comments = _commentList,
                Content = "lol",
                SubTitle = "ultralol"
            };
            _post2 = new Post()
            {
                Id = 2,
                Author = "Shrook",
                AuthorId = "2",
                Comments = _commentList,
                Content = "kek",
                SubTitle = "omegakek"
            };
            _postList = new List<Post>();
            _postList.Add(_post1);
            _postList.Add(_post2);


            _comment1 = new Comment()
            {
                Id = 1,
                Author = "Mushroom",
                AuthorId = "22",
                PostId = 1,
                Content ="Alere",
                Post = _post1
            };
            _comment2 = new Comment()
            {
                Id = 2,
                Author = "Allah",
                AuthorId = "31",
                PostId =2,
                Content = "Islamia",
                Post = _post2
            };
            _commentList = new List<Comment>();
            _commentList.Add(_comment1);
            _commentList.Add(_comment2);



            _postRepository.Setup(x => x.GetPost(It.IsAny<int>())).Returns(_post1);
            _postRepository.Setup(x => x.GetPosts()).Returns(_postList);

            _postRepository.Setup(x => x.GetComment(It.IsAny<int>())).Returns(_comment1);
            _postRepository.Setup(x => x.GetComments()).Returns(_commentList);

            _postRepository.Setup(x => x.SearchPosts(It.IsAny<string>())).Returns(_postList);
        }

        [Test]
        public void Given_postId_When_BuildBigPostViewModel_Returns_BigPostViewModel()
        {
            //Given
            var postId = 1;
            //When
            var result = _postBuilder.BuildBigPostViewModel(postId);
            //Then
            Assert.NotNull(result);
            Assert.NotNull(result.AddComment);
            Assert.NotNull(result.Comments);
            Assert.NotNull(result.PostModel);
            Assert.NotNull(result.SideBar);
            Assert.IsInstanceOf<BigPostViewModel>(result);
        }

        [Test]
        public void When_BuildHomepageViewModel_Returns_HomepageViewModel()
        {
            //Given
            //When
            var result = _postBuilder.BuildHomepageViewModel();
            //Then
            Assert.NotNull(result);
            Assert.NotNull(result.LastPosts);
            Assert.NotNull(result.PostCount);
            Assert.AreEqual(2,result.PostCount);
            Assert.NotNull(result.SideBar);
            Assert.IsInstanceOf<HomepageViewModel>(result);
        }

        [TestCase(2)]
        [TestCase(3)]
        [TestCase(1)]
        [TestCase(10)]
        public void Given_displayCount_When_BuildHomepageViewModel_Returns_HomepageViewModel(int displayCount)
        {
            //Given
            int expectedCount = (displayCount < 2) ? displayCount : 2;
            //When
            var result = _postBuilder.BuildHomepageViewModel(displayCount);
            //Then
            Assert.NotNull(result);
            Assert.NotNull(result.LastPosts);
            Assert.NotNull(result.PostCount);
            Assert.AreEqual(expectedCount, result.PostCount);
            Assert.NotNull(result.SideBar);
            Assert.IsInstanceOf<HomepageViewModel>(result);
        }

        [Test]
        public void Given_searchKey_When_BuildHomepageViewModel_Returns_HomepageViewModel()
        {
            //Given
            string searchKey = "URJUK";
            //When
            var result = _postBuilder.BuildHomepageViewModel(searchKey);
            //Then
            Assert.NotNull(result);
            Assert.NotNull(result.LastPosts);
            Assert.NotNull(result.PostCount);
            Assert.NotNull(result.SideBar);
            Assert.IsInstanceOf<HomepageViewModel>(result);
        }

        [Test]
        public void Given_postId_When_BuildAllCommentsViewModel_Returns_AllCommentsViewModel()
        {
            //Given
            int postId = 1;
            //When
            var result = _postBuilder.BuildAllCommentsViewModel(postId);
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(2, result.CommentList.Count);
            Assert.IsInstanceOf<AllCommentsViewModel>(result);
        }

        [Test]
        public void Given_postId_When_BuildAllCommentsViewModel_and_blockedORdeleted_comment_Returns_AllCommentsViewModel()
        {
            //Given
            int postId = 1;
            _displayValidator.Setup(x => x.IsAvailableComment(It.IsAny<Comment>())).Returns(false);
            //When
            var result = _postBuilder.BuildAllCommentsViewModel(postId);
            //Then
            Assert.NotNull(result);
            Assert.AreEqual(0, result.CommentList.Count);
            Assert.IsInstanceOf<AllCommentsViewModel>(result);
        }

        [Test]
        public void Given_commentId_When_BuildCommentViewModel_Returns_CommensViewModel()
        {
            //Given
            int commentId = 1;
            //When
            var result = _postBuilder.BuildCommentViewModel(commentId);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<CommentViewModel>(result);
            Assert.AreEqual(_comment1.Author, result.Author);
            Assert.AreEqual(_comment1.Content, result.Content);
            Assert.AreEqual(_comment1.Id, result.Id);
            Assert.AreEqual(_comment1.AuthorId, result.AuthorId);
        }

        [Test]
        public void Given_postId_When_BuildAddCommentViewModel_Returns_AddCommentViewModel()
        {
            //Given
            int postId = 1;
            //When
            var result = _postBuilder.BuildAddCommentsViewModel(postId);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<AddCommentViewModel>(result);
            Assert.AreEqual(postId, result.PostId);
        }

        [Test]
        public void Given_postId_without_image_When_BuildPostViewModel_Returns_SinglePostViewModel()
        {
            //Given
            int postId = 1;
            //When
            var result = _postBuilder.BuildPostViewModel(postId);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf< SinglePostViewModel>(result);
            Assert.AreEqual(string.Empty, result.PostImage);
            Assert.AreEqual(_post1.Author, result.Author);
            Assert.AreEqual(_post1.Id, result.Id);
            Assert.AreEqual(_post1.SubTitle, result.SubTitle);
            Assert.AreEqual(_post1.Title, result.Title);
        }

        [Test]
        public void Given_postId_with_image_When_BuildPostViewModel_Returns_SinglePostViewModel()
        {
            //Given
            int postId = 1;
            _post1.postPicture = new PostPicture();
            //When
            var result = _postBuilder.BuildPostViewModel(postId);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<SinglePostViewModel>(result);
            Assert.AreEqual("KUK", result.PostImage);
            Assert.AreEqual(_post1.Author, result.Author);
            Assert.AreEqual(_post1.Id, result.Id);
            Assert.AreEqual(_post1.SubTitle, result.SubTitle);
            Assert.AreEqual(_post1.Title, result.Title);
        }

        [Test]
        public void When_BuildSidebarContentViewModel_Returns_SidebarContentViewModel()
        {
            //Given
            //When
            var result = _postBuilder.BuildSidebarContentViewModel();
            //Then
            Assert.NotNull(result);
            Assert.NotNull(result.LastCommentedPosts);
            Assert.NotNull(result.LastPosts);
            Assert.NotNull(result.TrendingPosts);
            Assert.IsInstanceOf<SidebarContentViewModel>(result);
            Assert.AreEqual(2, result.LastPosts.Count);
            Assert.AreEqual(2, result.TrendingPosts.Count);
            Assert.AreEqual(1, result.LastCommentedPosts.Count);
        }

        [Test]
        public void When_BuildSearchResultViewModel_Returns_SearchViewModel()
        {
            //Given
            //When
            var result = _postBuilder.BuildSearchResultViewModel();
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<SearchViewModel>(result);
        }

        [Test]
        public void Given_listOfPosts_When_BuildListOfPostsViewModel_Returns_ListOfSinglePostViewModel()
        {
            //Given
            //When
            var result = _postBuilder.BuildListOfPostsViewModel(_postList);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<List<SinglePostViewModel>>(result);
            Assert.AreEqual(2,result.Count);
        }

        [Test]
        public void When_BuildCreatePostViewModel_Returns_CreatePostViewModelViewModel()
        {
            //Given
            //When
            var result = _postBuilder.BuildCreatePostViewModel();
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<CreatePostViewModel>(result);
        }

        [Test]
        public void Given_postId_When_BuildEditPostViewModel_Returns_EditPostViewModel()
        {
            //Given
            var postId = 1;
            //When
            var result = _postBuilder.BuildEditPostViewModel(postId);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<EditPostViewModel>(result);
            Assert.AreEqual(_post1.Content, result.Content);
            Assert.AreEqual(_post1.Id, result.Id);
            Assert.AreEqual(_post1.SubTitle, result.SubTitle);
            Assert.AreEqual(_post1.Title, result.Title);
        }

        [Test]
        public void Given_commentId_When_BuildEditCommentViewModel_Returns_EditCommentViewModel()
        {
            //Given
            var commentId = 1;
            //When
            var result = _postBuilder.BuildEditCommentViewModel(commentId);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<EditCommentViewModel>(result);
            Assert.AreEqual(_comment1.Content, result.Content);
            Assert.AreEqual(_comment1.PostId, result.CallbackId);
            Assert.AreEqual(_comment1.Id, result.Id);
        }

        [Test]
        public void Given_commentId_When_BuildReportCommentViewModel_Returns_ReportCommentViewModel()
        {
            //Given
            var commentId = 1;
            var callbackId = 2;
            //When
            var result = _postBuilder.BuildReportCommentViewModel(commentId, callbackId);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<ReportCommentViewModel>(result);
            Assert.AreEqual(callbackId, result.CallbackPostId);
            Assert.AreEqual(commentId, result.CommentId);
        }

        [Test]
        public void Given_searchKey_When_BuildSearchResultViewModel_Returns_SearchResultViewModel()
        {
            //Given
            string searchKey = "SHAAA";
            //When
            var result = _postBuilder.BuildSearchResultViewModel(searchKey);
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<SearchResultViewModel>(result);

        }
    }
}