using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Reflection;

namespace Enoch.Api.Base
{
    [Produces("application/json")]
    [Authorize("Bearer")]
    [ApiController]
    [DisableRequestSizeLimit]
    [EnableCors("SiteCorsPolicy")]
    public class ApiBaseController : Controller
    {
        //[AllowAnonymous]
        //[HttpGet("ping")]
        //public IActionResult Ping()
        //{
        //    var myName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
        //    return Ok($"{DateTime.Now}, {myName} it's alive!");
        //}
    }
}
