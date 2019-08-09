
using Blogge.Models.EntityModels;

namespace Blogge.Interfaces.Repositories
{

    using System.Collections.Generic;
    using Blogge.Models.Enums;
    using Blogge.Models.ViewModels;

    public interface IBlogRepository
    {
        List<Post> GetPosts();
        void AddPost(CreatePostViewModel p);
        Post GetPost(int postId);
        void EditPost(EditPostViewModel p);
        void EditComment(EditCommentViewModel Comment);
        void DeletePost(int postId);
        void RatePost(int postId, string userId, RatingType ratingType);
        void UpdateRating(RatingType ratingType, Post post, Like usersLikeExist);
        void AddRating(string userId, RatingType ratingType, Post post);
        Comment GetComment(int id);
        List<Comment> GetComments();
        void AddComment(AddCommentViewModel model);
        void DeleteComment(int id);
        void ReportComment(ReportCommentViewModel model);
        List<Report> GetReports();
        void BlockComment(int commentId);
        void DeleteReport(int reportId);
        List<Post> SearchPosts(string searchKey);
    }
}
