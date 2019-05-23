using DataStore.Entities;
using Diploma.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace DataStore.Repositories
{
    public class UserRepository
    {        
        protected BusinessUniversityContext DbContext { get; set; }

        public UserRepository(BusinessUniversityContext dbContext)
        {
            DbContext = dbContext;
        }

        public User Get(string id)
        {
            return GetAll().FirstOrDefault(x => x.Id == id);
        }

        public IEnumerable<User> GetAll()
        {
            return DbContext.Users.Include(x => x.Tags);
        }

    }
}



