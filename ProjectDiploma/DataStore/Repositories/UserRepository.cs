/*using Diploma.DataBase;
using Diploma.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStore.Repositories
{
    class UserRepository<User> : IGenericRepository<User> where User : class, IEntity
    {
        public int Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected BusinessUniversityContext DbContext { get; set; }


        public User Create(User element)
        {
            return DbContext.Set<User>().Add(element).Entity;
        }

        public void Delete(User element)
        {
            DbContext.Set<User>().Remove(element);
        }

        public User Get(int id)
        {
            return DbContext.Set<User>().Find(id);
        }

        public IEnumerable<User> GetAll()
        {
            return DbContext.Set<User>();
        }

        public void Update(User element)
        {
            DbContext.Set<User>().Update(element);
        }
    }
}*/



