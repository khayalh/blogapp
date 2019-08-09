

namespace Blogge.Interfaces.Repositories
{

    using System.Web;

    public interface IImageRepository
    {
        void AddImageToDb(string user, HttpPostedFileBase file);
        string GetImageInString(string userID);
    }
}