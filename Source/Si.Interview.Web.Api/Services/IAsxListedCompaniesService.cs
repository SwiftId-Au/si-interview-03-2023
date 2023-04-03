using Si.Interview.Web.Api.Models;
using System.Threading.Tasks;

namespace Si.Interview.Web.Api.Services
{
    public interface IAsxListedCompaniesService
    {
        Task<AsxListedCompany> GetByAsxCode(string asxCode);
    }
}
