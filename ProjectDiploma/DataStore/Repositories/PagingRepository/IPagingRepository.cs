using ProjectDiploma.Entities;
using System.Linq;

namespace DataStore.Repositories.PagingRepository
{
    public interface IPagingRepository<T>: IGenericRepository<T> where T : class, IEntity
    {
        IQueryable<T> GetPaging(int startIndex, int endIndex);
        int GetItemsCount();
    }
}
