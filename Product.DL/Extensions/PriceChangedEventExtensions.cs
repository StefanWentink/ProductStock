namespace Product.DL.Extensions
{
    using Product.DL.Events;

    public static class PriceChangedEventExtensions
    {
        public static bool ApplyEvent(this PriceChangedEvent priceChangedEvent, Models.Product product)
        {
            return priceChangedEvent.ProcessEvent(product, false);
        }

        public static bool RevokeEvent(this PriceChangedEvent priceChangedEvent, Models.Product product)
        {
            return priceChangedEvent.ProcessEvent(product, true);
        }

        internal static bool ProcessEvent(this PriceChangedEvent priceChangedEvent, Models.Product product, bool revoke)
        {
            var mutationAmount = priceChangedEvent.GetMutationAmount(revoke);

            if (priceChangedEvent.EventDate <= product.StateDate)
            {
                product.Price = mutationAmount;
            }

            return true;
        }

        internal static double GetMutationAmount(this PriceChangedEvent priceChangedEvent, bool revoke)
        {
            return revoke
                ? priceChangedEvent.OldPrice
                : priceChangedEvent.Value.Price;
        }
    }
}