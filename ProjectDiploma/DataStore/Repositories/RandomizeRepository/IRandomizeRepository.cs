using ProjectDiploma.Entities;
using System.Linq;

namespace DataStore.Repositories.RandomizeRepository
{
    public interface IRandomizeRepository<T> : IGenericRepository<T> where T : class, IEntity
    {
        IQueryable<T> GetRandomEntities(int count = 1);
    }
}
