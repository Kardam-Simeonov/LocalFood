using server.Data;
using server.Models;
using System.Diagnostics.Metrics;

namespace server
{
    public class Seed
    {
        private readonly DataContext dataContext;
        public Seed(DataContext context)
        {
            this.dataContext = context;
        }
        public void SeedDataContext()
        {
            if (!dataContext.Sellers.Any())
            {
                var seller = new Seller { Name = "Seller1" };
                dataContext.Sellers.Add(seller);
                dataContext.SaveChanges();

                var product = new Product { Name = "Product1", Price = 100.0, Distance = 10.0, SellerId = seller.Id };
                dataContext.Products.Add(product);
                dataContext.SaveChanges();
            }
        }
    }
}
