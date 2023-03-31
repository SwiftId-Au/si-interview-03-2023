using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Si.Interview.Web.Api.Services;

namespace Si.Interview.Web.Api.Tests.Services
{
    [TestClass]
    public class AsxListedCompaniesServiceTests
    {
        [TestMethod]
        public async Task ServiceThrowsExceptionWhenConstructorsAreNull()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => new AsxListedCompaniesService(null, null, null));
        }

        [TestMethod]
        public async Task ServiceThrowsExceptionWhenParameterIsNull()
        {
            // Arrange
            var _configMock = new Mock<IConfiguration>();
            var _cacheMock = new Mock<IMemoryCache>();
            var _httpClient = new HttpClient();
            var service = new AsxListedCompaniesService(_configMock.Object, _cacheMock.Object, _httpClient);

            // Act & Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var response = await service.GetByAsxCode(string.Empty);
            });
        }
    }
}
