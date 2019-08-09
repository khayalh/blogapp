//using System;
//using System.Collections.Generic;
//using Blogge.Core.Controllers;
//using Blogge.Core.Interfaces.Controllers;
//using Blogge.Core.Models;
//using Blogge.Interfaces.Repositories;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using Moq;
//using NUnit.Framework;

//namespace Blogge.Tests.Controllers
//{
//    [TestFixture]
//    public class PostDBControllerTests
//    {
//        PostDBController _postDBController;

//        [SetUp]
//        public void TestSetup()
//        {
//            _postDBController = new PostDBController();
//            Mock<IBlogRepository> repoMock = new Mock<IBlogRepository>();
//            repoMock.Setup(m => m.GetPosts()).Returns(new List<Post>
//            {
//                new Post {Id = 0, Author = "kek", Title = "Post1", Content = "Post content", PostedAt = DateTime.Now},
//                new Post {Id = 1, Author = "shrek", Title = "Post2", Content = "Post content", PostedAt = DateTime.MaxValue},
//                new Post {Id = 2, Author = "kok", Title = "Post3", Content = "Post content", PostedAt = DateTime.MinValue},
//                new Post {Id = 3, Author = "shrok", Title = "Post4", Content = "Post content", PostedAt = DateTime.Now},
//            });
//        }

//        [TestMethod]
//        public void When_AddPost_Then_adds_post_to_database()
//        {
//            //given
//            //when
//            var result = _postDBController.AddPost
//        }
//    }
//}
