using DataStore.Entities;
using Diploma.DataBase;
using ProjectDiploma.Entities;
using System.Linq;

namespace DataStore.Repositories.OrderedRepository
{
    public class OrderedByDateRepository<T> : GenericRepository<T>, IOrderedRepository<T> where T : class, IEntity, IDate
    {
        public OrderedByDateRepository(BusinessUniversityContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<T> GetAllOrderedAsc()
        {
            return GetAll().OrderBy(item => item.Date);
        }

        public IQueryable<T> GetAllOrderedDesc()
        {
            return GetAll().OrderByDescending(item => item.Date);
        }
    }
}
