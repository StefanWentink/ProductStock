namespace ProductStock.DAL.Repositories
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using ProductStock.Context.DAL;
    using ProductStock.DAL.Extension;
    using ProductStock.DAL.Interfaces;
    using SWE.Model.Interfaces;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public abstract class Repository<T> : IRepository<T>
        where T : IKey
    {
        private readonly IContext<T> _context;

        protected Repository(IContext<T> context)
        {
            _context = context;
        }

        public async Task<IEnumerable<T>> Read()
        {
            try
            {
                var cursor = await _context.ReadFilter(new BsonDocument()).ConfigureAwait(false);
                var result = new List<T>();

                while (await cursor.MoveNextAsync().ConfigureAwait(false))
                {
                    var batch = cursor.Current;

                    foreach (var document in cursor.Current)
                    {
                        result.Add(document);
                    }
                }

                return result;
            }
            catch (Exception exception)
            {
                // Log exception
                throw;
            }
        }

        public async Task<T> Read(Guid id)
        {
            try
            {
                var cursor = await _context.ReadFilter(id.GetIdFilter<T>()).ConfigureAwait(false);

                if (cursor?.Current?.Any() ?? false)
                {
                    return cursor.Current.SingleOrDefault();
                }
            }
            catch (Exception exception)
            {
                // Log exception
                throw;
            }

            return default;
        }

        public async Task<bool> Create(T item)
        {
            try
            {
                await _context.Items.InsertOneAsync(item).ConfigureAwait(false);
                return true;
            }
            catch (Exception exception)
            {
                // Log exception
                throw;
            }
        }

        public async Task<bool> Update(Guid id, T item)
        {
            try
            {
                var result = await _context.Items.ReplaceOneAsync(
                    x => x.Id == id,
                    item,
                    new UpdateOptions { IsUpsert = true }).ConfigureAwait(false);

                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception exception)
            {
                // Log exception
                throw;
            }
        }

        public async Task<bool> Delete(Guid id)
        {
            try
            {
                var result = await _context.Items.DeleteOneAsync(id.GetIdFilter<T>()).ConfigureAwait(false);

                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception exception)
            {
                // Log exception
                throw;
            }
        }
    }
}