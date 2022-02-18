using Enoch.CrossCutting.Notification;
using Enoch.Domain.Services.User;
using Enoch.Domain.Services.User.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Enoch.Api.Controllers
{
    [Route("api/user")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly INotification _notification;

        public UserController(IUserService userService, INotification notification)
        {
            _userService = userService;
            _notification = notification;
        }

        [HttpGet]
        public IActionResult Get(int? page = null)
        {
            var users = _userService.Get(out var total, page);

            return Ok(new { total, users });         
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            var users = _userService.GetById(id);

            return Ok(users);
        }

        [HttpPost]
        public IActionResult Post(UserDto user)
        {
            var userData = _userService.Post(user);

            if(userData <= 0 || _notification.Any())
                return BadRequest(_notification.GetNotifications());

            return Ok(userData);
        }

        [HttpPut]
        public IActionResult Put(UserDto user)
        {
            var userData = _userService.Put(user);

            if (_notification.Any())
                return BadRequest(_notification.GetNotifications());

            return Ok(userData);
        }
    }
}
