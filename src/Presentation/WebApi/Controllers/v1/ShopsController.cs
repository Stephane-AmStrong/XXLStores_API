using Application.Features.Shops.Commands.Create;
using Application.Features.Shops.Commands.Delete;
using Application.Features.Shops.Commands.Update;
using Application.Features.Shops.Queries.GetById;
using Application.Features.Shops.Queries.GetPagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Serilog;


namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [Authorize]
    public class ShopsController : BaseApiController
    {
        //readonly IDiagnosticContext _diagnosticContext;
        public ShopsController()
        {
        }


        // GET: api/<controller>
        /// <summary>
        /// return shops that matche the criteria
        /// </summary>
        /// <param name="shopsQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "shop.read.policy")]
        public async Task<IActionResult> Get([FromQuery] GetShopsQuery shopsQuery)
        {
            var shops = await Mediator.Send(shopsQuery);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(shops.MetaData));
            return Ok(shops.PagedList);
        }


        // GET api/<controller>/5
        /// <summary>
        /// Retreives a specific Shop.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "shop.read.policy")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetShopByIdQuery { Id = id }));
        }


        // POST api/<controller>
        /// <summary>
        /// Creates a Shop.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created Shop</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost]
        [Authorize(Policy = "shop.write.policy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateShopCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        // PUT api/<controller>/5
        /// <summary>
        /// Update a specific Shop.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "shop.write.policy")]
        public async Task<IActionResult> Put(Guid id, UpdateShopCommand command)
        {
            command.Id = id;
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Deletes a specific Shop.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "shop.manage.policy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteShopCommand { Id = id }));
        }
    }
}
