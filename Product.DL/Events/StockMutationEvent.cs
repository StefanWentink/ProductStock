namespace Product.DL.Events
{
    using ProductStock.DL.Interfaces;
    using System;

    public class StockMutationEvent : ProductReferenceEvent<IProductStockMutation>
    {
        [Obsolete("Only for serialisation", true)]
        public StockMutationEvent()
        {
        }

        public StockMutationEvent(IProductStockMutation value)
            : this(value, value.OrderDate)
        {
        }

        public StockMutationEvent(IProductStockMutation value, DateTimeOffset eventDate)
            : base(value, eventDate)
        {
        }
    }
}