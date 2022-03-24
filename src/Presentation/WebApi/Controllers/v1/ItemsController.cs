using Application.Features.Items.Commands.Create;
using Application.Features.Items.Commands.Delete;
using Application.Features.Items.Commands.Update;
using Application.Features.Items.Queries.GetById;
using Application.Features.Items.Queries.GetPagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Serilog;


namespace WebApi.Controllers.v1
{
    [Authorize]
    [ApiVersion("1.0")]
    public class ItemsController : BaseApiController
    {
        //readonly IDiagnosticContext _diagnosticContext;
        public ItemsController()
        {
        }


        // GET: api/<controller>
        /// <summary>
        /// return items that matche the criteria
        /// </summary>
        /// <param name="itemsQuery"></param>
        /// <returns></returns>
        [HttpGet]
        //[Authorize(Policy = "item.read.policy")]
        public async Task<IActionResult> Get([FromQuery] GetItemsQuery itemsQuery)
        {
            var items = await Mediator.Send(itemsQuery);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(items.MetaData));
            return Ok(items.PagedList);
        }


        // GET api/<controller>/5
        /// <summary>
        /// Retreives a specific Item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        //[Authorize(Policy = "item.read.policy")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetItemByIdQuery { Id = id }));
        }


        // POST api/<controller>
        /// <summary>
        /// Creates a Item.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created Item</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost]
        //[Authorize(Policy = "item.write.policy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateItemCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        // PUT api/<controller>/5
        /// <summary>
        /// Update a specific Item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        //[Authorize(Policy = "item.write.policy")]
        public async Task<IActionResult> Put(Guid id, UpdateItemCommand command)
        {
            command.Id = id;
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Deletes a specific Item.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        //[Authorize(Policy = "item.manage.policy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteItemCommand { Id = id }));
        }
    }
}
