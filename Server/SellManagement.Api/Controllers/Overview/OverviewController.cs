using Microsoft.AspNetCore.Mvc;
using SellManagement.Api.Models;
using SellManagement.Api.Services;
using SellManagement.Api.Functions;
using SellManagement.Api.Controllers.Overview;
using SellManagement.Api.Messages;
using System.Threading.Tasks;

namespace SellManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]

    public class OverviewController : Controller
    {
        IOverviewFunction _overviewFunction;
        public OverviewController(IOverviewFunction overviewFunction)
        {
            _overviewFunction = overviewFunction;
        }

        [HttpPost("GetOverviewInfo")]
        [Authorize]
        public async Task<IActionResult> GetOverviewInfo()
        {
            var response = new GetOverviewInfoResponse
            {
                OverviewData = await _overviewFunction.GetOverviewInfo()
            };
            return Ok(response);
        }

        
    }
}
