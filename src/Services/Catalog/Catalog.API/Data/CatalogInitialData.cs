using Catalog.API.Models;
using Marten;
using Marten.Schema;

namespace Catalog.API.Data
{
    public class CatalogInitialData : IInitialData
    {
        public async Task Populate(IDocumentStore store, CancellationToken cancellation)
        {
            using var session = store.LightweightSession();
            if (await session.Query<Product>().AnyAsync())
            {
                return;
            }

            session.Store<Product>(GetPreconfigredProducts());
            await session.SaveChangesAsync();
        }

        private static IEnumerable<Product> GetPreconfigredProducts()
        {
            return new List<Product>
            {
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Smartphone A",
            Description = "A high-quality smartphone with a sleek design.",
            ImageFile = "smartphone_a.jpg",
            Price = 299.99,
            Category = new List<string> { "Electronics", "Phones" }
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Smartphone B",
            Description = "An advanced smartphone with a powerful processor.",
            ImageFile = "smartphone_b.jpg",
            Price = 399.99,
            Category = new List<string> { "Electronics", "Phones" }
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Smartphone C",
            Description = "A budget-friendly smartphone with essential features.",
            ImageFile = "smartphone_c.jpg",
            Price = 199.99,
            Category = new List<string> { "Electronics", "Phones" }
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Smartphone D",
            Description = "A smartphone with an excellent camera and long battery life.",
            ImageFile = "smartphone_d.jpg",
            Price = 499.99,
            Category = new List<string> { "Electronics", "Phones" }
        },
        new Product
        {
            Id = Guid.NewGuid(),
            Name = "Smartphone E",
            Description = "A smartphone with high-end features and a premium build.",
            ImageFile = "smartphone_e.jpg",
            Price = 599.99,
            Category = new List<string> { "Electronics", "Phones" }
        }
    };
        }

    }
}
