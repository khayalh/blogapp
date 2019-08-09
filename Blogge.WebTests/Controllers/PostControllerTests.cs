using System;
using System.Web.Mvc;
using System.Web.Routing;
using Blogge.Interfaces.Builders.Model;
using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Repositories;
using Blogge.Interfaces.Validators;
using Blogge.Models.EntityModels;
using Blogge.Models.Enums;
using Blogge.Models.ViewModels;
using Blogge.Web.Controllers;
using Moq;
using NUnit.Framework;
using RedirectToRouteResult = System.Web.Mvc.RedirectToRouteResult;

namespace Blogge.WebTests.Controllers
{
    [TestFixture]
    public class PostControllerTests
    {
        private Mock<IBlogRepository> _postRepository;
        private PostController _postController;
        private Mock<IIdentityFacade> _identityFacade;
        private Mock<IPostBuilder> _postBuilder;
        private Mock<IDisplayValidator> _displayValidator;
        private Mock<IAccessValidator> _accessValidator;
        private Post post;
        private Comment comment;
        private RouteData routeData;

        [SetUp]
        public void Setup()
        {
            _postRepository = new Mock<IBlogRepository>(MockBehavior.Strict);
            _postBuilder = new Mock<IPostBuilder>(MockBehavior.Strict);
            _identityFacade = new Mock<IIdentityFacade>(MockBehavior.Strict);
            _displayValidator = new Mock<IDisplayValidator>(MockBehavior.Strict);
            _accessValidator = new Mock<IAccessValidator>(MockBehavior.Strict);


            _postController = new PostController(
                _postRepository.Object,
                _identityFacade.Object,
                _postBuilder.Object,
                _displayValidator.Object,
                _accessValidator.Object);

            _identityFacade.Setup(x => x.GetUserName()).Returns("UserKek");
            _identityFacade.Setup(x => x.GetUserId()).Returns("UserKekID");

            routeData = new RouteData();
            routeData.Values.Add("key1", "value1");

            _identityFacade.Setup(x => x.GetRouteData()).Returns(routeData);

            _displayValidator.Setup(X => X.IsAvailableComment(It.IsAny<int>())).Returns(true);
            _displayValidator.Setup(X => X.IsAvailableComment(It.IsAny<Comment>())).Returns(true);
            _displayValidator.Setup(X => X.IsAvailablePost(It.IsAny<int>())).Returns(true);
            _displayValidator.Setup(X => X.IsAvailablePost(It.IsAny<Post>())).Returns(true);
            _accessValidator.Setup(X => X.CanAccessComment(It.IsAny<int>())).Returns(true);
            _accessValidator.Setup(X => X.CanAccessPost(It.IsAny<int>())).Returns(true);

            post = new Post()
            {
                Id = 1,
                Title = "kekTitle",
                Content = "kekContent",
                AuthorId = "UserKekID"
            };

            comment = new Comment()
            {
                Id = 1,
                Content = "kekContent",
                AuthorId = "UserKekID"
            };
            _accessValidator.Setup(x => x.IsRedactor()).Returns(true);
            _accessValidator.Setup(x => x.IsAdministration()).Returns(true);

            _postRepository.Setup(x => x.AddPost(It.IsAny<CreatePostViewModel>()));
            _postRepository.Setup(x => x.AddComment(It.IsAny<AddCommentViewModel>()));
            _postRepository.Setup(x => x.EditPost(It.IsAny<EditPostViewModel>()));
            _postRepository.Setup(x => x.EditComment(It.IsAny<EditCommentViewModel>()));
            _postRepository.Setup(x => x.GetPost(It.IsAny<int>())).Returns(post);
            _postRepository.Setup(x => x.DeletePost(It.IsAny<int>()));
            _postRepository.Setup(x => x.DeleteComment(It.IsAny<int>()));
            _postRepository.Setup(x => x.RatePost(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<RatingType>()));
            _postRepository.Setup(x => x.GetComment(It.IsAny<int>())).Returns(comment);
            _postRepository.Setup(x => x.ReportComment(It.IsAny<ReportCommentViewModel>()));
        }

        [Test]
        public void When_Index_Returns_Index()
        {
            //When
            var result = _postController.Index() as RedirectToRouteResult;
            //Then
            Assert.AreEqual("Home", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
        }

        [Test]
        public void When_Create_Returns_CreateView()
        {
            //Given
            _postBuilder.Setup(x => x.BuildCreatePostViewModel()).Returns(new CreatePostViewModel());
            //When
            var result = _postController.Create() as ViewResult;
            //Then
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.NotNull(result);
            Assert.IsInstanceOf<CreatePostViewModel>(result.Model);
        }

        [Test]
        public void Given_valid_CreatePostViewModel_When_Create_Returns_Home_Index()
        {
            //Given
            var model = new CreatePostViewModel();
            
            //When
            var result = _postController.Create(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_CreatePostViewModel_When_unprivileged_Create_Returns_Home_Index()
        {
            //Given
            var model = new CreatePostViewModel();
            _accessValidator.Setup(x => x.IsRedactor()).Returns(false);
            _accessValidator.Setup(x => x.IsAdministration()).Returns(false);

            //When
            var result = _postController.Create(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Unauthorized", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_inValid_CreatePostViewModel_When_Create_Returns_Home_Index()
        {
            //Given
            var model = new CreatePostViewModel();
            _postController.ModelState.AddModelError(string.Empty, "There is an error with your model, boy!");
            //When
            var result = _postController.Create(model) as ViewResult;
            //Then
            Assert.AreEqual(String.Empty, result.ViewName);
            Assert.NotNull(result);
            Assert.IsInstanceOf<CreatePostViewModel>(result.Model);
        }

        [Test]
        public void Given_inValid_AddCommentViewModel_When_AddComent_Returns_SinglePost()
        {
            //Given
            var model = new AddCommentViewModel();

            _postController.ModelState.AddModelError(string.Empty, "There is an error with your model, boy!");
            //When
            var result = _postController.AddComment(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Post", result.RouteValues["controller"].ToString());
            Assert.AreEqual("SinglePost", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_Valid_AddCommentViewModel_When_AddComent_Returns_SinglePost()
        {
            //Given
            var model = new AddCommentViewModel();

            //When
            var result = _postController.AddComment(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Post", result.RouteValues["controller"].ToString());
            Assert.AreEqual("SinglePost", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_postId_When_SinglePost_Returns_SinglePostView()
        {
            //Given
            var model = new BigPostViewModel();

            _postBuilder.Setup(x => x.BuildBigPostViewModel(It.IsAny<int>())).Returns(model);
            var postId = 1;

            //When

            var result = _postController.SinglePost(postId) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<BigPostViewModel>(result.Model);
            Assert.AreEqual(String.Empty, result.ViewName);
        }

        [Test]
        public void Given_inValid_postId_When_SinglePost_Returns_404()
        {
            //Given
            var model = new BigPostViewModel();

            Post Null = null;
            _postRepository.Setup(x => x.GetPost(It.IsAny<int>())).Returns(Null);

            var postId = 1;

            //When

            var result = _postController.SinglePost(postId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("NotFound", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_post_When_Edit_Returns_SinglePostView()
        {

            //Given
            int id = 1;

            var model = new EditPostViewModel();
            _postBuilder.Setup(x => x.BuildEditPostViewModel(It.IsAny<int>())).Returns(model);

            //When
            var result = _postController.Edit(id) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<EditPostViewModel>(result.Model);
            Assert.AreEqual(String.Empty, result.ViewName);
        }

        [Test]
        public void Given_valid_post_When_unprivileged_Edit_Returns_SinglePostView()
        {
            
            //Given
            int id = 1;
            _accessValidator.Setup(x => x.IsRedactor()).Returns(false);
            _accessValidator.Setup(x => x.IsAdministration()).Returns(false);

            var model = new EditPostViewModel();
            _postBuilder.Setup(x => x.BuildEditPostViewModel(It.IsAny<int>())).Returns(model);

            //When
            var result = _postController.Edit(id) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Unauthorized", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_inValid_post_When_Edit_Returns_httpNotFound()
        {
            //Given
            int id = 1;

            Post Null = null;
            _postRepository.Setup(x => x.GetPost(It.IsAny<int>())).Returns(Null);

            //When
            var result = _postController.Edit(id) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("NotFound", result.RouteValues["action"].ToString());

        }

        [Test]
        public void Given_blockedORdeleted_post_When_Edit_Returns_Forbidden()
        {

            //Given
            int id = 1;

            _displayValidator.Setup(X => X.IsAvailablePost(It.IsAny<int>())).Returns(false);

            var model = new EditPostViewModel();
            _postBuilder.Setup(x => x.BuildEditPostViewModel(It.IsAny<int>())).Returns(model);

            //When
            var result = _postController.Edit(id) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_post_When_unprivileged_user_tries_Edit_Returns_Forbidden()
        {

            //Given
            int id = 1;

            _accessValidator.Setup(x => x.CanAccessPost(It.IsAny<int>())).Returns(false);

            var model = new EditPostViewModel();
            _postBuilder.Setup(x => x.BuildEditPostViewModel(It.IsAny<int>())).Returns(model);

            //When
            var result = _postController.Edit(id) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_EditPostViewModel_When_Edit_Returns_Home_Index()
        {
            //Given
            var model = new EditPostViewModel();

            //When
            var result = _postController.Edit(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_inValid_EditPostViewModel_When_Edit_Returns_Edit_View()
        {
            //Given
            var model = new EditPostViewModel();
            _postController.ModelState.AddModelError(string.Empty, "There is an error with your model, boy!");
            //When
            var result = _postController.Edit(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<EditPostViewModel>(result.Model);
            Assert.AreEqual(String.Empty, result.ViewName);
        }

        [Test]
        public void Given_valid_post_id_When_Delete_Returns_DeleteView()
        {
            //Given
            var id = 1;

            var model = new SinglePostViewModel();
            _postBuilder.Setup(x => x.BuildPostViewModel(It.IsAny<int>())).Returns(model);

            //When
            var result = _postController.Delete(id) as ViewResult;
            //Then

            Assert.NotNull(result);
            Assert.IsInstanceOf<SinglePostViewModel>(result.Model);
            Assert.AreEqual(String.Empty, result.ViewName);

        }

        [Test]
        public void Given_valid_post_id_When_unprivileged_user_tries_Delete_Returns_DeleteView()
        {
            //Given
            var id = 1;
            _accessValidator.Setup(x => x.IsRedactor()).Returns(false);
            _accessValidator.Setup(x => x.IsAdministration()).Returns(false);

            var model = new SinglePostViewModel();
            _postBuilder.Setup(x => x.BuildPostViewModel(It.IsAny<int>())).Returns(model);

            //When
            var result = _postController.Delete(id) as RedirectToRouteResult;
            //Then

            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Unauthorized", result.RouteValues["action"].ToString());

        }

        [Test]
        public void Given_inValid_post_id_When_Delete_Returns_404()
        { 
            //Given
            var id = 1;
            Post Null = null;
            _postRepository.Setup(x => x.GetPost(It.IsAny<int>())).Returns(Null);

            //When
            var result = _postController.Delete(id) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("NotFound", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_blockedORdeleted_post_id_When_Delete_Returns_403()
        {

            //Given
            var id = 1;

            _displayValidator.Setup(X => X.IsAvailablePost(It.IsAny<int>())).Returns(false);
            
            //When
            var result = _postController.Delete(id) as RedirectToRouteResult;
            //Then

            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_post_id_When_unprivileged_user_tries_Delete_Returns_403()
        {
            //Given
            var id = 1;

            _displayValidator.Setup(X => X.IsAvailablePost(It.IsAny<int>())).Returns(false);

            //When
            var result = _postController.Delete(id) as RedirectToRouteResult;
            //Then

            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_postId_When_DeleteConfirmed_Returns_Home_Index()
        {
            //Given
            var model = new BigPostViewModel();

            _postBuilder.Setup(x => x.BuildBigPostViewModel(It.IsAny<int>())).Returns(model);
            var postId = 1;

            //When

            var result = _postController.DeleteConfirmed(postId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Home", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Index", result.RouteValues["action"].ToString());
        }
        [Test]
        public void Given_valid_postId_and_ratingType_When_RatePost_Returns_SinglePost()
        {
            //Given
            var postId = 1;
            var ratingType = RatingType.Positive;

            //When

            var result = _postController.RatePost(postId, ratingType) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Post", result.RouteValues["controller"].ToString());
            Assert.AreEqual("SinglePost", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_inValid_postId_and_ratingType_When_RatePost_Returns_404()
        {
            //Given
            var postId = 1;
            var ratingType = RatingType.Positive;

            Post Null = null;
            _postRepository.Setup(x => x.GetPost(It.IsAny<int>())).Returns(Null);

            //When

            var result = _postController.RatePost(postId, ratingType) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("NotFound", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_blockedORdeleted_postId_and_ratingType_When_RatePost_Returns_403()
        {
            //Given
            var postId = 1;
            var ratingType = RatingType.Positive;

            _displayValidator.Setup(X => X.IsAvailablePost(It.IsAny<int>())).Returns(false);

            //When

            var result = _postController.RatePost(postId, ratingType) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_commentId_When_EditComment_Returns_EditCommentViewModel()
        {
            //Given
            var commentId = 1;
            var postId = 1;

            var model = new EditCommentViewModel();
            _postBuilder.Setup(x=>x.BuildEditCommentViewModel(It.IsAny<int>())).Returns(model);

            //When

            var result = _postController.EditComment(commentId, postId) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<EditCommentViewModel>(result.Model);
            Assert.AreEqual(String.Empty, result.ViewName);
        }


        [Test]
        public void Given_inValid_commentId_When_EditComment_Returns_404()
        {
            //Given
            var commentId = 1;
            var postId = 1;
            Comment Null = null;
           
            _postRepository.Setup(x => x.GetComment(It.IsAny<int>())).Returns(Null);
          
            //When
            var result = _postController.EditComment(commentId, postId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("NotFound", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_BlockedORdeleted_commentId_When_EditComment_Returns_403()
        {
            //Given
            var commentId = 1;
            var postId = 1;

            Post Null = null;
            _postRepository.Setup(x => x.GetPost(It.IsAny<int>())).Returns(Null);

            _displayValidator.Setup(X => X.IsAvailableComment(It.IsAny<int>())).Returns(false);
            //When

            var result = _postController.EditComment(commentId, postId) as RedirectToRouteResult;
            //Then

            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_commentId_with_blockedORdeleted_post_When_EditComment_Returns_403()
        {
            //Given
            var commentId = 1;
            var postId = 1;

            _displayValidator.Setup(X => X.IsAvailablePost(It.IsAny<int>())).Returns(false);
            //When

            var result = _postController.EditComment(commentId, postId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_commentId_When_unprivileged_user_tries_EditComment_Returns_403()
        {
            //Given
            var commentId = 1;
            var postId = 1;

            Post Null = null;
            _postRepository.Setup(x => x.GetPost(It.IsAny<int>())).Returns(Null);

            _accessValidator.Setup(X => X.CanAccessComment(It.IsAny<int>())).Returns(false);
            //When

            var result = _postController.EditComment(commentId, postId) as RedirectToRouteResult;
            //Then

            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_EditCommentViewModel_When_EditComment_Returns_SinglePost()
        {
            //Given

            var model = new EditCommentViewModel()
            {
                CallbackId = 1
            };

            _postBuilder.Setup(x => x.BuildEditCommentViewModel(It.IsAny<int>())).Returns(model);

            //When

            var result = _postController.EditComment(model) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Post", result.RouteValues["controller"].ToString());
            Assert.AreEqual("SinglePost", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_inValid_EditCommentViewModel_When_EditComment_Returns_SinglePost()
        {
            //Given

            var model = new EditCommentViewModel()
            {
                CallbackId = 1
            };

            _postBuilder.Setup(x => x.BuildEditCommentViewModel(It.IsAny<int>())).Returns(model);

            _postController.ModelState.AddModelError(string.Empty, "There is an error with your model, boy!");

            //When

            var result = _postController.EditComment(model) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<EditCommentViewModel>(result.Model);
            Assert.AreEqual(String.Empty, result.ViewName);
        }

        [Test]
        public void Given_valid_commentId_When_DeleteComment_Returns_SinglePost()
        {
            //Given

            var commentId = 1;
            var postId = 1;
            var callbackId = 2;
            //When

            var result = _postController.DeleteComment(commentId, postId, callbackId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Post", result.RouteValues["controller"].ToString());
            Assert.AreEqual("SinglePost", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_inValid_commentId_When_DeleteComment_Returns_404()
        {
            //Given

            var commentId = 1;
            var postId = 1;
            var callbackId = 2;

            Comment Null = null;
            _postRepository.Setup(x => x.GetComment(It.IsAny<int>())).Returns(Null);

            //When

            var result = _postController.DeleteComment(commentId, postId,callbackId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("NotFound", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_blockedORremoved_commentId_When_DeleteComment_Returns_403()
        {
            //Given

            var commentId = 1;
            var callbackId = 2;
            var postId = 1;

            _displayValidator.Setup(X => X.IsAvailableComment(It.IsAny<int>())).Returns(false);

            //When

            var result = _postController.DeleteComment(commentId, postId,callbackId) as RedirectToRouteResult;
            //Then

            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_commentId_with_blockedORdeleted_post_When_DeleteComment_Returns_403()
        {
            //Given

            var commentId = 1;
            var callbackId = 2;
            var postId = 1;

            _displayValidator.Setup(X => X.IsAvailablePost(It.IsAny<int>())).Returns(false);      

            //When

            var result = _postController.DeleteComment(commentId, postId, callbackId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_commentId_When_unprivileged_user_tries_DeleteComment_Returns_403()
        {
            //Given

            var commentId = 1;
            var callbackId = 2;
            var postId = 1;

            _accessValidator.Setup(X => X.CanAccessComment(It.IsAny<int>())).Returns(false);

            //When

            var result = _postController.DeleteComment(commentId, postId, callbackId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_commentId_When_ReportComment_Returns_ReportCommentViewModel()
        {
            //Given

            var commentId = 1;
            var postId = 1;
            var callbackId = 2;

            var model = new ReportCommentViewModel();
            _postBuilder.Setup(x=>x.BuildReportCommentViewModel(It.IsAny<int>(), It.IsAny<int>())).Returns(model);
            //When

            var result = _postController.ReportComment(commentId, postId, callbackId) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<ReportCommentViewModel>(result.Model);
            Assert.AreEqual(String.Empty, result.ViewName);
        }

        [Test]
        public void Given_inValid_commentId_When_ReportComment_Returns_404()
        {
            //Given

            var commentId = 1;
            var callbackId = 2;
            var postId = 1;

            Comment Null = null;
            _postRepository.Setup(x => x.GetComment(It.IsAny<int>())).Returns(Null);

            //When

            var result = _postController.ReportComment(commentId, postId, callbackId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("NotFound", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_Valid_commentId_with_blockedORdeleted_post_When_ReportComment_Returns_403()
        {
            //Given

            var commentId = 1;
            var callbackId = 2;
            var postId = 2;

            _displayValidator.Setup(X => X.IsAvailablePost(It.IsAny<int>())).Returns(false);

            //When

            var result = _postController.ReportComment(commentId, postId, callbackId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_blockedORdeleted_commentId_When_ReportComment_Returns_403()
        {
            //Given

            var commentId = 1;
            var postId = 2;
            var callbackId = 2;

           _displayValidator.Setup(X => X.IsAvailableComment(It.IsAny<int>())).Returns(false);
            //When

            var result = _postController.ReportComment(commentId, postId, callbackId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Error", result.RouteValues["controller"].ToString());
            Assert.AreEqual("Forbidden", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_valid_ReportCommentViewModel_When_ReportComment_Returns_SinglePost()
        {
            //Given

            var callbackId = 2;

            var model = new ReportCommentViewModel();

            //When

            var result = _postController.ReportComment(model, callbackId) as RedirectToRouteResult;
            //Then
            Assert.NotNull(result);
            Assert.AreEqual("Post", result.RouteValues["controller"].ToString());
            Assert.AreEqual("SinglePost", result.RouteValues["action"].ToString());
        }

        [Test]
        public void Given_inValid_ReportCommentViewModel_When_ReportComment_Returns_ReportCommentViewModel()
        {
            //Given

            var callbackId = 2;

            var model = new ReportCommentViewModel();

            _postController.ModelState.AddModelError(string.Empty, "There is an error with your model, boy!");

            //When

            var result = _postController.ReportComment(model, callbackId) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<ReportCommentViewModel>(result.Model);
            Assert.AreEqual(String.Empty, result.ViewName);
        }
    }
}