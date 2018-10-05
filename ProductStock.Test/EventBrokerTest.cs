namespace ProductStock.Test
{
    using Product.DAL.Broker;
    using ProductStock.Factory.Factories;
    using SWE.Xunit.Attributes;
    using System;
    using Xunit;

    public class EventBrokerTest
    {
        [Theory]
        [Category("EventBroker")]
        [InlineData(1, 10, 0.89, 0, -2)]
        [InlineData(1, 20, 0.89, -2, -2)]
        [InlineData(2, 10, 0.89, 8, 6)]
        [InlineData(2, 20, 0.89, 6, 6)]
        [InlineData(3, 31, 0.95, 4, 4)]
        [InlineData(4, 12, 0.92, 3, 1)]
        [InlineData(6, 30, 0.92, -3, -3)]
        public void SetProductState_Should_ApplyEventsBasedOnStateDate(
            int month,
            int day,
            double expectedPrice,
            int expectedInStock,
            int expectedAvailable)
        {
            var productStock = ProductFactory.CreateBulbProduct();
            var product = EventBroker.SetProduct(productStock);
            var stateDate = new DateTime(2018, month, day, 0, 0, 0).ToLocalTime();

            EventBroker.SetProductState(product, stateDate);

            Assert.Equal(expectedPrice, product.Price);
            Assert.Equal(expectedInStock, product.InStock);
            Assert.Equal(expectedAvailable, product.Available);
        }
    }
}
