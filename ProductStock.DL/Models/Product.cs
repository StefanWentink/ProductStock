namespace ProductStock.DL.Models
{
    using System;
    using System.Collections.Generic;

    public class Product : BaseProduct
    {
        public DateTimeOffset StateDate { get; set; }

        public ICollection<ProductPrice> Prices { get; set; } = new List<ProductPrice>();

        public ICollection<ProductStockCount> StockCounts { get; set; } = new List<ProductStockCount>();

        public ICollection<ProductStockMutation> StockMutations { get; set; } = new List<ProductStockMutation>();

        [Obsolete("Only for serialization.", true)]
        protected Product()
        {
        }

        public Product(
            string number,
            string description)
            : base(Guid.NewGuid(), number, description)
        {
        }
    }
}