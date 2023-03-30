using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Si.Interview.Web.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AsxListedCompaniesController : ControllerBase
    {
        private IAsxListedCompaniesService _asxListedCompaniesService;

        public AsxListedCompaniesController(IAsxListedCompaniesService asxListedCompaniesService)
        {
            _asxListedCompaniesService = asxListedCompaniesService;
        }

        [HttpGet]
        public async Task<ActionResult<AsxListedCompanyResponse>> Get(string asxCode)
        {
            var asxListedCompany = await _asxListedCompaniesService.GetByAsxCode(asxCode);

            return asxListedCompany;
        }
    }
}
