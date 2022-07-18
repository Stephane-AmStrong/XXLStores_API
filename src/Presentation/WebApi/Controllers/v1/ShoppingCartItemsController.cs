using Application.Features.ShoppingCartItems.Commands.Create;
using Application.Features.ShoppingCartItems.Commands.Delete;
using Application.Features.ShoppingCartItems.Commands.Update;
using Application.Features.ShoppingCartItems.Queries.GetById;
using Application.Features.ShoppingCartItems.Queries.GetPagedList;
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
    public class ShoppingCartItemsController : BaseApiController
    {
        //readonly IDiagnosticContext _diagnosticContext;
        public ShoppingCartItemsController()
        {
        }


        /// <summary>
        /// return shoppingCartItems that matche the criteria
        /// </summary>
        /// <param name="shoppingCartItemsQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "shoppingCartItem.read.policy")]
        public async Task<IActionResult> Get([FromQuery] GetShoppingCartItemsQuery shoppingCartItemsQuery)
        {
            var shoppingCartItems = await Mediator.Send(shoppingCartItemsQuery);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(shoppingCartItems.MetaData));
            return Ok(shoppingCartItems.PagedList);
        }


        /// <summary>
        /// Retreives a specific ShoppingCartItem.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "shoppingCartItem.read.policy")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetShoppingCartItemByIdQuery { Id = id }));
        }


        /// <summary>
        /// Creates a ShoppingCartItem.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created ShoppingCartItem</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost]
        [Authorize(Policy = "shoppingCartItem.write.policy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateShoppingCartItemCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Update a specific ShoppingCartItem.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "shoppingCartItem.write.policy")]
        public async Task<IActionResult> Put(Guid id, UpdateShoppingCartItemCommand command)
        {
            command.Id = id;
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Deletes a specific ShoppingCartItem.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "shoppingCartItem.manage.policy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteShoppingCartItemCommand { Id = id });
            return NoContent();
        }
    }
}
