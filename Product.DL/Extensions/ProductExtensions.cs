namespace Product.DL.Extensions
{
    using System;
    using DL.Models;

    public static class ProductExtensions
    {
        public static void SetStateDate(this Product product, DateTimeOffset stateDate)
        {
            product.StateDate = stateDate;
        }
    }
}