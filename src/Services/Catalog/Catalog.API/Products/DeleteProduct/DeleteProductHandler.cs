using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.DeleteProduct
{

    public record DeleteProductCommand(Guid id) : ICommand<DeleleProductResult>;
    public record DeleleProductResult(bool IsSuccess);

    public class DeleteProductCommandValidator : AbstractValidator<DeleteProductCommand>
    {
        public DeleteProductCommandValidator()
        {
            RuleFor(x => x.id).NotEmpty().WithMessage("Product Id is requirede");
        }
    }
    public class DeleteProductCommandHandler(IDocumentSession session)
        : ICommandHandler<DeleteProductCommand, DeleleProductResult>
    {
        public async Task<DeleleProductResult> Handle(DeleteProductCommand command, CancellationToken cancellationToken)
        {

            var product = await session.LoadAsync<Product>(command.id);
            if (product is null)
            {
                throw new ProductNotFoundException(command.id);
            }

            session.Delete(product);
            await session.SaveChangesAsync();

            return new DeleleProductResult(true);
        }
    }
}
