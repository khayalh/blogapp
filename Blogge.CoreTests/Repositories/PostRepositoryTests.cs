
using NUnit.Framework;
using Moq;
using Blogge.Core.Repositories;
using System.Data.Entity;
using System.Linq;
using System.Collections.Generic;
using Blogge.Models.DB;
using Blogge.Interfaces.Facades.DB;
using Blogge.Interfaces.Factories.Model;
using Blogge.Models.ViewModels;
using Blogge.Interfaces.Converters;
using Blogge.Models.EntityModels;
using Blogge.Models.Enums;
using Assert = NUnit.Framework.Assert;

namespace Blogge.CoreTests.Repositories
{
    [TestFixture]
    public class ArticleRepositoryTests : DbSet<Post>
    {
        private BlogRepository _blogRepository;
        Mock<IDBContextFacade> _dbContextFacade;
        Mock<IDataConverter> _dataConverter;
        Mock<IPostFactory> _postFactory;
        Mock<ApplicationDbContext> _databaseImitation;
        Mock<DbSet<Post>> _dbsetMockPost;
        Mock<DbSet<Comment>> _dbsetMockComment;
        Mock<DbSet<ApplicationUser>> _dbsetMockUser;
        Mock<DbSet<Report>> _dbsetMockReport;
        Mock<DbSet<Like>> _dbsetMockLike;
        ApplicationUser User;
        List<Post> postList;
        List<Report>reportList;
        List<Comment> commentList;
        List<ApplicationUser> userList;
        List<Like> likeList;
        Post post1;
        Post post2;
        Like positiveLike;
        Like negativeLike;
        Comment comment2;
        Comment comment1;
        Report report1;
        Report report2;

        [SetUp]
        public void Setup()
        {
            _dbContextFacade = new Mock<IDBContextFacade>(MockBehavior.Loose);
            _dataConverter = new Mock<IDataConverter>(MockBehavior.Strict);
            _postFactory = new Mock<IPostFactory>(MockBehavior.Strict);

            _databaseImitation = new Mock<ApplicationDbContext>(MockBehavior.Loose);
            _dbsetMockPost = new Mock<DbSet<Post>>(MockBehavior.Strict);
            _dbsetMockUser = new Mock<DbSet<ApplicationUser>>(MockBehavior.Strict);
            _dbsetMockComment = new Mock<DbSet<Comment>>(MockBehavior.Strict);
            _dbsetMockLike = new Mock<DbSet<Like>>(MockBehavior.Strict);
            _dbsetMockReport = new Mock<DbSet<Report>>(MockBehavior.Strict);

            User = new ApplicationUser()
            {
                Id = "randomId",
                UserPosts = new List<Post>()
            };

            post1 = new Post()
            {
                Id = 0,
                Author = "Shrekko",
                AuthorId = "randomId",
                Title = "MockPost",
                Content = "RandomContent",
                SubTitle = "Shrej",
                User = User,
                Likes = new List<Like>()
        };

            post2 = new Post()
            {
                Id = 1,
                Author = "Shrekkoko",
                AuthorId = "randomId2",
                Title = "MockPost2",
                Content = "RandomContent2",
                SubTitle = "rORKA",
                User = User,
                Likes = new List<Like>()
            };

            postList = new List<Post>();
            postList.Add(post1);
            postList.Add(post2);

            comment1 = new Comment()
            {
                Id = 1,
                Author = "Shrekko",
                AuthorId = "randomId1",
                Content = "RandomContent1",
                User = User,
                Post = post1,
                Reports = new List<Report>()
            };

            comment2 = new Comment()
            {
                Id = 2,
                Author = "Shrekkoko",
                AuthorId = "randomId2",
                Content = "RandomContent2",
                User = User,
                Post = post2,
                Reports = new List<Report>()
            };

            positiveLike = new Like()
            {
                Id = 1,
                RatingType = RatingType.Positive,
                User = User,
                UserID = User.Id
            };
            negativeLike = new Like()
            {
                Id = 2,
                RatingType = RatingType.Negative,
                User = User,
                UserID = User.Id
            };

            likeList = new List<Like>();
            likeList.Add(positiveLike);
            likeList.Add(negativeLike);

            report1 = new Report()
            {
                Id = 1,
                Comment = comment1,
                ReportText = "UMMAYAD",
                SenderName = User.UserName,
                CommentId = comment1.Id
            };

            report2 = new Report()
            {
                Id = 2,
                Comment = comment2,
                ReportText = "BAD comment, indeed aga",
                SenderName = User.UserName,
                CommentId = comment2.Id
            };

            reportList = new List<Report>();
            reportList.Add(report1);
            reportList.Add(report2);

            userList = new List<ApplicationUser>();
            userList.Add(User);

            commentList = new List<Comment>();
            commentList.Add(comment1);
            commentList.Add(comment2);

            User.UserPosts = postList;
            _dbContextFacade.Setup(x => x.GetDBContext()).Returns(_databaseImitation.Object);
            _databaseImitation.Setup(x => x.Set<Post>()).Returns(_dbsetMockPost.Object);

            _dbsetMockPost.As<IQueryable<Post>>().Setup(m => m.Provider).Returns(postList.AsQueryable().Provider);
            _dbsetMockPost.As<IQueryable<Post>>().Setup(m => m.Expression).Returns(postList.AsQueryable().Expression);
            _dbsetMockPost.As<IQueryable<Post>>().Setup(m => m.ElementType).Returns(postList.AsQueryable().ElementType);
            _dbsetMockPost.As<IQueryable<Post>>().Setup(m => m.GetEnumerator()).Returns(postList.AsQueryable().GetEnumerator());
            _dbsetMockPost.As<IEnumerable<Post>>().Setup(m => m.GetEnumerator()).Returns(postList.AsQueryable().GetEnumerator());

            _dbsetMockComment.As<IQueryable<Comment>>().Setup(m => m.Provider).Returns(commentList.AsQueryable().Provider);
            _dbsetMockComment.As<IQueryable<Comment>>().Setup(m => m.Expression).Returns(commentList.AsQueryable().Expression);
            _dbsetMockComment.As<IQueryable<Comment>>().Setup(m => m.ElementType).Returns(commentList.AsQueryable().ElementType);
            _dbsetMockComment.As<IQueryable<Comment>>().Setup(m => m.GetEnumerator()).Returns(commentList.AsQueryable().GetEnumerator());
            _dbsetMockComment.As<IEnumerable<Comment>>().Setup(m => m.GetEnumerator()).Returns(commentList.AsQueryable().GetEnumerator());

            _dbsetMockReport.As<IQueryable<Report>>().Setup(m => m.Provider).Returns(reportList.AsQueryable().Provider);
            _dbsetMockReport.As<IQueryable<Report>>().Setup(m => m.Expression).Returns(reportList.AsQueryable().Expression);
            _dbsetMockReport.As<IQueryable<Report>>().Setup(m => m.ElementType).Returns(reportList.AsQueryable().ElementType);
            _dbsetMockReport.As<IQueryable<Report>>().Setup(m => m.GetEnumerator()).Returns(reportList.AsQueryable().GetEnumerator());
            _dbsetMockReport.As<IEnumerable<Report>>().Setup(m => m.GetEnumerator()).Returns(reportList.AsQueryable().GetEnumerator());

            _dbsetMockLike.As<IQueryable<Like>>().Setup(m => m.Provider).Returns(likeList.AsQueryable().Provider);
            _dbsetMockLike.As<IQueryable<Like>>().Setup(m => m.Expression).Returns(likeList.AsQueryable().Expression);
            _dbsetMockLike.As<IQueryable<Like>>().Setup(m => m.ElementType).Returns(likeList.AsQueryable().ElementType);
            _dbsetMockLike.As<IQueryable<Like>>().Setup(m => m.GetEnumerator()).Returns(likeList.AsQueryable().GetEnumerator());
            _dbsetMockLike.As<IEnumerable<Like>>().Setup(m => m.GetEnumerator()).Returns(likeList.AsQueryable().GetEnumerator());

            _dbsetMockUser.As<IQueryable<ApplicationUser>>().Setup(m => m.Provider).Returns(userList.AsQueryable().Provider);
            _dbsetMockUser.As<IQueryable<ApplicationUser>>().Setup(m => m.Expression).Returns(userList.AsQueryable().Expression);
            _dbsetMockUser.As<IQueryable<ApplicationUser>>().Setup(m => m.ElementType).Returns(userList.AsQueryable().ElementType);
            _dbsetMockUser.As<IQueryable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(userList.AsQueryable().GetEnumerator());
            _dbsetMockUser.As<IEnumerable<ApplicationUser>>().Setup(m => m.GetEnumerator()).Returns(userList.AsQueryable().GetEnumerator());

            _dbsetMockPost.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => postList.AsQueryable().FirstOrDefault(d => d.Id == (int)ids[0]));
            _dbsetMockComment.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => commentList.AsQueryable().FirstOrDefault(d => d.Id == (int)ids[0]));
            _dbsetMockReport.Setup(m => m.Find(It.IsAny<object[]>())).Returns<object[]>(ids => reportList.AsQueryable().FirstOrDefault(d => d.Id == (int)ids[0]));

            _dbsetMockLike.Setup(x => x.Remove(negativeLike)).Callback<Like>((entity) => likeList.Remove(entity));
            _dbsetMockLike.Setup(x => x.Remove(positiveLike)).Callback<Like>((entity) => likeList.Remove(entity));

            _dbsetMockPost.Setup(m => m.Remove(It.IsAny<Post>())).Callback<Post>((entity) => postList.Remove(entity));

            _databaseImitation.Setup(x => x.Posts.Remove(It.IsAny<Post>())).Callback<Post>((entity) => postList.Remove(entity)); 

            _databaseImitation.Setup(x => x.Posts).Returns(_dbsetMockPost.Object);
            _databaseImitation.Setup(x => x.Users).Returns(_dbsetMockUser.Object);
            _databaseImitation.Setup(x => x.Comments).Returns(_dbsetMockComment.Object);
            _databaseImitation.Setup(x => x.PostLikes).Returns(_dbsetMockLike.Object);
            _databaseImitation.Setup(x => x.Reports).Returns(_dbsetMockReport.Object);

            _postFactory.Setup(x => x.CreateLike(It.IsAny<int>(), It.IsAny<string>(), RatingType.Positive)).Returns(positiveLike);
            _postFactory.Setup(x => x.CreateLike(It.IsAny<int>(), It.IsAny<string>(), RatingType.Negative))
                .Returns(negativeLike);

            _databaseImitation.Setup(x => x.PostLikes.Remove(negativeLike)).Callback<Like>((entity) => likeList.Remove(entity));
            _databaseImitation.Setup(x => x.PostLikes.Remove(positiveLike)).Callback<Like>((entity) => likeList.Remove(entity));

            _databaseImitation.Setup(x => x.SaveChanges());
            _blogRepository = new BlogRepository(_dbContextFacade.Object, _dataConverter.Object, _postFactory.Object);
        }

        //[Test]
        //public void When_GetPosts_Then_returns_postList()
        //{
        //    //When
        //    var result = _blogRepository.GetPosts();

        //    //Then
        //    Assert.IsInstanceOf<List<Post>>(result);
        //    Assert.AreEqual(2, result.Count);
        //    Assert.NotNull(result);
        //}


        [Test]
        public void Given_postId_When_GetPost_Then_returns_post()
        {
            //Given

            var postId = 1;

            //When
            var result = _blogRepository.GetPost(postId);

            //Then
            Assert.IsInstanceOf<Post>(result);
            Assert.AreEqual("Shrekkoko", result.Author);
            Assert.AreEqual("rORKA", result.SubTitle);
            Assert.AreEqual("RandomContent2", result.Content);
        }

        [Test]
        public void Given_CreatePostViewModel_When_AddPost_Then_adds_post()
        {
            //Given
            var brandNewUser = new ApplicationUser()
            {
                Id = "randomId2",
                UserPosts = new List<Post>()
            };
            userList.Add(brandNewUser);

            var brandNeWPost = new Post()
            {
                Id = 0,
                Author = brandNewUser.UserName,
                AuthorId = brandNewUser.Id,
                Title = "MockPossdfdsft",
                Content = "RandosdfsdfsdmContent",
                SubTitle = "Shrsdfsdfej",
                User = brandNewUser,
                Likes = new List<Like>()
            };
            var model = new CreatePostViewModel()
            {
            };
            _postFactory.Setup(x=>x.CreatePost(It.IsAny<CreatePostViewModel>())).Returns(brandNeWPost);
            //When
            _blogRepository.AddPost(model);

            //Then
            Assert.AreEqual(1, brandNewUser.UserPosts.Count);
            Assert.AreEqual(brandNeWPost.Content, brandNewUser.UserPosts.Single().Content);
            Assert.AreEqual(brandNeWPost.Title, brandNewUser.UserPosts.Single().Title);
            Assert.AreEqual(brandNeWPost.SubTitle, brandNewUser.UserPosts.Single().SubTitle);

        }

        [Test]
        public void Given_EditPostViewModel_When_EditPost_Then_returns_UPDATED_post()
        {
            //Given
           var post3 = new Post()
            {
                Id = 10,
                Author = "Shrekkoko",
                AuthorId = "randomId2",
                Title = "MockPost2",
                Content = "RandomContent2",
                SubTitle = "rORKA",
                User = User
            };
            postList.Add(post3);

            var model = new EditPostViewModel()
            {
                Id = 10,
                Title = "KOKS",
                Content = "KOKS",
                SubTitle = "KOKS",
            };

            //When
            _blogRepository.EditPost(model);

            //Then
            Assert.AreEqual("KOKS", post3.Title);
            Assert.AreEqual("KOKS", post3.SubTitle);
            Assert.AreEqual("KOKS", post3.Content);
        }

        [Test]
        public void Given_postId_When_DeletePost_Then_returns_UPDATED_post()
        {
            //Given
            var post3 = new Post()
            {
                Id = 10,
                Author = "Shrekkoko",
                AuthorId = "randomId2",
                Title = "MockPost2",
                Content = "RandomContent2",
                SubTitle = "rORKA",
                User = User
            };
            postList.Add(post3);

            var postId = 10;

            //When
            _blogRepository.DeletePost(postId);

            //Then
            Assert.AreEqual(true, post3.IsDeleted);
        }


        [TestCase(0, "randomId", RatingType.Negative, -1, 1)]
        [TestCase(0, "randomId", RatingType.Positive, 1, 1)]
        public void Given_postId_userId_NEGATIVE_nonEXISTING_ratingType_When_RatePost_Then_updates_post(int postID, string userId, RatingType rating, int expectedRank, int expectedLikeCount )
        {

            //When
            _blogRepository.RatePost(postID, userId, rating);

            //Then
            Assert.AreEqual(expectedRank, post1.Rank);
            Assert.AreEqual(expectedLikeCount, post1.Likes.Count);
            Assert.AreEqual(userId, post1.Likes.Single().User.Id);
            Assert.AreEqual(rating, post1.Likes.Single().RatingType);
        }

        [TestCase(0, "randomId", RatingType.Positive, RatingType.Positive, 0)]
        [TestCase(0, "randomId", RatingType.Positive, RatingType.Negative, -1)]
        [TestCase(0, "randomId", RatingType.Negative, RatingType.Negative, 0)]
        [TestCase(0, "randomId", RatingType.Negative, RatingType.Positive, 1)]
        public void Given_postId_userId_POSITIVElike_POSITIVEexisting_ratingType_When_RatePost_Then_updates_post(int postID, string userId, RatingType rating1, RatingType rating2, int expectedRank)
        {
            //Given


            //When
            _blogRepository.RatePost(postID, userId, rating1);
            _blogRepository.RatePost(postID, userId, rating2);
            //Then
            Assert.AreEqual(expectedRank, post1.Rank);


        }

        [TestCase(1, "randomId", "randomId2", RatingType.Positive, RatingType.Positive, 2, 2)]
         [TestCase(1, "randomId", "randomId2", RatingType.Positive, RatingType.Negative, 0, 2)]
        [TestCase(1, "randomId", "randomId2", RatingType.Negative, RatingType.Negative, -2, 2)]
        public void Given_2_DifferentUser_POSITIVE_likes_When_RatePost_Then_updates_postRank_and_RatingType(int postID, string userId, string userId2, RatingType rating1, RatingType rating2, int expectedRank, int expectedLikeCount)
        {
            //Given      

            //When
            _blogRepository.RatePost(post2.Id, userId, rating1);
            _blogRepository.RatePost(post2.Id, userId2, rating2);
            //Then
            Assert.AreEqual(expectedRank, post2.Rank);
            Assert.AreEqual(expectedLikeCount, post2.Likes.Count);

        }

        [Test]
        public void Given_2_DifferentUser_POSITIVE_and_NEGATIVE_likes_When_RatePost_Then_updates_postRank_and_RatingType()
        {
            //Given
            var User2 = new ApplicationUser()
            {
                Id = "randomId2",
                UserPosts = null
            };

            //When
            _blogRepository.RatePost(post2.Id, User.Id, RatingType.Positive);
            _blogRepository.RatePost(post2.Id, User2.Id, RatingType.Negative);
            //Then
            Assert.AreEqual(0, post2.Rank);
            Assert.AreEqual(2, post2.Likes.Count);
        }
        [Test]
        public void Given_2_DifferentUser_NEGATIVE_and_NEGATIVE_likes_When_RatePost_Then_updates_postRank_and_RatingType()
        {
            //Given
            var User2 = new ApplicationUser()
            {
                Id = "randomId2",
                UserPosts = null
            };

            //When
            _blogRepository.RatePost(post2.Id, User.Id, RatingType.Negative);
            _blogRepository.RatePost(post2.Id, User2.Id, RatingType.Negative);
            //Then
            Assert.AreEqual(-2, post2.Rank);
            Assert.AreEqual(2, post2.Likes.Count);
        }

        [Test]
        public void When_GetComments_Then_returns_commentList()
        {
            //When
            var result = _blogRepository.GetComments();

            //Then
            Assert.IsInstanceOf<List<Comment>>(result);
            Assert.AreEqual(2, result.Count);
            Assert.NotNull(result);
        }

        [Test]
        public void Given_commentId_When_GetComment_Then_returns_comment()
        {
            //gIVEN
    
            //When
            var result = _blogRepository.GetComment(comment1.Id);

            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<Comment>(result);
            Assert.AreEqual(comment1.Id, result.Id);
            Assert.AreEqual(comment1.Content, result.Content);
            Assert.AreEqual(comment1.User.Id, result.User.Id);
        }

        [Test]
        public void Given_CreateCommentViewModel_When_AddComment_Then_adds_comment()
        {
            //Given
            var brandNewUser = new ApplicationUser()
            {
                Id = "randomId2",
            };
            
            userList.Add(brandNewUser);

            var brandNeWPost = new Post()
            {
                Id = 5,
                Author = brandNewUser.UserName,
                AuthorId = brandNewUser.Id,
                Title = "MockPossdfdsft",
                Content = "RandosdfsdfsdmContent",
                SubTitle = "Shrsdfsdfej",
                User = brandNewUser,
                Comments = new List<Comment>()
            };
            postList.Add(brandNeWPost);

            var brandNeWComment = new Comment()
            {
                Id = 30000,
                Author = brandNewUser.UserName,
                AuthorId = brandNewUser.Id,
                Content = "RandosdfsdfsdmContent",
                User = brandNewUser,
   
            };
            var model = new AddCommentViewModel()
            {
                PostId = 5
            };
            _postFactory.Setup(x => x.CreateComment(It.IsAny<AddCommentViewModel>())).Returns(brandNeWComment);
            //When
            _blogRepository.AddComment(model);

            //Then
            Assert.AreEqual(1, brandNeWPost.Comments.Count);
            Assert.AreEqual(brandNeWComment.Content, brandNeWPost.Comments.Single().Content);
            Assert.AreEqual(brandNeWComment.Id, brandNeWPost.Comments.Single().Id);
            Assert.AreEqual(brandNeWComment.AuthorId, brandNeWPost.Comments.Single().AuthorId);
        }

        [Test]
        public void Given_EditCommentViewModel_When_EditComment_Then_updates_post()
        {
            //Given

            var model = new EditCommentViewModel()
            {
                Id = 2,
                Content = "KOKS",
            };

            //When
            _blogRepository.EditComment(model);

            //Then
            Assert.AreEqual("KOKS", comment2.Content);
        }

        [Test]
        public void Given_commentId_When_DeleteComment_Then_softDeletes_comment()
        {
            //Given

            var commentId = 1;

            //When
            _blogRepository.DeleteComment(commentId);

            //Then
            Assert.AreEqual(true, comment1.IsDeleted);
        }

        [Test]
        public void When_ReportComment_Then_adds_report()
        {
            //given
            var model = new ReportCommentViewModel()
            {
                CallbackPostId = 1,
                CommentId = 1,
                Content = "UMMAYAD"
            };

            _postFactory.Setup(x=>x.CreateReport(It.IsAny<ReportCommentViewModel>(), It.IsAny<Comment>())).Returns(report1);
            //When
            _blogRepository.ReportComment(model);

            //Then
            Assert.AreEqual(1, comment1.Reports.Count);
            Assert.AreEqual(comment1.Id, comment1.Reports.Single().CommentId);
            Assert.AreEqual(model.Content, comment1.Reports.Single().ReportText);
        }

        [Test]
        public void Given_commentId_When_BlockComment_Then_blocks_comment()
        {
            //Given

            var commentId = 1;

            //When
            _blogRepository.BlockComment(commentId);

            //Then
            Assert.AreEqual(true, comment1.Blocked);
        }
        [Test]
        public void Given_commentId_When_BlockComment_blockedComment_Then_unBlocks_comment()
        {
            //Given

            var commentId = 1;

            //When
            _blogRepository.BlockComment(commentId);
            _blogRepository.BlockComment(commentId);

            //Then
            Assert.AreEqual(false, comment1.Blocked);
        }

        [Test]
        public void Given_reportId_When_deleteReport_Then_Deletes_report()
        {
            //Given

            var reportId = 1;

            //When
            _blogRepository.DeleteReport(reportId);

            //Then
            Assert.AreEqual(true, report1.IsDeleted);
        }

        [Test]
        public void Given_reportId_When_GetReport_Then_returns_report()
        {
            //Given

            var reportId = 1;

            //When
           var result = _blogRepository.GetReport(reportId);

            //Then
            Assert.NotNull(result);
            Assert.AreEqual(result.Id, report1.Id);
        }

        //[Test]
        //public void Given_searchKey_When_SearchPosts_Then_returns_single_needed_post()
        //{
        //    //Given
        //    var searchkey = "MockPost2";

        //    //When
        //    Post result = _blogRepository.SearchPosts(searchkey).Single();

        //    //Then
        //    Assert.NotNull(result);
        //    Assert.AreEqual(post2.Id, result.Id);
        //}
        //[Test]
        //public void Given_searchKey_When_SearchPosts_Then_returns_multiple_needed_post()
        //{
        //    //Given

        //    var searchkey = "MockPost";

        //    //When
        //    List<Post> result = _blogRepository.SearchPosts(searchkey);

        //    //Then
        //    Assert.NotNull(result);
        //    Assert.AreEqual(true, result.Contains(post1));
        //    Assert.AreEqual(true, result.Contains(post2));
        //}

        public void sdf(int t)
        {
            Assert.AreEqual("{t}", t.ToString());
        }
    }


}
