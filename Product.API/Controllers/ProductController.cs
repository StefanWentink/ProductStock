namespace Product.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Product.DAL.Broker;

    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        /// <summary>
        /// <see cref="HttpGetAttribute"/> api/<see cref="T"/>/<see cref="Get"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        public async Task<IEnumerable<DL.Models.Product>> Get()
        {
            return await EventBroker.GetProducts().ConfigureAwait(false);
        }

        /// <summary>
        /// <see cref="HttpGetAttribute"/> api/<see cref="T"/>/<see cref="Get"/>/<see cref="IKey.Id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        public async Task<DL.Models.Product> Get(Guid id)
        {
            return await EventBroker.GetProduct(id).ConfigureAwait(false);
        }
    }
}
