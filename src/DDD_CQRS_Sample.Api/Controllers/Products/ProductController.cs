using DDD_CQRS_Sample.Api.Controllers.Base;
using DDD_CQRS_Sample.Application.Products.AddProduct;
using DDD_CQRS_Sample.Application.Products.DeleteProduct;
using DDD_CQRS_Sample.Application.Products.EditProduct;
using DDD_CQRS_Sample.Application.Products.GeProductById;
using DDD_CQRS_Sample.Application.Products.GetProductList;
using DDD_CQRS_Sample.Application.Products.RevertProductStatus;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Shared.DataGrids;
using Shared.Results;

namespace DDD_CQRS_Sample.Api.Controllers.Products
{
    [Route("products")]
    public class ProductController : BaseController
    {
        private readonly ISender _sender;

        public ProductController(ISender sender)
        {
            _sender = sender;
        }


        [HttpPost]
        public async Task<IActionResult> Add([FromBody] AddProductCommand command, CancellationToken cancellationToken)
        {
            Result result = await _sender.Send(command, cancellationToken);
            return ResponseResult(result);
        }

        [HttpPut]
        public async Task<IActionResult> Edit([FromBody] EditProductCommand command, CancellationToken cancellationToken)
        {
            Result result = await _sender.Send(command, cancellationToken);
            return ResponseResult(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            var command = new DeleteProductCommand(id);
            Result result = await _sender.Send(command, cancellationToken);

            return ResponseResult(result);
        }

        [HttpPatch("RevertStatus/{id}")]
        public async Task<IActionResult> RevertStatus(int id, CancellationToken cancellationToken)
        {
            var command = new RevertProductStatusCommand(id);
            Result result = await _sender.Send(command, cancellationToken);

            return ResponseResult(result);
        }

        [HttpGet("List")]
        public async Task<IActionResult> GetList([FromQuery] GetProductListQuery query, CancellationToken cancellationToken)
        {
            Result<DataGridResponse<ProductListResponse>> result = await _sender.Send(query, cancellationToken);
            return ResponseResult(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id, CancellationToken cancellationToken)
        {
            var query = new GetProductByIdQuery(id);
            Result<ProductResponse> response = await _sender.Send(query, cancellationToken);

            return ResponseResult(response);
        }
    }
}