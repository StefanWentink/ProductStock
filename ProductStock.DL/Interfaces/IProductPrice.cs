namespace ProductStock.DL.Interfaces
{
    using System;

    public interface IProductPrice : IProductReference
    {
        DateTimeOffset RegisterDate { get; set; }

        double Price { get; set; }
    }
}