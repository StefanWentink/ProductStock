namespace ProductStock.DL.Models
{
    using ProductStock.DL.Interfaces;
    using System;

    public class ProductStockCount : IProductStockCount
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public DateTimeOffset RegisterDate { get; set; }

        public int Amount { get; set; }

        [Obsolete("Only for serialization.", true)]
        public ProductStockCount()
        {
        }

        public ProductStockCount(
            Guid productId,
            DateTimeOffset registerDate,
            int amount)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            RegisterDate = registerDate;
            Amount = amount;
        }
    }
}