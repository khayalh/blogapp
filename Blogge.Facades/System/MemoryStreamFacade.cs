using Blogge.Interfaces.Facades;
using Blogge.Interfaces.Facades.Internal;
using System.IO;


namespace Blogge.Facades.Outer
{
   public class MemoryStreamFacade : IMemoryStreamFacade
    {
        public MemoryStream GetMemoryStream()
        {
            return new MemoryStream();
        }
    }
}
