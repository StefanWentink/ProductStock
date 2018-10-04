namespace ProductStock.DL.Models
{
    using ProductStock.DL.Interfaces;
    using System;

    public class ProductPrice : IProductPrice
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public DateTimeOffset RegisterDate { get; set; }

        public double Price { get; set; }

        [Obsolete("Only for serialization.", true)]
        public ProductPrice()
        {
        }

        public ProductPrice(
            Guid productId,
            DateTimeOffset registerDate,
            double price)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            RegisterDate = registerDate;
            Price = price;
        }
    }
}