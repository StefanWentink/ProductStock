namespace ProductStock.Context.DAL
{
    using Microsoft.Extensions.Options;
    using MongoDB.Bson;
    using MongoDB.Driver;
    using ProductStock.DAL.Models;
    using System;
    using System.Threading.Tasks;

    public abstract class Context<T> : IContext<T>
    {
        private readonly IMongoDatabase _database;

        protected Context(IOptions<SettingsModel> settings)
        {
            var client = new MongoClient(settings.Value.ConnectionString);

            if (client == null)
            {
                throw new NullReferenceException($"{nameof(client)} can not be null.");
            }

            _database = client.GetDatabase(settings.Value.Database);
        }

        public IMongoCollection<T> Items
        {
            get
            {
                return _database.GetCollection<T>(EntityName);
            }
        }

        protected abstract string EntityName { get; }

        public async Task<IAsyncCursor<T>> ReadFilter(BsonDocument filter)
        {
            return await Items.FindAsync(filter).ConfigureAwait(false);
        }

        public async Task<IAsyncCursor<T>> ReadFilter(FilterDefinition<T> filter)
        {
            return await Items.FindAsync(filter).ConfigureAwait(false);
        }
    }
}
