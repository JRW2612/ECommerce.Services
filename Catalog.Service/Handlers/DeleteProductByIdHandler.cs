using Catalog.Service.Commands;
using Catalog.Service.Repositories.Interfaces;
using MediatR;

namespace Catalog.Service.Handlers
{
    public class DeleteProductByIdHandler : IRequestHandler<DeleteProductByIdCommand, bool>
    {
        private readonly IProductRepository _prepository;
        public DeleteProductByIdHandler(IProductRepository prepositor)
        {
            _prepository = prepositor;
        }

        public async Task<bool> Handle(DeleteProductByIdCommand request, CancellationToken cancellationToken)
        {
            var existingData = await _prepository.GetProductByIdAsync(request.Id!);
            if (existingData is null)
            {
                throw new Exception("Product not found");
            }
            var result = await _prepository.DeleteProduct(request.Id);
            return result;
        }
    }
}
