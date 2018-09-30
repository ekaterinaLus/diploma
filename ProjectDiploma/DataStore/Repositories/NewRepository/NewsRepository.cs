using DataStore.Entities;
using DataStore.Repositories.RandomizeRepository;
using Diploma.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataStore.Repositories.NewRepository
{
    public class NewsRepository : RandomizeRepository<News>, INewsRepository
    {
        public NewsRepository(BusinessUniversityContext dbContext) : base(dbContext) { }

        public override IEnumerable<News> GetAll()
        {
            return DbContext.
                News.Include(x => x.Tags).
                ThenInclude(x => x.Tags);
        }

        public IEnumerable<News> GetSortedNews()
        {
            return GetAll().OrderByDescending(x => x.Header);
        }
    }
}



