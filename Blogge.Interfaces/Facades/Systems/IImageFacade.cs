
using System.Drawing;
using System.IO;


namespace Blogge.Interfaces.Facades.Systems
{
    public interface IImageFacade
    {
        Image ImageFromStream(Stream stream);
    }
}
