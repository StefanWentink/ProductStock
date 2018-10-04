namespace Product.DL.Events
{
    using ProductStock.DL.Interfaces;
    using System;

    public class StockCountEvent : ProductReferenceEvent<IProductStockCount>
    {
        public int OldAmount { get; set; }

        [Obsolete("Only for serialisation", true)]
        public StockCountEvent()
        {
        }

        public StockCountEvent(IProductStockCount value, int oldAmount)
            : this(value, value.RegisterDate)
        {
        }

        public StockCountEvent(IProductStockCount value, int oldAmount, DateTimeOffset eventDate)
            : base(value, eventDate)
        {
        }
    }
}