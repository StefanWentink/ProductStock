namespace ProductStock.DL.Interfaces
{
    using System;

    public interface IProduct
    {
        Guid Id { get; set; }

        string Number { get; set; }

        string Description { get; set; }
    }
}