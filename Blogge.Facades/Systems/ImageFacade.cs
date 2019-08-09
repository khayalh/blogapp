
using Blogge.Interfaces.Facades.Systems;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.IO;


namespace Blogge.Facades.Systems
{
    [ExcludeFromCodeCoverage]
    public class ImageFacade: IImageFacade
    {
      public Image ImageFromStream(Stream stream)
        {
            return Image.FromStream(stream);
        }
    }
}
