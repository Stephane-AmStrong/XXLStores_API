using Application.Features.InventoryLevels.Commands.Create;
using Application.Features.InventoryLevels.Commands.Delete;
using Application.Features.InventoryLevels.Commands.Update;
using Application.Features.InventoryLevels.Queries.GetById;
using Application.Features.InventoryLevels.Queries.GetPagedList;
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
    public class InventoryLevelsController : BaseApiController
    {
        //readonly IDiagnosticContext _diagnosticContext;
        public InventoryLevelsController()
        {
        }


        /// <summary>
        /// return inventoryLevels that matche the criteria
        /// </summary>
        /// <param name="inventoryLevelsQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "inventoryLevel.read.policy")]
        public async Task<IActionResult> Get([FromQuery] GetInventoryLevelsQuery inventoryLevelsQuery)
        {
            var inventoryLevels = await Mediator.Send(inventoryLevelsQuery);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(inventoryLevels.MetaData));
            return Ok(inventoryLevels.PagedList);
        }


        /// <summary>
        /// Retreives a specific InventoryLevel.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "inventoryLevel.read.policy")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetInventoryLevelByIdQuery { Id = id }));
        }


        /// <summary>
        /// Creates a InventoryLevel.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created InventoryLevel</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost]
        [Authorize(Policy = "inventoryLevel.write.policy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateInventoryLevelCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Update a specific InventoryLevel.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "inventoryLevel.write.policy")]
        public async Task<IActionResult> Put(Guid id, UpdateInventoryLevelCommand command)
        {
            command.Id = id;
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Deletes a specific InventoryLevel.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "inventoryLevel.manage.policy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeleteInventoryLevelCommand { Id = id });
            return NoContent();
        }
    }
}
