using Diploma.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStore.Repositories
{
    public interface IGenericRepository<T> where T: class, IEntity
    {
        void Update(T element);
        IEnumerable<T> GetAll();
        void Delete(T element);
        T Get(int id);
        void Create(T element);
    }
}
