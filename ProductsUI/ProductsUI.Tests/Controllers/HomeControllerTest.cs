using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProductsUI.Controllers;
using System;
using System.Web.Mvc;

namespace ProductsUI.Tests.Controllers
{
    [TestClass]
    public class HomeControllerTest
    {
        [TestMethod]
        [ExpectedException(typeof(NullReferenceException))]
        public void Index()
        {
            // Arrange
            HomeController controller = new HomeController();            

            // Act
            ViewResult result = controller.Index() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void About()
        {
            // Arrange
            HomeController controller = new HomeController();

            // Act
            ViewResult result = controller.About() as ViewResult;

            // Assert
            Assert.AreEqual("Products Demo page.", result.ViewBag.Message);
        }        
    }
}
