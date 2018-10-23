namespace Product.DAL.Broker
{
    using Product.DL.Extensions;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Threading;
    using MoreLinq;
    using Product.DAL.Context;
    using SWE.EventSourcing.Factories;
    using SWE.EventSourcing.Containers;
    using SWE.EventSourcing.Interfaces.Events;

    public static class EventBroker
    {
        private static readonly SemaphoreSlim _loadLock = new SemaphoreSlim(1, 1);

        private static Dictionary<Guid, ProductStock.DL.Models.Product> ProductStocks { get; }
            = new Dictionary<Guid, ProductStock.DL.Models.Product>();

        private static Dictionary<Guid, DL.Models.Product> Products { get; }
            = new Dictionary<Guid, DL.Models.Product>();

        private static OrderedEventContainer<DL.Models.Product, Guid, Guid, DateTimeOffset> EventContainer { get; set; }

        private static void AddEvents(Guid id, OrderedEventCollection<DL.Models.Product, Guid, DateTimeOffset> collection)
        {
            if (EventContainer == null)
            {
                EventContainer = new OrderedEventContainer<DL.Models.Product, Guid, Guid, DateTimeOffset>(id, collection);
            }
            else
            {
                collection.ForEach(x => EventContainer.Add(x, id));
            }
        }

        //private static Dictionary<Guid, IEnumerable<OrderedChangeEvent<DL.Models.Product, Guid, DateTimeOffset>>> PriceChangedEvents { get; }
        //    = new Dictionary<Guid, IEnumerable<OrderedChangeEvent<DL.Models.Product, Guid, DateTimeOffset>>>();

        //private static Dictionary<Guid, IEnumerable<OrderedMutationEvent<DL.Models.Product, Guid, DateTimeOffset>>> StockAvailableMutationEvents { get; }
        //    = new Dictionary<Guid, IEnumerable<OrderedMutationEvent<DL.Models.Product, Guid, DateTimeOffset>>>();

        //private static Dictionary<Guid, IEnumerable<OrderedMutationEvent<DL.Models.Product, Guid, DateTimeOffset>>> StockInStockMutationEvents { get; }
        //    = new Dictionary<Guid, IEnumerable<OrderedMutationEvent<DL.Models.Product, Guid, DateTimeOffset>>>();

        public static async Task<DL.Models.Product> GetProduct(Guid productId)
        {
            return await GetProduct(productId, DateTimeOffset.Now).ConfigureAwait(false);
        }

        public static async Task<DL.Models.Product> GetProduct(Guid productId, DateTimeOffset referenceDate)
        {
            DL.Models.Product result = null;

            if (!ProductStocks.ContainsKey(productId))
            {
                await _loadLock.WaitAsync().ConfigureAwait(false);

                try
                {
                    result = await LoadProduct(productId).ConfigureAwait(false);
                    SetProductState(result, referenceDate);
                    return result;
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

        public static void SetProductState(DL.Models.Product product, DateTimeOffset referenceDate)
        {
            product.SetStateDate(referenceDate);

            EventContainer.ApplyAll(product, referenceDate);
        }

        internal static DL.Models.Product SetProduct(ProductStock.DL.Models.Product product)
        {
            if (!ProductStocks.ContainsKey(product.Id))
            {
                ProductStocks.Add(product.Id, product);

                var result = new DL.Models.Product(product.Id, product.Number, product.Description);
                Products.Add(product.Id, result);

                var priceChanges = ChangeFactory.ToOrderedChangeEvent<
                    DL.Models.Product,
                    ProductStock.DL.Models.ProductPrice,
                    Guid,
                    double,
                    DateTimeOffset>(
                    x => x.Price,
                    0,
                    product.Prices,
                    x => x.Id,
                    x => x.Price,
                    x => x.RegisterDate).
                    Cast<IEvent<DL.Models.Product, Guid>>().ToList();

                var stockAvailableMutations = MutationFactory.ToOrderedMutationEvent<
                                    DL.Models.Product,
                                    ProductStock.DL.Models.ProductStockMutation,
                                    Guid,
                                    int,
                                    DateTimeOffset>(
                                    x => x.Available,
                                    product.StockMutations,
                                    x => x.Id,
                                    x => x.Amount * (x.Type == ProductStock.DL.Enums.MutationType.Purchase ? -1 : 1),
                                    x => x.OrderDate).Cast<IEvent<DL.Models.Product, Guid>>().ToList();

                var stockInStockMutations = MutationFactory.ToOrderedMutationEvent<
                                    DL.Models.Product,
                                    ProductStock.DL.Models.ProductStockMutation,
                                    Guid,
                                    int,
                                    DateTimeOffset>(
                                    x => x.InStock,
                                    product.StockMutations,
                                    x => x.Id,
                                    x => x.Amount * (x.Type == ProductStock.DL.Enums.MutationType.Purchase ? -1 : 1),
                                    x => x.ShipmentDate).Cast<IEvent<DL.Models.Product, Guid>>().ToList();

                AddEvents(product.Id, new OrderedEventCollection<DL.Models.Product, Guid, DateTimeOffset>(priceChanges.ToList()));
                AddEvents(product.Id, new OrderedEventCollection<DL.Models.Product, Guid, DateTimeOffset>(stockAvailableMutations.ToList()));
                AddEvents(product.Id, new OrderedEventCollection<DL.Models.Product, Guid, DateTimeOffset>(stockInStockMutations.ToList()));

                return result;
            }

            return Products[product.Id];
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

        private static async Task<DL.Models.Product> LoadProduct(Guid productId)
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