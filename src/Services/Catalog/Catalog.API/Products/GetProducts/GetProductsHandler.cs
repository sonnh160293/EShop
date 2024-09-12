using BuildingBlocks.CQRS;
using Catalog.API.Models;
using Marten;
using Marten.Pagination;

namespace Catalog.API.Products.GetProduct
{

    public record GetProductsQuery(int? PageIndex, int? PageSize) : IQuery<GetProductsResult>;
    public record GetProductsResult(IEnumerable<Product> Products);

    internal class GetProductsQueryHandler(IDocumentSession session)
        : IQueryHandler<GetProductsQuery, GetProductsResult>
    {
        public async Task<GetProductsResult> Handle(GetProductsQuery query, CancellationToken cancellationToken)
        {


            var products = await session.Query<Product>().ToPagedListAsync(query.PageIndex ?? 1, query.PageSize ?? 2, cancellationToken);


            return new GetProductsResult(products);
        }
    }
}
