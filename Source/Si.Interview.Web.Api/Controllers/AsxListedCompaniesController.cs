using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Si.Interview.Web.Api.Mappers;
using Si.Interview.Web.Api.Models;
using Si.Interview.Web.Api.Services;

namespace Si.Interview.Web.Api.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AsxListedCompaniesController : ControllerBase
    {
        private IAsxListedCompaniesService _asxListedCompaniesService;

        public AsxListedCompaniesController(IAsxListedCompaniesService asxListedCompaniesService)
        {
            _asxListedCompaniesService = asxListedCompaniesService ?? throw new ArgumentNullException(nameof(asxListedCompaniesService));
        }

        [HttpGet]
        public async Task<ActionResult<AsxListedCompanyResponse>> Get(string asxCode)
        {
            try
            {
                if (string.IsNullOrEmpty(asxCode))
                {
                    throw new ArgumentNullException($"{nameof(asxCode)} cannot be null.");
                }

                var asxListedCompany = await _asxListedCompaniesService.GetByAsxCode(asxCode);

                return Ok(AsxListedCompanyMapper.MapToAsxListedCompanyResponse(asxListedCompany));
            }
            catch (ArgumentNullException ex)
            {
                return new StatusCodeResult(StatusCodes.Status400BadRequest);
            }
            catch (KeyNotFoundException ex)
            {
                return new StatusCodeResult(StatusCodes.Status404NotFound);
            }
            catch (Exception ex)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
