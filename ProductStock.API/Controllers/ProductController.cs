namespace ProductStock.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ProductStock.DAL.Interfaces;
    using ProductStock.DL.Enums;
    using ProductStock.DL.Models;
    using SWE.Async.Extensions;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : EntityController<Product>
    {
        public ProductController(IRepository<Product> repository)
            : base(repository)
        {
        }

        /// <summary>
        /// <see cref="HttpGetAttribute"/> api/<see cref="T"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet("Seed")]
        public async Task<bool> Seed()
        {
            var bulb = new Product("lb_12", "Light bulb");
            bulb.Prices = new List<ProductPrice>
            {
                new ProductPrice(bulb.Id, new DateTime(2018, 1, 1, 0, 0, 0).ToLocalTime(), 0.89),
                new ProductPrice(bulb.Id, new DateTime(2018, 4, 1, 0, 0, 0).ToLocalTime(), 0.95),
                new ProductPrice(bulb.Id, new DateTime(2018, 4, 10, 0, 0, 0).ToLocalTime(), 0.92)
            };
            bulb.StockCounts = new List<ProductStockCount>
            {
                new ProductStockCount(bulb.Id, new DateTime(2018, 1, 1, 0, 0, 0).ToLocalTime(), 5),
                new ProductStockCount(bulb.Id, new DateTime(2018, 4, 12, 0, 0, 0).ToLocalTime(), 8)
            };
            bulb.StockMutations = new List<ProductStockMutation>
            {
                new ProductStockMutation(bulb.Id, new DateTime(2018, 1, 10, 0, 0, 0).ToLocalTime(), new DateTime(2018, 1, 20, 0, 0, 0).ToLocalTime(), MutationType.Purchase, 2),
                new ProductStockMutation(bulb.Id, new DateTime(2018, 2, 10, 0, 0, 0).ToLocalTime(), new DateTime(2018, 2, 20, 0, 0, 0).ToLocalTime(), MutationType.Purchase, 2),
                new ProductStockMutation(bulb.Id, new DateTime(2018, 2, 10, 0, 0, 0).ToLocalTime(), new DateTime(2018, 2, 20, 0, 0, 0).ToLocalTime(), MutationType.Supply,  10),
                new ProductStockMutation(bulb.Id, new DateTime(2018, 3, 10, 0, 0, 0).ToLocalTime(), new DateTime(2018, 3, 20, 0, 0, 0).ToLocalTime(), MutationType.Purchase, 2),
                new ProductStockMutation(bulb.Id, new DateTime(2018, 4, 10, 0, 0, 0).ToLocalTime(), new DateTime(2018, 4, 20, 0, 0, 0).ToLocalTime(), MutationType.Purchase, 2),
                new ProductStockMutation(bulb.Id, new DateTime(2018, 4, 12, 0, 0, 0).ToLocalTime(), new DateTime(2018, 4, 12, 0, 0, 0).ToLocalTime(), MutationType.Correction, -1), // should be 9 were 8
                new ProductStockMutation(bulb.Id, new DateTime(2018, 5, 10, 0, 0, 0).ToLocalTime(), new DateTime(2018, 5, 20, 0, 0, 0).ToLocalTime(), MutationType.Purchase, 2),
                new ProductStockMutation(bulb.Id, new DateTime(2018, 6, 10, 0, 0, 0).ToLocalTime(), new DateTime(2018, 6, 20, 0, 0, 0).ToLocalTime(), MutationType.Purchase, 2)
            };

            var table = new Product("tb_square", "Table");
            table.Prices = new List<ProductPrice>
            {
                new ProductPrice(table.Id, new DateTime(2018, 1, 1, 0, 0, 0).ToLocalTime(), 95),
                new ProductPrice(table.Id, new DateTime(2018, 4, 1, 0, 0, 0).ToLocalTime(), 97.50),
                new ProductPrice(table.Id, new DateTime(2018, 4, 10, 0, 0, 0).ToLocalTime(), 0.92)
            };
            table.StockCounts = new List<ProductStockCount>
            {
                new ProductStockCount(table.Id, new DateTime(2018, 1, 1, 0, 0, 0).ToLocalTime(), 2),
                new ProductStockCount(table.Id, new DateTime(2018, 4, 12, 0, 0, 0).ToLocalTime(), 3)
            };
            table.StockMutations = new List<ProductStockMutation>
            {
                new ProductStockMutation(table.Id, new DateTime(2018, 2, 10, 0, 0, 0).ToLocalTime(), new DateTime(2018, 2, 20, 0, 0, 0).ToLocalTime(), MutationType.Supply,  1),
                new ProductStockMutation(table.Id, new DateTime(2018, 5, 10, 0, 0, 0).ToLocalTime(), new DateTime(2018, 5, 20, 0, 0, 0).ToLocalTime(), MutationType.Purchase, 2)
            };
            
            return await Repository.Create(bulb)
                .MapAsync(x => x ? Repository.Create(table) : Task.FromResult(false))
                .ConfigureAwait(false);
        }
    }
}
