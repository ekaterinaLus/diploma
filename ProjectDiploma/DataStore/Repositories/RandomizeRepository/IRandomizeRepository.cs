using ProjectDiploma.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStore.Repositories.RandomizeRepository
{
    public interface IRandomizeRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        IEnumerable<T> GetRandomEntities(int count = 1);
    }
}
