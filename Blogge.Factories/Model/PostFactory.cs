using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Factories.Model;
using Blogge.Models.Enums;
using Blogge.Models.ViewModels;
using System;
using Blogge.Interfaces.Converters;
using Blogge.Models.EntityModels;

namespace Blogge.Factories.Model
{
    public class PostFactory : IPostFactory
    {
        private readonly IIdentityFacade _identityFacade;
        private readonly IDataConverter _dataConverter;

        public PostFactory(IIdentityFacade identityFacade, IDataConverter dataConverter)
        {
            _identityFacade = identityFacade;
            _dataConverter = dataConverter;
        }

        public Post CreatePost(CreatePostViewModel p)
        {
            Post post = new Post()
            {
                Author = _identityFacade.GetUserName(),
                AuthorId = _identityFacade.GetUserId(),
                PostedAt = DateTime.Now,
                Content = p.Content,
                Title = p.Title,
                SubTitle = p.SubTitle
            };

            if (p.file != null)
            {
                post.postPicture = new PostPicture()
                {
                    PostImageInBytes = _dataConverter.FileBaseToByteArray(p.file)
                };
            }
            return post;
        }

        public Like CreateLike(int PostId, string UserId, RatingType rank)
        {
            var model = new Like() { PostId = PostId, UserID = UserId, RatingType = rank };
            return model;
        }

        public Comment CreateComment(AddCommentViewModel model)
        {
            var comment = new Comment()
            {
                Author = _identityFacade.GetUserName(),
                AuthorId = _identityFacade.GetUserId(),
                Content = model.Content,
                PostedAt = DateTime.Now,
                PostId = model.PostId
            };
            return comment;
        }

        public Report CreateReport(ReportCommentViewModel model, Comment comment)
        {
            var report = new Report()
            {
                Comment = comment,
                ReportText = model.Content,
                SenderName = _identityFacade.GetUserName()
            };
            return report;
        }
    }
}
