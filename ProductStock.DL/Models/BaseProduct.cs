namespace ProductStock.DL.Models
{
    using ProductStock.DL.Interfaces;
    using SWE.Model.Interfaces;
    using System;

    public abstract class BaseProduct : IProduct, IKey
    {
        public Guid Id { get; set; }

        public string Number { get; set; }

        public string Description { get; set; }

        [Obsolete("Only for serialization.", true)]
        protected BaseProduct()
        {
        }

        protected BaseProduct(
            Guid id,
            string number,
            string description)
        {
            Id = id;
            Number = number;
            Description = description;
        }
    }
}