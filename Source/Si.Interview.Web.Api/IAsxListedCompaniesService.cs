using System.Threading.Tasks;

namespace Si.Interview.Web.Api
{
    public interface IAsxListedCompaniesService
    {
        Task<AsxListedCompany> GetByAsxCode(string asxCode);
    }
}
