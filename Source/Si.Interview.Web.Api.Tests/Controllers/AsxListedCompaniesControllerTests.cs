using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Si.Interview.Web.Api.Controllers;
using Si.Interview.Web.Api.Models;
using Si.Interview.Web.Api.Services;

namespace Si.Interview.Web.Api.Tests.Controllers
{
    [TestClass]
    public class AsxListedCompaniesControllerTests
    {
        [TestMethod]
        public async Task Get_ReturnsOkResult()
        {
            // Arrange
            var mockService = new Mock<IAsxListedCompaniesService>();
            mockService.Setup(s => s.GetByAsxCode(It.IsAny<string>()))
                       .ReturnsAsync(new AsxListedCompany { AsxCode = "ABC", CompanyName = "Company", GicsIndustryGroup = "Group" });
            var controller = new AsxListedCompaniesController(mockService.Object);

            // Act
            var result = await controller.Get("ABC");

            // Assert
            Assert.IsNotNull(result);
            var okObjectResult = (OkObjectResult)result.Result;
            Assert.IsNotNull(okObjectResult);
            Assert.AreEqual(200, okObjectResult.StatusCode);
            Assert.IsNotNull(okObjectResult.Value);
            Assert.IsInstanceOfType(okObjectResult.Value, typeof(AsxListedCompanyResponse));
            var value = (AsxListedCompanyResponse)okObjectResult.Value;
            Assert.AreEqual("ABC", value.AsxCode);
            Assert.AreEqual("Company", value.CompanyName);
        }

        [TestMethod]
        public async Task Get_ReturnsNoValueWhenNotFound()
        {
            // Arrange
            var mockService = new Mock<IAsxListedCompaniesService>();
            mockService.Setup(s => s.GetByAsxCode(It.IsAny<string>()))
                       .ThrowsAsync(new KeyNotFoundException());
            var controller = new AsxListedCompaniesController(mockService.Object);

            // Act
            var result = await controller.Get("ABC");

            // Assert
            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
            var statusCodeResult = (StatusCodeResult)result.Result;
            Assert.AreEqual(404, statusCodeResult.StatusCode);
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public async Task Get_ReturnsNoValueWhenArgumentIsNull()
        {
            // Arrange
            var mockService = new Mock<IAsxListedCompaniesService>();
            mockService.Setup(s => s.GetByAsxCode(It.IsAny<string>()))
                       .ThrowsAsync(new KeyNotFoundException());
            var controller = new AsxListedCompaniesController(mockService.Object);

            // Act
            var result = await controller.Get(string.Empty);
            var statusCodeResult = (StatusCodeResult)result.Result;

            // Assert
            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
            Assert.AreEqual(400, statusCodeResult.StatusCode);
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public async Task Get_ReturnsNoValueWhenExceptionIsThrownInService()
        {
            // Arrange
            var mockService = new Mock<IAsxListedCompaniesService>();
            mockService.Setup(s => s.GetByAsxCode(It.IsAny<string>()))
                       .ThrowsAsync(new Exception());
            var controller = new AsxListedCompaniesController(mockService.Object);

            // Act
            var result = await controller.Get("ABC");
            var statusCodeResult = (StatusCodeResult)result.Result;

            // Assert
            Assert.IsNotNull(result.Result);
            Assert.IsInstanceOfType(result.Result, typeof(StatusCodeResult));
            Assert.AreEqual(500, statusCodeResult.StatusCode);
            Assert.IsNull(result.Value);
        }

        [TestMethod]
        public async Task ControllerThrowsExceptionWhenConstructorIsNull()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new AsxListedCompaniesController(null));
        }
    }
}
