namespace ProductStock.DL.Models
{
    using global::ProductStock.DL.Enums;
    using ProductStock.DL.Interfaces;
    using System;

    public class ProductStockMutation : IProductStockMutation
    {
        public Guid Id { get; set; }

        public Guid ProductId { get; set; }

        public DateTimeOffset OrderDate { get; set; }

        public DateTimeOffset ShipmentDate { get; set; }

        public MutationType Type { get; set; }

        public int Amount { get; set; }

        [Obsolete("Only for serialization.", true)]
        public ProductStockMutation()
        {
        }

        public ProductStockMutation(
            Guid productId,
            DateTimeOffset orderDate,
            DateTimeOffset shipmentDate,
            MutationType type,
            int amount)
        {
            Id = Guid.NewGuid();
            ProductId = productId;
            OrderDate = orderDate;
            ShipmentDate = shipmentDate;
            Type = type;
            Amount = amount;
        }
    }
}