using Catalog.Service.Commands;
using Catalog.Service.Data.DTOs;
using Catalog.Service.Mappers;
using Catalog.Service.Queries;
using Catalog.Service.Specifications;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Service.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class CatalogsController(IMediator _mediator) : ControllerBase
    {
        [HttpGet("GetAllProducts")]
        public async Task<ActionResult<IList<ProductDto>>> GetAllProducts([FromQuery] CatalogSpecParams specParams)
        {
            var resultQuery = new GetAllProductQuery(specParams);
            var result = await _mediator.Send(resultQuery);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductDto>> GetProductById(string id)
        {
            var resultQuery = new GetProductByIdQuery(id);
            var result = await _mediator.Send(resultQuery);
            return Ok(result);
        }

        [HttpGet("productName/{productName}")]
        public async Task<ActionResult<IList<ProductDto>>> GetProductByName(string productName)
        {
            var resultQuery = new GetProductByNameQuery(productName);
            var result = await _mediator.Send(resultQuery);
            if (result is null)
            {
                return NotFound();
            }
            var dtoList = result.Select(p => p.ToDto()).ToList();
            return Ok(dtoList);
        }

        [HttpGet("brand/{brandName}", Name = "GetProductByBrandName")]
        public async Task<ActionResult<IList<ProductDto>>> GetProductByBrand(string brandName)
        {
            var resultQuery = new GetProductByBrandQuery(brandName);
            var result = await _mediator.Send(resultQuery);
            return Ok(result);
        }

        [HttpPost("AddProduct")]
        public async Task<ActionResult<CreateProductDto>> CreateProduct([FromBody] CreateProductCommand productCmd)
        {
            var result = await _mediator.Send(productCmd);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(string id)
        {
            var resultCmd = new DeleteProductByIdCommand(id);
            var result = await _mediator.Send(resultCmd);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProduct(string id, UpdateProductDto updateProductDto)
        {
            var resultCmd = updateProductDto.ToCommand(id);
            var result = await _mediator.Send(resultCmd);
            if (!result)
            {
                return NotFound();
            }
            return NoContent();
        }

        [HttpGet("GetAllBrands")]
        public async Task<ActionResult<IList<BrandDto>>> GetAllBrands()
        {
            var resultQuery = new GetAllBrandQuery();
            var result = await _mediator.Send(resultQuery);
            return Ok(result);
        }

        [HttpGet("GetAllTypes")]
        public async Task<ActionResult<IList<TypeDto>>> GetAllTypes()
        {
            var resultQuery = new GetAllTypeQuery();
            var result = await _mediator.Send(resultQuery);
            return Ok(result);
        }
    }
}
