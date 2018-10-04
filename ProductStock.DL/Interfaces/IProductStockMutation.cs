namespace ProductStock.DL.Interfaces
{
    using global::ProductStock.DL.Enums;
    using System;

    public interface IProductStockMutation : IProductReference
    {
        DateTimeOffset OrderDate { get; set; }

        DateTimeOffset ShipmentDate { get; set; }

        MutationType Type { get; set; }

        int Amount { get; set; }
    }
}