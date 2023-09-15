using Microsoft.AspNetCore.Mvc;
using SellManagement.Api.Models;
using SellManagement.Api.Services;
using SellManagement.Api.Messages;
using SellManagement.Api.Helpers;

namespace SellManagement.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : Controller
    {
        private IUserService _userService;
        UserOperator _userOperator;
        public LoginController(IUserService userService, UserOperator userOperator)
        {
            _userService = userService;
            _userOperator = userOperator;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthenticateRequest model)
        {
            var response = _userService.Authenticate(model);

            if (response == null)
                return BadRequest(new { message = VNIMessages.MSG_LOGIN_001 });

            return Ok(response);
        }

        [HttpPost("FetchMe")]
        public IActionResult FetchMe()
        {
            var loginUser = _userOperator.GetRequestUser();

            if (loginUser == null)
                return Unauthorized();

            var response = new AuthenticateResponse
            {
                Id = loginUser.Id,
                UserName = loginUser.UserName,
                UserRole = loginUser.UserRole,
            };
            return Ok(response);
        }
    }
}
