//using Blogge.Core.Controllers;
//using NUnit.Framework;
//using System.Drawing;
//using Moq;
//using System.IO;
//using Blogge.Interfaces.Facades;
//using Blogge.Interfaces.Facades.Systems;
//using System.Web;
//using System.Configuration;
//using System;
//using System.Drawing.Imaging;

//namespace Blogge.CoreTests.Converters
//{
//    [TestFixture]
//    public class DataConverterTests
//    {
//        private DataConverter _dataConverter;
//        private Mock<IMemoryStreamFacade> _memoryStreamFacade;
//        private Mock<IImageFacade> _imageFacade;
//        private Mock<HttpPostedFileBase> file;
//        private Mock<Image> _image;
//        private Mock<MemoryStream> _memoryStream;

//        [SetUp]
//        public void Setup()
//        {
//            _memoryStreamFacade = new Mock<IMemoryStreamFacade>(MockBehavior.Strict);
//            _imageFacade = new Mock<IImageFacade>(MockBehavior.Strict);
//            _dataConverter = new DataConverter(_memoryStreamFacade.Object, _imageFacade.Object);
//            _memoryStreamFacade.Setup(x => x.GetMemoryStream()).Returns(new MemoryStream());

//            _image = new Mock<Image>(1,1);


//            file = new Mock<HttpPostedFileBase>();

//            _imageFacade.Setup(x=>x.ImageFromStream(It.IsAny<Stream>())).Returns(_image.Object);
//            _image.Setup(X=>X.Save(It.IsAny<MemoryStream>(), It.IsAny<ImageFormat>()));

//            _memoryStream = new Mock<MemoryStream>();
//            _memoryStream.Setup(x=>x.ToArray()).Returns(new byte[6]);
//                _memoryStreamFacade.Setup(x => x.GetMemoryStream()).Returns(_memoryStream.Object);
//        }

//        [Test]
//        public void Given_httpPostedfileBase_When_ImageToByteArray_Then_return_byteArray()
//        {
//            //Given
            
            
//            //When
//            var result = _dataConverter.FileBaseToByteArray(file.Object);

//            //Then
//            Assert.IsInstanceOf<byte[]>(result);
//        }

//        //[Test]
//        //public void Given_byteArray_When_ByteArrayToImage_Then_return_Image()
//        //{
//        //    //Given
//        //    byte[] byteArray = new byte[5];

//        //    //When
//        //    var result = _dataConverter.ByteArrayToImage(byteArray);

//        //    //Then
//        //    Assert.IsInstanceOf<Image>(result);
//        //}
//    }
//}
