
using Blogge.Interfaces.Facades.Systems;
using System.Diagnostics.CodeAnalysis;
using System.IO;


namespace Blogge.Facades.Systems
{
    [ExcludeFromCodeCoverage]
    public class MemoryStreamFacade : IMemoryStreamFacade
    {
        public MemoryStream GetMemoryStream()
        {
            return new MemoryStream();
        }
    }
}
