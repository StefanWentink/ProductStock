namespace Product.DL.Events
{
    using ProductStock.DL.Interfaces;
    using SWE.Model.Interfaces;
    using System;

    public abstract class ProductReferenceEvent<T> : EventArgs, IKey
        where T : IProductReference
    {
        public Guid Id { get; set; }

        public T Value { get; set; }

        public DateTimeOffset EventDate { get; set; }

        [Obsolete("Only for serialisation", true)]
        protected ProductReferenceEvent()
        {
        }

        protected ProductReferenceEvent(T value, DateTimeOffset eventDate)
        {
            Id = Guid.NewGuid();
            Value = value;
            EventDate = eventDate;
        }
    }
}