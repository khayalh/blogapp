
using System.Diagnostics.CodeAnalysis;

namespace Blogge.Models.EntityModels
{
    [ExcludeFromCodeCoverage]
    public class Picture
    {

        public int Id { get; set; }

        public virtual ApplicationUser User { get; set; }

        public byte[] AvatarInBytes { get; set; }
    }
}
