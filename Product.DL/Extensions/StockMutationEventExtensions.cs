namespace Product.DL.Extensions
{
    using Product.DL.Events;

    public static class StockMutationEventExtensions
    {
        public static bool ApplyEvent(this StockMutationEvent stockMutationEvent, Models.Product product)
        {
            return stockMutationEvent.ProcessEvent(product, false);
        }

        public static bool RevokeEvent(this StockMutationEvent stockMutationEvent, Models.Product product)
        {
            return stockMutationEvent.ProcessEvent(product, true);
        }

        internal static bool ProcessEvent(this StockMutationEvent stockMutationEvent, Models.Product product, bool revoke)
        {
            var mutationAmount = stockMutationEvent.GetMutationAmount(revoke);

            if (stockMutationEvent.EventDate <= product.StateDate)
            {
                product.Available += mutationAmount;
            }

            if (stockMutationEvent.Value.ShipmentDate <= product.StateDate)
            {
                product.InStock += mutationAmount;
            }

            return true;
        }

        internal static int GetMutationAmount(this StockMutationEvent stockMutationEvent, bool revoke)
        {
            var result = stockMutationEvent.Value.Amount;

            if (stockMutationEvent.Value.Type == ProductStock.DL.Enums.MutationType.Purchase)
            {
                result *= -1;
            }

            if (revoke)
            {
                result *= -1;
            }

            return result;
        }
    }
}