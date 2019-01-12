using ProjectDiploma.Entities;
using System.Linq;

namespace DataStore.Repositories
{
    public interface IGenericRepository<T> where T: class, IEntity
    {
        void Update(T element);
        IQueryable<T> GetAll();
        void Delete(T element);
        T Get(int id);
        void Create(T element);
    }
}
