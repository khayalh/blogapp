
namespace Blogge.Interfaces.Interfaces.Controllers
{
    using Blogge.Models;

    public interface IPostDBController
    {
        void AddPost(Post p, string userId);
    }
}
