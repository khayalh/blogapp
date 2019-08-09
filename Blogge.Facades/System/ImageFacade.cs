using Blogge.Interfaces.Facades;
using Blogge.Interfaces.Facades.Internal;
using System.Drawing;
using System.IO;


namespace Blogge.Facades.Outer
{
   public class ImageFacade: IImageFacade
    {
      public Image ImageFromStream(Stream stream)
        {
            return Image.FromStream(stream);
        }
    }
}
