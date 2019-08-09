

using Blogge.Interfaces.Repositories;
using Blogge.Interfaces.Validators;
using Blogge.Models;
using Blogge.Models.EntityModels;

namespace Blogge.Core.Validators
{
    public class DisplayValidator : IDisplayValidator
    {
        private readonly IBlogRepository _blogRepository;

        public DisplayValidator(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

        public bool IsAvailableComment(int id)
        {
            var comment = _blogRepository.GetComment(id);
            bool access = CheckComment(comment);
            return access;
        }
        public bool IsAvailableComment(Comment comment)
        {
            bool access = CheckComment(comment);
            return access;
        }

        private static bool CheckComment(Comment comment)
        {
            return (!comment.IsDeleted && !comment.Blocked);
        }

        public bool IsAvailablePost(int id)
        {
            var post = _blogRepository.GetPost(id);
            bool access = CheckPost(post);
            return access;
        }

        private static bool CheckPost(Post post)
        {
            return (!post.IsDeleted && !post.Blocked);
        }

        public bool IsAvailablePost(Post post)
        {
            var access = CheckPost(post);
            return access;
        }

    }
}
