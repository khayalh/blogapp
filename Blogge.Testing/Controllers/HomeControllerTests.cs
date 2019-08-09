//using System;
//using System.Collections.Generic;
//using Blogge.Interfaces.Repositories;
//using Blogge.Models;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using NUnit.Framework;

//namespace Blogge.Testing.Controllers
//{
//    [TestClass]
//    public class HomeControllerTests
//    {

//        private readonly Mock<IBlogRepository> _allPosts;
//        private readonly Controller HomeController;

//        public HomeControllerTests()
//        {
//            _allPosts = new Mock<IBlogRepository>();
//            HomeController = new Controller();
//        }

//        [SetUp]
//        public void Setup()
//        {
//            _allPosts.Setup(x => x.GetPosts()).Returns(new List<Post>()); 
//        }

//        [TestMethod]
//        public void TestMethod1()
//        {
//        }
//    }
//}
