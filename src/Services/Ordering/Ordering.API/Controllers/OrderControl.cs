using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MediatR;
using Ordering.Application.Features.Orders.Commands.CheckoutOrder;
using Ordering.Application.Features.Orders.Commands.DeleteOrder;
using Ordering.Application.Features.Orders.Commands.UpdateOrder;
using Ordering.Application.Features.Orders.Queries.GetOrderList;

namespace Ordering.API.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class OrderControl : ControllerBase
    {
        private readonly IMediator _mediator;

        public OrderControl(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userName}", Name = nameof(GetOrdersByUserName))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<OrderVm>>> GetOrdersByUserName(string userName)
        {
            var orderVms = await _mediator.Send(new GetOrderListQuery(userName));
            return orderVms;
        }

        [HttpPost(Name = nameof(CheckoutOrder))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<Guid>> CheckoutOrder([FromBody] CheckoutOrderCommand cmd)
        {
            var result = await _mediator.Send(cmd);
            return result;
        }

        [HttpPost(Name = nameof(UpdateOrder))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> UpdateOrder([FromBody] UpdateOrderCommand cmd)
        {
            await _mediator.Send(cmd);
            return Ok();
        }

        [HttpDelete(Name = nameof(DeleteOrder))]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult> DeleteOrder([FromBody] DeleteOrderCommand cmd)
        {
            await _mediator.Send(cmd);
            return Ok();
        }
    }
}
