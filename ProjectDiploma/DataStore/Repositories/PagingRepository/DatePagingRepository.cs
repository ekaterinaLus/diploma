using DataStore.Repositories.OrderedRepository;
using Diploma.DataBase;
using ProjectDiploma.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using DataStore.Entities;

namespace DataStore.Repositories.PagingRepository
{
    public abstract class DatePagingRepository<T> : OrderedByDateRepository<T>, IPagingRepository<T>, IOrderedRepository<T> where T : class, IEntity, IDate
    {
        public DatePagingRepository(BusinessUniversityContext dbContext) : base(dbContext)
        {
        }

        public int GetItemsCount()
        {
            return GetAll().AsNoTracking().Count();
        }

        public IQueryable<T> GetPaging(int pageIndex, int pageSize)
        {
            return GetAllOrderedDesc()
                    .Skip(pageIndex * pageSize)
                    .Take(pageSize);
        }
    }
}
