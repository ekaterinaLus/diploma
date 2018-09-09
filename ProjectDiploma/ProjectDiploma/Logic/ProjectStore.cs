using DataStore.Entities;
using Diploma.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDiploma.Logic
{
    public class ProjectStore
    {
        public static Random rnd = new Random();

        public static IEnumerable<Project> GetProject()
        {
            using (var dbContext = new BusinessUniversityContext())
            {
                return dbContext.Projects.OrderByDescending(x => x.Start);
            }
        }

        public static Project GetRndmProject()
        {
            using (var dbContext = new BusinessUniversityContext())
            {
                return dbContext.Projects.OrderBy(x => rnd.Next()).FirstOrDefault();
            }
        }
    }
}
