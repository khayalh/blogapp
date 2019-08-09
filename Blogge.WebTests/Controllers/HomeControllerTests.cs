using System;
using System.Web.Mvc;
using Blogge.Interfaces.Builders.Model;
using Blogge.Models.EntityModels;
using Blogge.Models.ViewModels;
using Blogge.Web.Controllers;
using Moq;
using NUnit.Framework;

namespace Blogge.WebTests.Controllers
{
    [TestFixture]
    public class HomeControllerTests
    {

        private Mock<IPostBuilder> _postBuilder;
        private HomeController _homeController;
        private HomepageViewModel _homepageModel;


        [SetUp]
        public void Setup()
        {
            _postBuilder = new Mock<IPostBuilder>();
            _homeController = new HomeController(_postBuilder.Object);

            var post = new Post()
            {
                Id = 1,
                Content = "kek"
            };

        }

        [Test]
        public void When_Index_Returns_IndexView()
        {
            //Given
            _homepageModel = new HomepageViewModel();
            _postBuilder.Setup(x => x.BuildHomepageViewModel()).Returns(_homepageModel);

            //When
            var result = _homeController.Index() as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<HomepageViewModel>(result.Model);
            Assert.AreEqual(String.Empty, result.ViewName);

        }

        [Test]
        public void When_Index_with_int_Returns_IndexView()
        {
            //Given
            _homepageModel = new HomepageViewModel();
            _postBuilder.Setup(x => x.BuildHomepageViewModel(It.IsAny<int>())).Returns(_homepageModel);

            //When
            var result = _homeController.Index(5) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<HomepageViewModel>(result.Model);
            Assert.AreEqual("Index", result.ViewName);
        }

        [Test]
        public void When_Index_with_SearchViewModel_Returns_IndexView()
        {
            //Given
            _homepageModel = new HomepageViewModel();
            _postBuilder.Setup(x => x.BuildHomepageViewModel(It.IsAny<string>())).Returns(_homepageModel);
            var searchModel = new SearchViewModel();
            //When
            var result = _homeController.Index(searchModel) as ViewResult;
            //Then
            Assert.NotNull(result);
            Assert.IsInstanceOf<HomepageViewModel>(result.Model);
            Assert.AreEqual("Index", result.ViewName);
        }
    }
}
