namespace ProductStock.Context.DAL
{
    using MongoDB.Bson;
    using MongoDB.Driver;
    using System.Threading.Tasks;

    public interface IContext<T>
    {
        IMongoCollection<T> Items { get; }

        Task<IAsyncCursor<T>> ReadFilter(BsonDocument filter);

        Task<IAsyncCursor<T>> ReadFilter(FilterDefinition<T> filter);
    }
}