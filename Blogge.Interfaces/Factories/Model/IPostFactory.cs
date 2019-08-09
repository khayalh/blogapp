using Blogge.Models.EntityModels;
using Blogge.Models.Enums;
using Blogge.Models.ViewModels;

namespace Blogge.Interfaces.Factories.Model
{
    public interface IPostFactory
    {
        Post CreatePost(CreatePostViewModel p);

        Like CreateLike(int PostId, string UserId, RatingType ratingType);

        Comment CreateComment(AddCommentViewModel model);

        Report CreateReport(ReportCommentViewModel model, Comment comment);
    }
}