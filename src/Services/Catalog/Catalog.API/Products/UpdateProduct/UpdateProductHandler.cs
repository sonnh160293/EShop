using BuildingBlocks.CQRS;
using Catalog.API.Exceptions;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.UpdateProduct
{

    public record UpdateProductCommand(Guid id, string Name, string Description, string ImageFile, double Price, List<string> Category) : ICommand<UpdateProductResult>;
    public record UpdateProductResult(bool IsSuccess);

    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        public UpdateProductCommandValidator()
        {
            RuleFor(command => command.id).NotEmpty().WithMessage("Product Id is required");
            RuleFor(command => command.Name).NotEmpty().WithMessage("Name is required")
                .Length(2, 150).WithMessage("Name must between 2 and 150 characters");

            RuleFor(command => command.Price).GreaterThan(0).WithMessage("Price must be greater than 0");
        }
    }

    public class UpdateProductCommandHandler(IDocumentSession session)
        : ICommandHandler<UpdateProductCommand, UpdateProductResult>
    {
        public async Task<UpdateProductResult> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {

            var product = await session.LoadAsync<Product>(command.id);
            if (product == null)
            {
                throw new ProductNotFoundException(command.id);
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
