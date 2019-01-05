using Diploma.DataBase;
using ProjectDiploma.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataStore.Repositories.RandomizeRepository
{
    public class RandomizeRepository<T> : GenericRepository<T>, IRandomizeRepository<T> where T : class, IEntity
    {
        private static Random rnd = new Random();

        public RandomizeRepository(BusinessUniversityContext dbContext) : base(dbContext)
        {
        }

        public IEnumerable<T> GetRandomEntities(int count = 1)
        {
            return GetAll()
                    .OrderBy(x => rnd.Next())
                    .Take(count);
        }
    }
}
