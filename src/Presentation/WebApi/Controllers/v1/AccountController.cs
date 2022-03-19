using Application.Features.Account.Commands.Authenticate;
using Application.Features.Account.Commands.RegisterUser;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [ApiController]
    [AllowAnonymous]
    public class AccountController : BaseApiController
    {



        // POST api/<controller>
        /// <summary>
        /// Authenticate a User.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>An authenticated User</returns>
        /// <response code="200">The authenticated User</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost("authenticate")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Authenticate(AuthenticationCommand command)
        {
            command.IpAddress = GenerateIPAddress();
            return Ok(await Mediator.Send(command));
        }



        // POST api/<controller>
        /// <summary>
        /// Creates a User Account.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created User Account</returns>
        /// <response code="201">Returns the newly User Account</response>
        /// <response code="400">If the command is null</response>           
        [HttpPost("Register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            return Ok(await Mediator.Send(command));
        }





        private string GenerateIPAddress()
        {
            if (Request.Headers.ContainsKey("X-Forwarded-For"))
                return Request.Headers["X-Forwarded-For"];
            else
                return HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
        }
    }
}
