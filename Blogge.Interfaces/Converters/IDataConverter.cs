using System.Web;

namespace Blogge.Interfaces.Converters
{
    public interface IDataConverter
    {
        byte[] FileBaseToByteArray(HttpPostedFileBase file);
        string ConvertToString(byte[] image);


    }
}
