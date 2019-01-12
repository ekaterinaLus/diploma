using ProjectDiploma.Entities;
using System.Linq;

namespace DataStore.Repositories.OrderedRepository
{
    public interface IOrderedRepository<T> : IGenericRepository<T> where T: class, IEntity
    {
        IQueryable<T> GetAllOrderedAsc();
        IQueryable<T> GetAllOrderedDesc();
    }
}
