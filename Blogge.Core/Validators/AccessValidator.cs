using Blogge.Interfaces.Facades.Identity;
using Blogge.Interfaces.Repositories;
using Blogge.Interfaces.Validators;

namespace Blogge.Core.Validators
{
   public class AccessValidator : IAccessValidator
    {
        private readonly IIdentityFacade _identityFacade;
        private readonly IBlogRepository _postRepository;

        public AccessValidator(IIdentityFacade identityFacade, IBlogRepository postRepository)
        {
            _identityFacade = identityFacade;
            _postRepository = postRepository;
        }
        public bool CanAccessPost(int id)
        {
            var post = _postRepository.GetPost(id);
            return (IsAdministration() || post.AuthorId == _identityFacade.GetUserId());
        }
        public bool CanAccessComment(int id)
        {
            var comment = _postRepository.GetComment(id);
            return (IsAdministration() || comment.AuthorId == _identityFacade.GetUserId());
        }

        public bool IsAdmin()
        {
            return _identityFacade.CheckRole("admin");
        }
        public bool IsModerator()
        {
            return _identityFacade.CheckRole("moderator");
        }

        public bool IsAdministration()
        {
            return (IsModerator() || IsAdmin());
        }

        public bool IsRedactor()
        {
            return _identityFacade.CheckRole("redactor");
        }
        public bool IsUser()
        {
            return _identityFacade.CheckRole("user");
        }
    }
}
