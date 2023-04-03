using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Si.Interview.Web.Api.Helpers;
using Si.Interview.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Si.Interview.Web.Api.Services
{
    public class AsxListedCompaniesService : IAsxListedCompaniesService
    {
        private string cacheKey = nameof(AsxListedCompaniesService) + DateTime.Today.ToString("yyyyMMdd");
        private readonly AsxSettings _options;
        private readonly IMemoryCache _memoryCache;
        private readonly HttpClient _httpClient;

        public AsxListedCompaniesService(IOptions<AsxSettings> options, IMemoryCache memoryCache, HttpClient httpClient)
        {
            _options = options?.Value ?? throw new ArgumentNullException(nameof(IOptions<AsxSettings>));
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

                using (var response = await _httpClient.GetAsync(_options.ListedSecuritiesCsvUrl))
                {
                    response.EnsureSuccessStatusCode();

                    companies = await CSVExtractor.ExtractCSVData(response.Content);

                    var cacheOptions = new MemoryCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(_options.CacheDurationInHours)
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
    }
}
