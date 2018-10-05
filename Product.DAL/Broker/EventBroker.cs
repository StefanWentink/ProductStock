namespace Product.DAL.Broker
{
    using Product.DL.Events;
    using Product.DL.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Threading;
    using MoreLinq;
    using Product.DAL.Context;

    public static class EventBroker
    {
        private static readonly SemaphoreSlim _loadLock = new SemaphoreSlim(1, 1);

        private static Dictionary<Guid, ProductStock.DL.Models.Product> ProductStocks { get; }
            = new Dictionary<Guid, ProductStock.DL.Models.Product>();

        private static Dictionary<Guid, DL.Models.Product> Products { get; }
            = new Dictionary<Guid, DL.Models.Product>();

        private static Dictionary<Guid, List<PriceChangedEvent>> PriceChangedEvents { get; }
            = new Dictionary<Guid, List<PriceChangedEvent>>();

        private static Dictionary<Guid, List<StockMutationEvent>> StockMutationEvents { get; }
            = new Dictionary<Guid, List<StockMutationEvent>>();

        public static async Task<DL.Models.Product> GetProduct(Guid productId)
        {
            if (!ProductStocks.ContainsKey(productId))
            {
                await _loadLock.WaitAsync().ConfigureAwait(false);

                try
                {
                    if (!(await LoadProduct(productId).ConfigureAwait(false)))
                    {
                        throw new NullReferenceException($"Loading product with id {productId} failed.");
                    }
                }
                catch (Exception exception)
                {
                    // Log exception
                    throw;
                }
                finally
                {
                    _loadLock.Release();
                }
            }

            var result = Products[productId];

            SetProductState(result, DateTimeOffset.Now);

            return result;
        }

        public static async Task<IEnumerable<DL.Models.Product>> GetProducts()
        {
            await _loadLock.WaitAsync().ConfigureAwait(false);

            try
            {
                if (!(await LoadProducts().ConfigureAwait(false)))
                {
                    throw new NullReferenceException("Loading products failed.");
                }
            }
            catch (Exception exception)
            {
                // Log exception
                throw;
            }
            finally
            {
                _loadLock.Release();
            }

            var result = Products.Values;

            result.ForEach(x => SetProductState(x, DateTimeOffset.Now));

            return result;
        }

        public static void SetProductState(DL.Models.Product product, DateTimeOffset stateDate)
        {
            product.SetStateDate(stateDate);

            foreach (var priceChangedEvent in PriceChangedEvents[product.Id]
                .Where(x => x.Value.RegisterDate <= stateDate)
                .OrderBy(x => x.Value.RegisterDate))
            {
                priceChangedEvent.ApplyEvent(product);
            }

            foreach (var stockMutationEvent in StockMutationEvents[product.Id]
                .Where(x => x.Value.OrderDate <= stateDate)
                .OrderBy(x => x.Value.OrderDate))
            {
                stockMutationEvent.ApplyEvent(product);
            }
        }

        internal static bool SetProduct(ProductStock.DL.Models.Product product)
        {
            if (!ProductStocks.ContainsKey(product.Id))
            {
                ProductStocks.Add(product.Id, product);

                Products.Add(product.Id, new DL.Models.Product(product.Id, product.Number, product.Description));

                var priceChangedEvents = new List<PriceChangedEvent>();
                double oldPrice = 0;
                foreach (var productPrice in product.Prices)
                {
                    priceChangedEvents.Add(new PriceChangedEvent(productPrice, oldPrice));
                    oldPrice = productPrice.Price;
                }

                var stockMutationEvents = new List<StockMutationEvent>();
                foreach (var stockMutation in product.StockMutations)
                {
                    stockMutationEvents.Add(new StockMutationEvent(stockMutation));
                }

                PriceChangedEvents.Add(product.Id, priceChangedEvents);
                StockMutationEvents.Add(product.Id, stockMutationEvents);

                return true;
            }

            return false;
        }

        private static async Task<bool> LoadProducts()
        {
            try
            {
                var products = await ProductContext.LoadStockProducts().ConfigureAwait(false);

                products.ForEach(x => SetProduct(x));

                return true;
            }
            catch (Exception exception)
            {
                // Log exception
                throw;
            }
        }

        private static async Task<bool> LoadProduct(Guid productId)
        {
            try
            {
                var product = await ProductContext.LoadStockProduct(productId).ConfigureAwait(false);
                return SetProduct(product);
            }
            catch (Exception exception)
            {
                // Log exception
                throw;
            }
        }
    }
}