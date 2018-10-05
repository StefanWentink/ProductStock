namespace ProductStock.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ProductStock.DAL.Interfaces;
    using ProductStock.DL.Enums;
    using ProductStock.DL.Models;
    using ProductStock.Factory.Factories;
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
        /// <see cref="HttpGetAttribute"/> api/<see cref="Product"/>/<see cref="Seed"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet("Seed")]
        public async Task<bool> Seed()
        {
            var bulb = ProductFactory.CreateBulbProduct();
            var table = ProductFactory.CreateTableProduct();

            return await Repository.Create(bulb)
                .MapAsync(
                    x => x
                    ? Repository.Create(table)
                    : Task.FromResult(false))
                .ConfigureAwait(false);
        }
    }
}
