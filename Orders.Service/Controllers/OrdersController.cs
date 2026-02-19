using MediatR;
using Microsoft.AspNetCore.Mvc;
using Orders.Service.Commands;
using Orders.Service.DTOs;
using Orders.Service.Mapper;
using Orders.Service.Queries;

namespace Orders.Service.Controllers
{
    [ApiController]
    [Route("api/v1/[controller]")]

    public class OrdersController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ILogger<OrdersController> _logger;


        public OrdersController(IMediator mediator, ILogger<OrdersController> logger)
        {
            _mediator = mediator;
            _logger = logger;
        }


        [HttpGet("{userName}", Name = "GetOrderByUserName")]
        public async Task<ActionResult<IEnumerable<OrderDTO>>> GetOrderByUserName([FromRoute] string userName)
        {
            var query = new GetOrderListQuery(userName);
            var orders = await _mediator.Send(query);
            _logger.LogInformation("Getting orders for user: {UserName}", userName);
            return Ok(orders);
        }

        [HttpPost(Name = "CheckoutOrder")]
        public async Task<ActionResult<int>> CheckoutOrder([FromBody] CheckoutOrderDTO dto)
        {
            //Extraxt Correlation Id from the request header x-correlation-id
            var correlationid = HttpContext.Request.Headers["x-correlation-id"].FirstOrDefault()
                ?? Guid.NewGuid().ToString();

            var command = dto.ToCommand();
            command.CorrelationId = Guid.Parse(correlationid);
            var orderId = await _mediator.Send(command);
            _logger.LogInformation($"Checking out order for user: {dto.UserName},  Correlation Id:{command.CorrelationId}");
            return Ok(orderId);
        }

        [HttpPut(Name = "UpdateOrder")]
        public async Task<ActionResult<bool>> UpdateOrder([FromBody] OrderDTO dto)
        {
            //Extraxt Correlation Id from the request header x-correlation-id
            var correlationid = HttpContext.Request.Headers["x-correlation-id"].FirstOrDefault()
                ?? Guid.NewGuid().ToString();
            var command = dto.ToCommand();
            command.CorrelationId = Guid.Parse(correlationid);
            var orderId = await _mediator.Send(command);
            _logger.LogInformation($"Updated order for user: {dto.Id}, Correlation Id:{command.CorrelationId}");
            return Ok(orderId);
        }

        [HttpDelete("{Id}", Name = "CancelOrder")]
        public async Task<ActionResult<int>> DeleteOrder([FromRoute] int Id)
        {
            //Extraxt Correlation Id from the request header x-correlation-id
            var correlationid = HttpContext.Request.Headers["x-correlation-id"].FirstOrDefault()
                ?? Guid.NewGuid().ToString();
            var command = new DeleteOrderCommand { Id = Id, CorrelationId = Guid.Parse(correlationid) };
            await _mediator.Send(command);
            _logger.LogInformation($"Deleted order with Id: {Id},Correlation Id:{command.CorrelationId}");
            return NoContent();
        }
    }
}
