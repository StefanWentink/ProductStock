namespace Product.DL.Events
{
    using ProductStock.DL.Interfaces;
    using System;

    public class PriceChangedEvent : ProductReferenceEvent<IProductPrice>
    {
        public double OldPrice { get; set; }

        [Obsolete("Only for serialisation", true)]
        public PriceChangedEvent()
        {
        }

        public PriceChangedEvent(IProductPrice value, double oldPrice)
            : this(value, oldPrice, value.RegisterDate)
        {
        }

        public PriceChangedEvent(IProductPrice value, double oldPrice, DateTimeOffset eventDate)
            : base(value, eventDate)
        {
            OldPrice = oldPrice;
        }
    }
}