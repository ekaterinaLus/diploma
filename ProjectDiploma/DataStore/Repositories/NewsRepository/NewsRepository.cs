using DataStore.Entities;
using DataStore.Repositories.PagingRepository;
using Diploma.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataStore.Repositories.NewsRepository
{
    public class NewsRepository : DatePagingRepository<News>, INewsRepository
    {
        public NewsRepository(BusinessUniversityContext dbContext) : base(dbContext) { }

        public override IQueryable<News> GetAll()
        {
            return DbContext.News.Include(x => x.Tags)
                .ThenInclude(x => x.Tag); 
        }
    }
}



