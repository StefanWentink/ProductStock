namespace Product.DL.Events
{
    using ProductStock.DL.Interfaces;
    using SWE.Model.Interfaces;
    using System;

    public abstract class ProductReferenceEvent<TValue> : EventArgs, IKey
        where TValue : IProductReference
    {
        public Guid Id { get; set; }

        public TValue Value { get; set; }

        public DateTimeOffset EventDate { get; set; }

        [Obsolete("Only for serialisation", true)]
        protected ProductReferenceEvent()
        {
        }

        protected ProductReferenceEvent(TValue value, DateTimeOffset eventDate)
        {
            Id = Guid.NewGuid();
            Value = value;
            EventDate = eventDate;
        }
    }
}