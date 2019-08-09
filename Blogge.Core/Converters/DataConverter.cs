using System;
using System.Drawing;
using System.Web;
using Blogge.Interfaces.Converters;
using Blogge.Interfaces.Facades.Systems;

namespace Blogge.Core.Converters
{
    public class DataConverter : IDataConverter
    {
        private readonly IMemoryStreamFacade _memoryStreamFacade;
        private readonly IImageFacade _imageFacade;


        public DataConverter(IMemoryStreamFacade memoryStreamFacade, IImageFacade imageFacade)
        {
            _memoryStreamFacade = memoryStreamFacade;
            _imageFacade = imageFacade;
        }

        public byte[] FileBaseToByteArray(HttpPostedFileBase file)
        {
            var image = ConvertToImage(file);
            var ms = _memoryStreamFacade.GetMemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            return ms.ToArray();
        }

        private Image ConvertToImage(HttpPostedFileBase file)
        {
            Image sourceImage = _imageFacade.ImageFromStream(file.InputStream);
            return sourceImage;
        }

        public string ConvertToString(byte[] image)
        {
            if (image == null)
            {
                return string.Empty;
            }
            var imageInString = Convert.ToBase64String(image);
            imageInString = String.Format("data:image/gif;base64,{0}", imageInString);
            return imageInString;
        }


    }
}