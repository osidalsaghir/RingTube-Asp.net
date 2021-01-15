using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Moq;
using NUnit.Framework;
using RingTube.Controllers;

namespace RingTube.Test
{
    [TestFixture]
    public class UnitTest
    {
        [TestCase]
        public void TestHomeController()
        {
            HomeController obj = new HomeController();  //HoneTest
        
            var result = obj.Index() as ViewResult;
            Assert.IsEmpty(result.ViewName);
    

        }
        [TestCase]
        public void TestProductController()
        {
            

            ProductsController pc = new ProductsController(); //ProductsTest
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("osid.alsagheer@gmail.com");
            pc.ControllerContext = mock.Object;

            var result = pc.SingleProduct(23) as ViewResult;
            Assert.AreEqual("", result.ViewName);

            result = pc.Index() as ViewResult;
            Assert.AreEqual("", result.ViewName);

            int[] cats = { 9 };
            result = pc.Filter(cats) as ViewResult ;
            Assert.AreEqual("~/Views/Products/Index.cshtml", result.ViewName);

            
            result = pc.Cart() as ViewResult;
            Assert.AreEqual("", result.ViewName);

            result = pc.Fav() as ViewResult;
            Assert.AreEqual("", result.ViewName);

            result = pc.Purchase() as ViewResult;
            Assert.AreEqual("", result.ViewName);




        }
        [TestCase]
        public void TestAdminPanelController()
        {
            AdminPanelController obj = new AdminPanelController();  //HoneTest
            var mock = new Mock<ControllerContext>();
            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("osid.alsagheer@gmail.com");
            obj.ControllerContext = mock.Object;


            var result = obj.Index() as ViewResult;
            Assert.IsEmpty(result.ViewName);

            result = obj.EditCategory(9) as ViewResult;
            Assert.IsEmpty(result.ViewName);

            result = obj.EditTag(8) as ViewResult;
            Assert.IsEmpty(result.ViewName);

            result = obj.Users() as ViewResult;
            Assert.IsEmpty(result.ViewName);

            result = obj.Table("Hi from test") as ViewResult;
            Assert.IsEmpty(result.ViewName);


        }
        [TestCase]
        public void TestUserController()
        {
            UserController obj = new UserController();  //HoneTest
            var mock = new Mock<ControllerContext>();
           

            

            var result = obj.SignUp() as ViewResult;
            Assert.IsEmpty(result.ViewName);

            mock.SetupGet(x => x.HttpContext.User.Identity.Name).Returns("osid.alsagheer@gmail.com");
            obj.ControllerContext = mock.Object;

            result = obj.Index() as ViewResult;
            Assert.IsEmpty(result.ViewName);

            result = obj.UserProfile() as ViewResult;
            Assert.IsEmpty(result.ViewName);



        }
    }
}