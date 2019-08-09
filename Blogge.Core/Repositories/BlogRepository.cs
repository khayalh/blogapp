using Blogge.Interfaces.Converters;
using Blogge.Models.EntityModels;

namespace Blogge.Core.Repositories
{
    using Blogge.Models;
    using Blogge.Interfaces.Repositories;
    using System.Collections.Generic;
    using System.Linq;
    using Blogge.Interfaces.Facades.DB;
    using Blogge.Models.ViewModels;
    using Blogge.Interfaces.Factories.Model;
    using Blogge.Models.DB;
    using Blogge.Models.Enums;

    public class BlogRepository : IBlogRepository
    {
        private readonly IDBContextFacade _dBContext;
        private readonly IDataConverter _dataConverter;
        private readonly IPostFactory _postFactory;
        private ApplicationDbContext _db;

        public BlogRepository(IDBContextFacade dbContext,
            IDataConverter dataConverter,
            IPostFactory postFactory)
        {
            _dBContext = dbContext;
            _dataConverter = dataConverter;
            _postFactory = postFactory;
            _db = _dBContext.GetDBContext();
        }

        public List<Post> GetPosts()
        {
            var posts = _db.Posts;
            return posts.ToList();
        }

        //ADD
        public void AddPost(CreatePostViewModel p)
        {
            var post = _postFactory.CreatePost(p);
            var currentUser = _db.Users.Where(x => x.Id == post.AuthorId).Single();

            currentUser.UserPosts.Add(post);
            _db.SaveChanges();
        }

        //GET
        public Post GetPost(int postId)
        {
            var post = _db.Posts.Find(postId);
            return post;
        }

        //EDIT
        public void EditPost(EditPostViewModel p)
        {
            var post = GetPost(p.Id);

            post.Content = p.Content;
            post.Title = p.Title;
            post.SubTitle = p.SubTitle;

            if (p.PostImage != null)
            {
                post.postPicture = new PostPicture() { PostImageInBytes = _dataConverter.FileBaseToByteArray(p.PostImage) };
            }
            _db.SaveChanges();
        }

        //DELETE
        public void DeletePost(int postId)
        {
            var post = GetPost(postId);

            post.IsDeleted = true;
            var comments = _db.Comments.Where(x => x.Post.Id == post.Id).ToList();
            foreach (var comment in comments)
            {
                DeleteComment(comment.Id);
            }
            _db.SaveChanges();
        }

        public void RatePost(int postId, string userId, RatingType ratingType)
        {
            var post = GetPost(postId);
            var usersLikeExist = post.Likes.Where(x => x.UserID == userId).SingleOrDefault();

            if (usersLikeExist == null)
            {
                AddRating(userId, ratingType, post);
            }
            else
            {
                UpdateRating(ratingType, post, usersLikeExist);
            }
            _db.SaveChanges();
        }

        public void UpdateRating(RatingType ratingType, Post post, Like usersLikeExist)
        {
            if (usersLikeExist.RatingType == ratingType)
            {
                _db.PostLikes.Remove(usersLikeExist);
                post.Rank = (usersLikeExist.RatingType == RatingType.Positive) ? --post.Rank : ++post.Rank;
            }
            else
            {
                post.Rank = (usersLikeExist.RatingType == RatingType.Positive) ? post.Rank - 2 : post.Rank + 2;
                usersLikeExist.RatingType = (usersLikeExist.RatingType == RatingType.Positive) ? RatingType.Negative : RatingType.Positive;
            }
        }

        public void AddRating(string userId, RatingType ratingType, Post post)
        {
            var like = _postFactory.CreateLike(post.Id, userId, ratingType);
            post.Likes.Add(like);
            post.Rank = (ratingType == RatingType.Positive) ? ++post.Rank : --post.Rank;
        }

        public Comment GetComment(int id)
        {
            var comment = _db.Comments.Find(id);
            return comment;
        }

        public List<Comment> GetComments()
        {
            var commentList = _db.Comments.ToList();
            return commentList;
        }

        public void AddComment(AddCommentViewModel model)
        {
            var comment = _postFactory.CreateComment(model);

            _db.Posts.Find(model.PostId).Comments.Add(comment);
            _db.SaveChanges();
        }

        public void EditComment(EditCommentViewModel p)
        {
            var comment = GetComment(p.Id);

            comment.Content = p.Content;

            _db.SaveChanges();
        }

        //DELETE
        public void DeleteComment(int commentId)
        {

            var comment = GetComment(commentId);
            comment.IsDeleted = true;

            _db.SaveChanges();
        }

        public List<Report> GetReports()
        {
            return _db.Reports.ToList();
        }

        public void ReportComment(ReportCommentViewModel model)
        {

            var comment = GetComment(model.CommentId);

            var report = _postFactory.CreateReport(model, comment);

            comment.Reports.Add(report);
            _db.SaveChanges();
        }

        public void BlockComment(int commentId)
        {
            var comment = GetComment(commentId);
            comment.Blocked = (comment.Blocked) ? false : true;
            _db.SaveChanges();
        }

        public void DeleteReport(int reportId)
        {
            var report = GetReport(reportId);
            report.IsDeleted = true;
            _db.SaveChanges();
        }

        public Report GetReport(int reportId)
        {
            return _db.Reports.Find(reportId);
        }

        public List<Post> SearchPosts(string searchKey)
        {
            var result = _db.Posts
                .Where(x => x.IsDeleted == false && (x.Title.StartsWith(searchKey)
                || x.Title.Contains(searchKey)
                || searchKey == null))
                .ToList();

            return result;
        }
    }

}