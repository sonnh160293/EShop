using BuildingBlocks.CQRS;
using Catalog.API.Models;
using FluentValidation;
using Marten;

namespace Catalog.API.Products.CreateProduct
{

    //request gửi đến trong mediatr
    public record CreateProductCommand(string Name, string Description, string ImageFile, double Price, List<string> Category) : ICommand<CreateProductResult>;

    //response trả ra bởi mediatr
    public record CreateProductResult(Guid id);


    //validate request
    public class CreateProductCommandValidator : AbstractValidator<CreateProductCommand>
    {
        public CreateProductCommandValidator()
        {
            RuleFor(p => p.Name).NotEmpty().WithMessage("Name is required");
            RuleFor(p => p.Category).NotEmpty().WithMessage("Category is required");
            RuleFor(p => p.ImageFile).NotEmpty().WithMessage("ImageFile is required");
            RuleFor(p => p.Price).GreaterThan(0).WithMessage("Price is greater than 0");
        }
    }

    public class CreateProductCommandHandler(IDocumentSession session)
        : ICommandHandler<CreateProductCommand, CreateProductResult>
    {
        public async Task<CreateProductResult> Handle(CreateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {


                var product = new Product()
                {

                    Name = command.Name,
                    Category = command.Category,
                    Description = command.Description,
                    ImageFile = command.ImageFile,
                    Price = command.Price,
                };

                // Store the product
                session.Store(product);

                // Save changes
                await session.SaveChangesAsync(cancellationToken);

                // Verify the product was saved by retrieving it
                var savedProduct = await session.LoadAsync<Product>(product.Id, cancellationToken);

                if (savedProduct != null)
                {

                    // Product was successfully saved
                    return new CreateProductResult(product.Id);
                }
                else
                {
                    // Product was not found after saving
                    throw new Exception("Product was not found after saving.");
                }
            }
            catch (Exception ex)
            {
                // Log the exception and handle it as needed
                // Example: logger.LogError(ex, "An error occurred while creating the product.");
                throw; // Rethrow the exception or handle it appropriately
            }
        }

    }
}
