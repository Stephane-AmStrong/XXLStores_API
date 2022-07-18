using Application.Features.ShoppingCarts.Commands.Create;
using Application.Features.ShoppingCarts.Commands.Delete;
using Application.Features.ShoppingCarts.Commands.Update;
using Application.Features.ShoppingCarts.Queries.GetById;
using Application.Features.ShoppingCarts.Queries.GetPagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;


namespace WebApi.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    public class ShoppingCartsController : BaseApiController
    {
        //readonly IDiagnosticContext _diagnosticContext;
        public ShoppingCartsController()
        {
        }


        /// <summary>
        /// return shoppingCarts that matche the criteria
        /// </summary>
        /// <param name="shoppingCartsQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "shoppingCart.read.policy")]
        public async Task<IActionResult> Get([FromQuery] GetShoppingCartsQuery shoppingCartsQuery)
        {
            var shoppingCarts = await Mediator.Send(shoppingCartsQuery);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(shoppingCarts.MetaData));
            return Ok(shoppingCarts.PagedList);
        }


        /// <summary>
        /// Retreives a specific ShoppingCart.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "shoppingCart.read.policy")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetShoppingCartByIdQuery { Id = id }));
        }


        /// <summary>
        /// Creates a ShoppingCart.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created ShoppingCart</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost]
        [Authorize(Policy = "shoppingCart.write.policy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateShoppingCartCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Update a specific ShoppingCart.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "shoppingCart.write.policy")]
        public async Task<IActionResult> Put(Guid id, UpdateShoppingCartCommand command)
        {
            command.Id = id;
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Deletes a specific ShoppingCart.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "shoppingCart.manage.policy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteShoppingCartCommand { Id = id });
            return NoContent();
        }
    }
}
