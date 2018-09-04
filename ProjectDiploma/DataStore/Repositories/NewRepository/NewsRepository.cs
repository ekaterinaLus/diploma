using DataStore.Entities;
using DiplomaBU.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace DataStore.Repositories.NewRepository
{
    public class NewsRepository : GenericRepository<News>, INewsRepository
    {
        public NewsRepository(BusinessUniversityContext dbContext) : base(dbContext) { }

        public override IEnumerable<News> GetAll()
        {
            return DbContext.
                News.Include(x => x.Tags).
                ThenInclude(x => x.Tags);
        }
    }
}



