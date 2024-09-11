using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using Marten;

namespace Catalog.API.Products.UpdateProduct
{

    public record UpdateProductCommand(Guid id, string Name, string Description, string ImageFile, double Price, List<string> Category) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandHandler(IDocumentSession session, ILogger<UpdateProductCommandHandler> logger)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            logger.LogInformation("UpdateProductCommandHandler. Handle called with {@Command}", command);
            var product = await session.LoadAsync<Product>(command.id);
            if (product == null)
            {
                throw new ProductNotFoundException();
            }
            product.Name = command.Name ?? product.Name;
            product.Description = command.Description ?? product.Description;
            product.ImageFile = command.ImageFile ?? product.ImageFile;
            product.Price = command.Price > 0 ? command.Price : product.Price;
            product.Category = command.Category ?? product.Category;
            session.Update(product);
            await session.SaveChangesAsync();
            return new UpdateProductResult(true);
        }
    }
}
