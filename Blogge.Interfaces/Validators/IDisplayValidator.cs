
using Blogge.Models.EntityModels;

namespace Blogge.Interfaces.Validators
{
    public interface IDisplayValidator
    {
        bool IsAvailableComment(int id);
        bool IsAvailableComment(Comment comment);
        bool IsAvailablePost(int id);
        bool IsAvailablePost(Post post);
    }
}