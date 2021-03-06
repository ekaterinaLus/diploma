﻿using Diploma.DataBase;
using ProjectDiploma.Entities;
using System.Linq;

namespace DataStore.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T: class, IEntity
    {
        protected BusinessUniversityContext DbContext { get; set; }

        public GenericRepository(BusinessUniversityContext dbContext)
        {
            DbContext = dbContext;
        }
         
        public virtual void Create(T element)
        {
            DbContext.Set<T>().Add(element);
        }

        public virtual void Delete(T element)
        {
            DbContext.Set<T>().Remove(element);
        }

        public virtual T Get(int id)
        {
            return DbContext.Set<T>().Find(id);
        }

        public virtual void Update(T element)
        {
            DbContext.Set<T>().Update(element); 
        }

        public virtual IQueryable<T> GetAll()
        {
            return DbContext.Set<T>();
        }

    }
}
