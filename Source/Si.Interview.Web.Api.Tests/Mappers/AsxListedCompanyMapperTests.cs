using Microsoft.VisualStudio.TestTools.UnitTesting;
using Si.Interview.Web.Api.Mappers;
using Si.Interview.Web.Api.Models;

namespace Si.Interview.Web.Api.Tests.Mappers
{
    [TestClass]
    public class AsxListedCompanyMapperTests
    {
        [TestMethod]
        public void Map_ReturnsCorrectModel()
        {
            // Arrange
            var entity = new AsxListedCompany
            {
                AsxCode = "ABC",
                CompanyName = "Company",
                GicsIndustryGroup = "Group"
            };

            // Act
            var response = AsxListedCompanyMapper.MapToAsxListedCompanyResponse(entity);

            // Assert
            Assert.IsNotNull(response);
            Assert.AreEqual(response.AsxCode, entity.AsxCode);
            Assert.AreEqual(response.CompanyName, entity.CompanyName);
        }

        [TestMethod]
        public void Map_ThrowsExceptionOnNull()
        {
            // Act & Assert
            Assert.ThrowsException<ArgumentNullException>(() => AsxListedCompanyMapper.MapToAsxListedCompanyResponse(null));
        }
    }
}
