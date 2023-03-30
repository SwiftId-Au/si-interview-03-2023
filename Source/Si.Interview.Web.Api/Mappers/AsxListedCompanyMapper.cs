using Si.Interview.Web.Api.Models;
using System;

namespace Si.Interview.Web.Api.Mappers
{
    public static class AsxListedCompanyMapper
    {
        public static AsxListedCompanyResponse MapToAsxListedCompanyResponse(AsxListedCompany company)
        {
            if(company is null)
            {
                throw new ArgumentNullException($"{nameof(AsxListedCompany)} cannot be null.");
            }

            return new AsxListedCompanyResponse
            {
                AsxCode = company.AsxCode,
                CompanyName = company.CompanyName
            };
        }
    }
}
