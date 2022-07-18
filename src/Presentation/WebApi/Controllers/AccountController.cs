﻿using Application.Features.Account.Commands.Authenticate;
using Application.Features.Account.Commands.ForgotPassword;
using Application.Features.Account.Commands.RefreshAccessToken;
using Application.Features.Account.Commands.RegisterUser;
using Application.Features.Account.Commands.ResetPassword;
using Application.Features.Account.Commands.RevokeAccessToken;
using Application.Features.Account.Queries.ConfirmEmail;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace WebApi.Controllers.v1
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {

        private IMediator _mediator;
        protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetService<IMediator>();

        // POST api/<controller>
        /// <summary>
        /// Authenticate a User.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>An authenticated User</returns>
        /// <response code="200">The authenticated User</response>
        /// <response code="400">If the command is null</response>            
        [AllowAnonymous, HttpPost("authenticate")]
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
        /// <response code="200">Returns the newly User Account</response>
        /// <response code="400">If the command is null</response>           
        [AllowAnonymous, HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register(RegisterUserCommand command)
        {
            command.Origin = Request.Headers["origin"];
            return Ok(await Mediator.Send(command));
        }



        // POST api/<controller>
        /// <summary>
        /// Confirm user's email.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Confirm a newly created User Account's email</returns>
        /// <response code="200">Returns confirmation success message</response>
        /// <response code="400">If the command is not valide</response>           
        [AllowAnonymous, HttpGet("confirm-email")]
        [ApiExplorerSettings(IgnoreApi = true)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ConfirmEmailAsync([FromQuery]ConfirmEmailQuery command)
        {
            return Ok(await Mediator.Send(command));
        }



        // POST api/<controller>
        /// <summary>
        /// Generate a Reset Token for a forgotten password.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Generate a Reset Token for a forgotten password</returns>
        /// <response code="200">Returns the generated password reset token</response>
        /// <response code="400">If the command is not valide</response>           
        [AllowAnonymous, HttpPost("forgot-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ForgotPasswordAsync(ForgotPasswordCommand command)
        {
            command.Origin = Request.Headers["origin"];
            return Ok(await Mediator.Send(command));
        }



        // POST api/<controller>
        /// <summary>
        /// Confirm user's email.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>Confirm a newly created User Account's email</returns>
        /// <response code="200">Returns confirmation success message</response>
        /// <response code="400">If the command is not valide</response>           
        [AllowAnonymous, HttpPost("reset-password")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> ResetPasswordAsync(ResetPasswordCommand command)
        {
            return Ok(await Mediator.Send(command));
        }



        /// <summary>
        /// Refreshes a Token.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created Token</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [AllowAnonymous, HttpPost("refresh-token")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Refresh(RefreshTokenCommand command)
        {
            command.IpAddress = GenerateIPAddress();
            return Ok(await Mediator.Send(command));
        }



        /// <summary>
        /// Revokes a Token.
        /// </summary>
        /// <returns>A newly created Token</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [Authorize, HttpPost("revoke-token")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Revoke()
        {
            var command = new RevokeTokenCommand 
            { 
                userId = User.FindFirstValue(ClaimTypes.NameIdentifier) 
            };
            //command.userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await Mediator.Send(command);
            return NoContent();
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
