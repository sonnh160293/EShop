using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.DeleteProduct
{

    public record DeleteProductCommand(Guid id) : ICommand<DeleleProductResult>;
    public record DeleleProductResult(bool IsSuccess);

    public class DeleteProductCommandHandler(IDocumentSession session, ILogger<DeleteProductCommandHandler> logger)
        : ICommandHandler<DeleteProductCommand, DeleleProductResult>
    {
        public async Task<DeleleProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("DeleteProductCommandHandler. Handle called with {@Command}", command);
            var product = await session.LoadAsync<Product>(command.id);
            if (product is null)
            {
                throw new ProductNotFoundException();
            }

            session.Delete(product);
            await session.SaveChangesAsync();

            return new DeleleProductResult(true);
        }
    }
}
