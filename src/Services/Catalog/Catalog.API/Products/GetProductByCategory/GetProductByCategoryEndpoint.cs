using Carter;
using MediatR;

namespace Catalog.API.Products.GetProductByCategory
{



    public class GetProductByCategoryEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.Map("/products/category/{category}", async (string category, ISender sender) =>
            {
                var query = new GetProductByCategoryQuery(category);
                var response = await sender.Send(query);
                return Results.Ok(response);
            });
        }
    }
}
