namespace ProductStock.DAL.Repositories
{
    using ProductStock.Context.DAL;
    using ProductStock.DL.Models;

    public class ProductRepository : Repository<Product>
    {
        public ProductRepository(IContext<Product> context)
            : base(context)
        {
        }
    }
}