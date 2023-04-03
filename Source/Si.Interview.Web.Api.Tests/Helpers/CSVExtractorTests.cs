using Microsoft.VisualStudio.TestTools.UnitTesting;
using Si.Interview.Web.Api.Helpers;
using Si.Interview.Web.Api.Models;
using System.Text;

namespace Si.Interview.Web.Api.UnitTests.Helpers
{
    [TestClass]
    public class CSVExtractorTests
    {
        [TestMethod]
        public async Task ExtractCSVData_Returns_ListOfAsxListedCompany()
        {
            // Arrange
            var csvData = "ASX listed companies as at Mon Dec 03 11:00:16 AEDT 2018\"\n" +
                          "\n" +
                          "\"Company name\",\"ASX Code\",\"GICS industry group\"\n" + 
                          "\"Company 1\",\"C1\",\"Industry 1\"\n" +
                          "\"Company 2\",\"C2\",\"Industry 2\"\n" +
                          "\"Company 3\",\"C3\",\"Industry 3\"\n";

            using var stream = new MemoryStream(Encoding.UTF8.GetBytes(csvData));
            using var httpContent = new StreamContent(stream);

            // Act
            var companies = await CSVExtractor.ExtractCSVData(httpContent);

            // Assert
            Assert.IsNotNull(companies);
            Assert.IsInstanceOfType(companies, typeof(List<AsxListedCompany>));
            Assert.AreEqual(3, companies.Count);

            Assert.AreEqual("Company 1", companies[0].CompanyName);
            Assert.AreEqual("C1", companies[0].AsxCode);
            Assert.AreEqual("Industry 1", companies[0].GicsIndustryGroup);

            Assert.AreEqual("Company 2", companies[1].CompanyName);
            Assert.AreEqual("C2", companies[1].AsxCode);
            Assert.AreEqual("Industry 2", companies[1].GicsIndustryGroup);

            Assert.AreEqual("Company 3", companies[2].CompanyName);
            Assert.AreEqual("C3", companies[2].AsxCode);
            Assert.AreEqual("Industry 3", companies[2].GicsIndustryGroup);
        }

        [TestMethod]
        public async Task ExtractCSVData_ShouldThrowException_WhenContentIsEmpty()
        {
            // Arrange
            var content = new StringContent("", Encoding.UTF8, "text/csv");

            // Act & Assert
            Assert.ThrowsExceptionAsync<InvalidDataException>(async () =>
            {
                var response = await CSVExtractor.ExtractCSVData(content);
            });
        }
    }
}