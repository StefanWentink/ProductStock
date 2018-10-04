namespace ProductStock.DL.Interfaces
{
    using System;

    public interface IProductStockCount : IProductReference
    {
        DateTimeOffset RegisterDate { get; set; }

        int Amount { get; set; }
    }
}