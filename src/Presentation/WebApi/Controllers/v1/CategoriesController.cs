using Application.Features.Categories.Commands.Create;
using Application.Features.Categories.Commands.Delete;
using Application.Features.Categories.Commands.Update;
using Application.Features.Categories.Queries.GetById;
using Application.Features.Categories.Queries.GetPagedList;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Serilog;


namespace WebApi.Controllers.v1
{
    [ApiVersion("1.0")]
    [AllowAnonymous]
    public class CategoriesController : BaseApiController
    {
        private readonly ILogger<CategoriesController> _logger;
        readonly IDiagnosticContext _diagnosticContext;
        public CategoriesController(ILogger<CategoriesController> logger)
        {
            _logger = logger;
        }

        // GET: api/<controller>
        /// <summary>
        /// return categories that matche the criteria
        /// </summary>
        /// <param name="categoriesQuery"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get([FromQuery] GetCategoriesQuery categoriesQuery)
        {
            var categories = await Mediator.Send(categoriesQuery);

            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(categories.MetaData));

            _logger.LogInformation($"Returned all categories from database.");

            return Ok(categories.PagedList);
        }


        // GET api/<controller>/5
        /// <summary>
        /// Retreives a specific Category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            _logger.LogInformation($"Returned all categories from database.");
            _logger.LogInformation($"Returned Category with id: {id}");
            return Ok(await Mediator.Send(new GetCategoryByIdQuery { Id = id }));
        }




        // POST api/<controller>

        /// <summary>
        /// Creates a Category.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created Category</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreateCategoryCommand command)
        {
            return Ok(await Mediator.Send(command));
        }

        // PUT api/<controller>/5
        /// <summary>
        /// Update a specific Category.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> Put(Guid id, UpdateCategoryCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }
            return Ok(await Mediator.Send(command));
        }

        /// <summary>
        /// Deletes a specific Category.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> Delete(Guid id)
        {
            return Ok(await Mediator.Send(new DeleteCategoryByIdCommand { Id = id }));
        }
    }
}
