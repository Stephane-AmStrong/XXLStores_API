using Application.Features.Payments.Commands.Create;
using Application.Features.Payments.Commands.Delete;
using Application.Features.Payments.Commands.Update;
using Application.Features.Payments.Queries.GetById;
using Application.Features.Payments.Queries.GetPagedList;
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
    public class PaymentsController : BaseApiController
    {
        //readonly IDiagnosticContext _diagnosticContext;
        public PaymentsController()
        {
        }


        /// <summary>
        /// return payments that matche the criteria
        /// </summary>
        /// <param name="paymentsQuery"></param>
        /// <returns></returns>
        [HttpGet]
        [Authorize(Policy = "payment.read.policy")]
        public async Task<IActionResult> Get([FromQuery] GetPaymentsQuery paymentsQuery)
        {
            var payments = await Mediator.Send(paymentsQuery);
            Response.Headers.Add("X-Pagination", JsonConvert.SerializeObject(payments.MetaData));
            return Ok(payments.PagedList);
        }


        /// <summary>
        /// Retreives a specific Payment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [Authorize(Policy = "payment.read.policy")]
        public async Task<IActionResult> Get(Guid id)
        {
            return Ok(await Mediator.Send(new GetPaymentByIdQuery { Id = id }));
        }


        /// <summary>
        /// Creates a Payment.
        /// </summary>
        /// <param name="command"></param>
        /// <returns>A newly created Payment</returns>
        /// <response code="201">Returns the newly created command</response>
        /// <response code="400">If the command is null</response>            
        [HttpPost]
        [Authorize(Policy = "payment.write.policy")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Post(CreatePaymentCommand command)
        {
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Update a specific Payment.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="command"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [Authorize(Policy = "payment.write.policy")]
        public async Task<IActionResult> Put(Guid id, UpdatePaymentCommand command)
        {
            command.Id = id;
            return Ok(await Mediator.Send(command));
        }


        /// <summary>
        /// Deletes a specific Payment.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [Authorize(Policy = "payment.manage.policy")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await Mediator.Send(new DeletePaymentCommand { Id = id });
            return NoContent();
        }
    }
}
