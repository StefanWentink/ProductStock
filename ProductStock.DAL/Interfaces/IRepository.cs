using ProductStock.DL.Models;
using System;
using System.Collections.Generic;
namespace ProductStock.DAL.Interfaces
{
    using SWE.Model.Interfaces;
    using System.Threading.Tasks;

    public interface IRepository<T>
        where T : IKey
    {
        Task<IEnumerable<T>> Read();

        Task<T> Read(Guid id);

        Task<bool> Create(T item);

        Task<bool> Update(Guid id, T item);

        Task<bool> Delete(Guid id);
    }
}