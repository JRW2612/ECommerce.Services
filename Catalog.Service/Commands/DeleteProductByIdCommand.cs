using MediatR;

namespace Catalog.Service.Commands
{
    public record DeleteProductByIdCommand : IRequest<bool>
    {
        public DeleteProductByIdCommand(string id)
        {
            Id = id;
        }

        public string Id { get; init; }
    }
}
