using Basket.Service.Commands;
using Basket.Service.Data.DTOs;
using Basket.Service.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.Service.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class BasketsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BasketsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{userName}")]
        public async Task<ActionResult<ShoppingCartDto>> GetBasket(string userName)
        {
            var query = new GetBasketByUsernameQuery(userName);
            var basket = await _mediator.Send(query);
            return Ok(basket);
        }

        [HttpPost("CreateBasket")]
        public async Task<ActionResult<ShoppingCartDto>> CreateOrUpdateBasket([FromBody] CreateShoppingCartCommand cartCommand)
        {
            var basket = await _mediator.Send(cartCommand);
            return Ok(basket);
        }

        [HttpDelete("{userName}")]
        public async Task<ActionResult<ShoppingCartDto>> DeleteBasket(string userName)
        {
            var query = new DeleteBasketByUserNameCommand(userName);
            var basket = await _mediator.Send(query);
            return Ok();
        }

        [HttpPost("Checkout")]
        public async Task<ActionResult> Checkout([FromBody] CheckoutBasketDto dto)
        {
            var basket = await _mediator.Send(new CheckoutBasketCommand(dto));
            return Accepted();
        }

    }
}
