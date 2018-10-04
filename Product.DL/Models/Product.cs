namespace Product.DL.Models
{
    using ProductStock.DL.Models;
    using System;

    public class Product : BaseProduct
    {
        public double Price { get; internal set; }

        public int Available { get; internal set; }

        public int InStock { get; internal set; }

        public DateTimeOffset StateDate { get; internal set; }

        public Product(Guid id, string number, string description)
            : this(id, number, description, 0, 0, 0, DateTimeOffset.Now)
        {
        }

        internal Product(Guid id, string number, string description, double price, int available, int inStock, DateTimeOffset stateDate)
            : base(id, number, description)
        {
            Price = price;
            Available = available;
            InStock = inStock;
            StateDate = stateDate;
        }
    }
}