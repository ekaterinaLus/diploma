﻿using Diploma.DataBase;
using ProjectDiploma.Entities;
using System;
using System.Linq;

namespace DataStore.Repositories.RandomizeRepository
{
    public class RandomizeRepository<T> : GenericRepository<T>, IRandomizeRepository<T> where T : class, IEntity
    {
        private static Random rnd = new Random();

        public RandomizeRepository(BusinessUniversityContext dbContext) : base(dbContext)
        {
        }

        public IQueryable<T> GetRandomEntities(int count = 1)
        {
            return GetAll()
                    .OrderBy(x => rnd.Next())
                    .Take(count);
        }
    }
}
