using BuildingBlocks.Logging;
using Carter;
using Mapster;
using MediatR;

namespace Catalog.API.Products.CreateProduct
{

    public record CreateProductRequest(string Name, string Description, string ImageFile, double Price, List<string> Category);
    public record CreateProductResponse(Guid id);

    public class CreateProductEndpoint(ILogService logService) : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/products", async (CreateProductRequest request, ISender sender, HttpContext context) =>
            {
                try
                {
                    var command = request.Adapt<CreateProductCommand>();
                    var result = await sender.Send(command);

                    //await logService.LogRequestAsync(new LogEntry()
                    //{
                    //    Timestamp = DateTime.UtcNow,
                    //    RequestPath = context.Request.Path,
                    //    IsSuccess = true,

                    //});
                    return Results.Created($"/products/{result.id}", result);
                }
                catch (Exception ex)
                {
                    //await logService.LogRequestAsync(new LogEntry()
                    //{
                    //    Timestamp = DateTime.UtcNow,
                    //    RequestPath = context.Request.Path,
                    //    IsSuccess = false,
                    //    ResponseStatus = StatusCodes.Status500InternalServerError.ToString(),
                    //    ErrorMessage = ex.Message
                    //});

                    // Return an appropriate error response
                    return Results.Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
                }
            })
                .WithName("CreateProduct")
                .Produces<CreateProductResponse>(StatusCodes.Status201Created)
                .ProducesProblem(StatusCodes.Status400BadRequest)
                .WithSummary("Create Product")
                .WithDescription("Create Product");
        }
    }
}
