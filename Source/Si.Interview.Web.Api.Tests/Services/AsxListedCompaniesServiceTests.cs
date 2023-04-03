using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Si.Interview.Web.Api.Models;
using Si.Interview.Web.Api.Services;

namespace Si.Interview.Web.Api.Tests.Services
{
    [TestClass]
    public class AsxListedCompaniesServiceTests
    {
        [TestMethod]
        public async Task ServiceThrowsExceptionWhenConstructorsAreNull()
        {
            // Act & AssertW
            Assert.ThrowsException<ArgumentNullException>(() => new AsxListedCompaniesService(null, null, null));
        }

        [TestMethod]
        public async Task ServiceThrowsExceptionWhenParameterIsNull()
        {
            // Arrange
            var _optionsMock = new Mock<IOptions<AsxSettings>>();
            _optionsMock.Setup(x => x.Value).Returns(new AsxSettings
            {
                ListedSecuritiesCsvUrl = "https://example.com",
                CacheDurationInHours = 1
            });

            var _cacheMock = new Mock<IMemoryCache>();
            var _httpClient = new HttpClient();
            var service = new AsxListedCompaniesService(_optionsMock.Object, _cacheMock.Object, _httpClient);

            // Act & Assert
            Assert.ThrowsExceptionAsync<ArgumentNullException>(async () =>
            {
                var response = await service.GetByAsxCode(string.Empty);
            });
        }
    }
}
