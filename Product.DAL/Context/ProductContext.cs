namespace Product.DAL.Context
{
    using System;
    using System.Collections.Generic;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Newtonsoft.Json;

    internal static class ProductContext
    {
        internal static async Task<ProductStock.DL.Models.Product> LoadStockProduct(Guid productId)
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync($"https://localhost:44312/api/Product/Get/{productId}").ConfigureAwait(false);
                return JsonConvert.DeserializeObject<ProductStock.DL.Models.Product>(result);
            }
        }

        internal static async Task<IEnumerable<ProductStock.DL.Models.Product>> LoadStockProducts()
        {
            using (var client = new HttpClient())
            {
                var result = await client.GetStringAsync($"https://localhost:44312/api/Product/Get").ConfigureAwait(false);
                return JsonConvert.DeserializeObject<IEnumerable<ProductStock.DL.Models.Product>>(result);
            }
        }
    }
}
