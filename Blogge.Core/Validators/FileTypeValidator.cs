
using System.Collections.Generic;
using System.Web;
using Blogge.Interfaces.Validators;

namespace Blogge.Core.Validators
{
   public class FileTypeValidator : IFileTypeValidator
    {
        private readonly List<string> _allowedFileTypes = new List<string>() { "image/jpeg", "image/gif", "image/png" };

        public bool IsValidFiletype(HttpPostedFileBase file)
        {
            bool result = _allowedFileTypes.Contains(file.ContentType);
            return result;
        }
    }
}
