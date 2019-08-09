using System.Web;

namespace Blogge.Interfaces.Validators
{
    public interface IFileTypeValidator
    {
        bool IsValidFiletype(HttpPostedFileBase file);
    }
}