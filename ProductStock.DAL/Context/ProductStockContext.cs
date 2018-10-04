namespace ProductStock.Context.DAL
{
    using Microsoft.Extensions.Options;
    using MongoDB.Driver;
    using ProductStock.DAL.Models;
    using ProductStock.DL.Models;

    public class ProductStockContext : Context<Product>
    {
        public ProductStockContext(IOptions<SettingsModel> settings)
            :base(settings)
        {
        }

        protected override string EntityName => nameof(Product);
    }
}