using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace Enoch.CrossCutting.Controller
{
    //    [Produces("application/json")]
    //    [Authorize()]
    //    [ApiController]
    //    [DisableRequestSizeLimit]
    //    [EnableCors("SiteCorsPolicy")]

    //    public class ApiBaseController : Controller
    //    {
    //        //[AllowAnonymous]
    //        //[HttpGet("ping")]
    //        //public IActionResult Ping()
    //        //{
    //        //    var myName = Path.GetFileNameWithoutExtension(Assembly.GetEntryAssembly().Location);
    //        //    return Ok($"{DateTime.Now}, {myName} it's alive!");
    //        //}

    //        //[AllowAnonymous]
    //        //[HttpGet("garbage")]
    //        //public IActionResult Garbage()
    //        //{
    //        //    var result = GCSettings.IsServerGC ? "server" : "workstation";
    //        //    return Ok($"{DateTime.Now},-- {result} <--!");
    //        //}

    //        //protected IActionResult Success(object _data = null)
    //        //{
    //        //    return ReponseMessage(HttpStatusCode.OK, _data, validToken: true);
    //        //}

    //        //protected IActionResult SuccessIntegration(object _data = null)
    //        //{
    //        //    return ReponseMessage(HttpStatusCode.OK, _data, validToken: false);
    //        //}

    //        //protected IActionResult ErrorIntegration(object errorMessage)
    //        //{
    //        //    return ReponseMessage(HttpStatusCode.BadRequest, _data: errorMessage);
    //        //}

    //        //protected IActionResult Error(object errorMessage)
    //        //{
    //        //    return ReponseMessage(HttpStatusCode.BadRequest, _data: errorMessage, validToken: true);
    //        //}

    //        //protected IActionResult Token(object data, string token = null, bool validToken = false)
    //        //{
    //        //    return ReponseMessage(HttpStatusCode.OK, data, token, validToken);
    //        //}

    //        //protected IActionResult ErrorToken(object data, string token = null, bool validToken = false)
    //        //{
    //        //    return ReponseMessage(HttpStatusCode.BadRequest, data, token, validToken);
    //        //}

    //        //private IActionResult ReponseMessage(HttpStatusCode status, object _data = null, string token = null, bool validToken = false)
    //        //{
    //        //    try
    //        //    {
    //        //        var tokenString = token;
    //        //        if (validToken && string.IsNullOrEmpty(tokenString))
    //        //        {
    //        //            token = HttpContext.Request.Headers["Authorization"];
    //        //            try
    //        //            {
    //        //                var dataToken = token.Token();
    //        //                if (dataToken != null)
    //        //                {
    //        //                    var interval = dataToken.IntervalToken;
    //        //                    if (interval >= 30 && interval <= 60)
    //        //                        tokenString = Encryption.CreateToken(Convert.ToInt32(dataToken.Name),
    //        //                            Convert.ToInt32(dataToken.Actor),
    //        //                            dataToken.NameIdentifier, dataToken.Authentication);
    //        //                }
    //        //            }
    //        //            catch (Exception e)
    //        //            {
    //        //            }
    //        //        }

    //        //        switch (status)
    //        //        {
    //        //            case HttpStatusCode.BadRequest:
    //        //                return BadRequest(new
    //        //                {
    //        //                    error = _data,
    //        //                    token = tokenString
    //        //                });
    //        //            case HttpStatusCode.OK:
    //        //                return Ok(new
    //        //                {
    //        //                    data = _data,
    //        //                    token = tokenString
    //        //                });
    //        //        }

    //        //        return NoContent();
    //        //    }
    //        //    catch (Exception e)
    //        //    {
    //        //        return BadRequest();
    //        //    }
    //        //}
    //    }
}
