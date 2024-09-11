using Carter;
using MediatR;

namespace Catalog.API.Products.UpdateProduct
{

    public record UpdateProductRequest(Guid id, string Name, string Description, string ImageFile, double Price, List<string> Category);


    public class UpdateProductEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPut("/products", async (UpdateProductRequest request, ISender sender) =>
            {
                var command = new UpdateProductCommand(request.id, request.Name, request.Description, request.ImageFile, request.Price, request.Category);

                var response = await sender.Send(command);
                return Results.Ok(response);
            });
        }
    }
}
