namespace ProductStock.DL.Interfaces
{
    using SWE.Model.Interfaces;
    using System;

    public interface IProductReference : IKey
    {
        Guid ProductId { get; set; }
    }
}