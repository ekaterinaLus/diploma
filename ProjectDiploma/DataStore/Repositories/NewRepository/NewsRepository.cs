using DataStore.Entities;
using DataStore.Repositories.PagingRepository;
using Diploma.DataBase;
using System.Linq;

namespace DataStore.Repositories.NewRepository
{
    public class NewsRepository : DatePagingRepository<News>, INewsRepository
    {
        public NewsRepository(BusinessUniversityContext dbContext) : base(dbContext) { }

        public override IQueryable<News> GetAll()
        {
            return DbContext.News;
        }
    }
}



