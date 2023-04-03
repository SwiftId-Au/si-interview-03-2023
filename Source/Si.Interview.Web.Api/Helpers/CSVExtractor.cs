using Si.Interview.Web.Api.Constants;
using Si.Interview.Web.Api.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Si.Interview.Web.Api.Helpers
{
    public static class CSVExtractor
    {
        public static async Task<List<AsxListedCompany>> ExtractCSVData(HttpContent content)
        {
            try
            {
                var companies = new List<AsxListedCompany>();

                using Stream stream = await content.ReadAsStreamAsync();

                if(stream == null || stream.Length <= 0)
                {
                    throw new InvalidDataException("The CSV file is empty.");
                }

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
