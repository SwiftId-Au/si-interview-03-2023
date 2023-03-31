using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Si.Interview.Web.Api.Constants;
using Si.Interview.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Si.Interview.Web.Api.Services
{
    public class AsxListedCompaniesService : IAsxListedCompaniesService
    {
        private string cacheKey = "AsxListedCompanies-" + DateTime.Today.ToString("yyyyMMdd");
        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;

        public AsxListedCompaniesService(IConfiguration configuration, IMemoryCache memoryCache, HttpClient httpClient)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(IConfiguration));
            _memoryCache = memoryCache ?? throw new ArgumentNullException(nameof(IMemoryCache));
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(HttpClient)); ;
        }

        public async Task<AsxListedCompany> GetByAsxCode(string asxCode)
        {
            try
            {
                if (string.IsNullOrEmpty(asxCode))
                {
                    throw new ArgumentNullException($"{nameof(asxCode)} cannot be null.");
                }

                var result = await GetAsxListedCompaniesAsync();

                return result.FirstOrDefault(x => x.AsxCode == asxCode) ?? throw new KeyNotFoundException($"Record having {asxCode} is not found.");
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<AsxListedCompany>> GetAsxListedCompaniesAsync()
        {
            try
            {
                if (_memoryCache.TryGetValue(cacheKey, out List<AsxListedCompany> companies))
                {
                    return companies;
                }

                var url = _configuration.GetValue<string>(AppConstants.AsxURLPath);

                using (var response = await _httpClient.GetAsync(url))
                {
                    response.EnsureSuccessStatusCode();

                    companies = await ExtractCSVData(response.Content);

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromDays(1)
                    };

                    _memoryCache.Set(cacheKey, companies, cacheOptions);

                    return companies;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        private async Task<List<AsxListedCompany>> ExtractCSVData(HttpContent content)
        {
            try
            {
                var companies = new List<AsxListedCompany>();

                using Stream stream = await content.ReadAsStreamAsync();

                using StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                int currentRow = 1;

                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (currentRow >= AppConstants.RowStart)
                    {
                        var values = line.Split(',');

                        if (values is not null)
                        {
                            companies.Add(new AsxListedCompany
                            {
                                CompanyName = values[0].ToString().Replace("\"", "").Replace("\\", ""),
                                AsxCode = values[1].ToString().Replace("\"", "").Replace("\\", ""),
                                GicsIndustryGroup = values[2].ToString().Replace("\"", "").Replace("\\", "")
                            });
                        }
                    }

                    currentRow++;
                }

                return companies;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
