using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.Auth;
using Enoch.Domain.Services.Auth.Dto;
using Microsoft.AspNetCore.Mvc;

namespace Enoch.Api.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;
        private readonly INotification _notification;

        public AuthController(IAuthService authService, INotification notification)
        {
            _authService = authService;
            _notification = notification;
        }

        [HttpPost("authenticate")]
        public IActionResult Authenticate(AuthDto auth)
        {
            var token = _authService.GenerateToken(auth);
            if (token == null)
                return BadRequest(_notification.GetNotifications());

            return Ok(token);
        }
    }
}
