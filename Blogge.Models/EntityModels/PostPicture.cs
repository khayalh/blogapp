
using System.Diagnostics.CodeAnalysis;

namespace Blogge.Models.EntityModels
{
    [ExcludeFromCodeCoverage]
    public class PostPicture
    {

        public int Id { get; set; }

        public virtual Post Post { get; set; }

        public byte[] PostImageInBytes { get; set; }
    }
}
