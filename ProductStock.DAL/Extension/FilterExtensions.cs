namespace ProductStock.DAL.Extension
{
    using MongoDB.Driver;
    using SWE.Model.Interfaces;
    using System;

    internal static class FilterExtensions
    {
        internal static FilterDefinition<T> GetIdFilter<T>(this Guid id)
            where T : IKey
        {
            return Builders<T>.Filter.Eq(nameof(IKey.Id), id);
        }
    }
}