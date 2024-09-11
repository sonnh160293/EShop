using Carter;
using Catalog.API.Models;
using MediatR;

namespace Catalog.API.Products.GetProductById
{

    public record GetProductByIdRequest(Guid id);
    public record GetProductByIdResponse(Product product);

    public class GetProductByIdEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapGet("/products/{id}", async (Guid id, ISender sender) =>
            {
                var query = new GetProductByIdQuery(id);
                var result = await sender.Send(query);

                return Results.Ok(result);
            });
        }
    }
}
