namespace ProductStock.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using ProductStock.DAL.Interfaces;
    using SWE.Model.Interfaces;

    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public abstract class EntityController<T> : ControllerBase
        where T : IKey
    {
        protected IRepository<T> Repository { get; }

        protected EntityController(IRepository<T> repository)
        {
            Repository = repository;
        }

        /// <summary>
        /// <see cref="HttpGetAttribute"/> api/<see cref="T"/>/<see cref="Index"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet("Index")]
        public Task<IEnumerable<string>> Index()
        {
            var result = new List<string>
            {
                nameof(Index),
                nameof(Get),
                nameof(Put),
                nameof(Post),
                nameof(Delete),
            }.AsEnumerable();

            return Task.FromResult(result);
        }

        /// <summary>
        /// <see cref="HttpGetAttribute"/> api/<see cref="T"/>/<see cref="Get"/>
        /// </summary>
        /// <returns></returns>
        [HttpGet("Get")]
        public async Task<IEnumerable<T>> Get()
        {
            return await Repository.Read().ConfigureAwait(false);
        }

        /// <summary>
        /// <see cref="HttpGetAttribute"/> api/<see cref="T"/>/<see cref="Get"/>/<see cref="IKey.Id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("Get/{id}")]
        public async Task<T> Get(Guid id)
        {
            return await Repository.Read(id).ConfigureAwait(false);
        }

        /// <summary>
        /// <see cref="HttpPosttAttribute"/> api/<see cref="T"/>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<bool> Post([FromBody] T item)
        {
            return await Repository.Create(item).ConfigureAwait(false);
        }

        /// <summary>
        /// <see cref="HttpPutAttribute"/> api/<see cref="T"/>
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        public async Task<bool> Put([FromBody] T item)
        {
            return await Repository.Create(item).ConfigureAwait(false);
        }

        /// <summary>
        /// <see cref="HttpDeleteAttribute"/> api/<see cref="T"/>/<see cref="IKey.Id"/>
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<bool> Delete(Guid id)
        {
            return await Repository.Delete(id).ConfigureAwait(false);
        }
    }
}