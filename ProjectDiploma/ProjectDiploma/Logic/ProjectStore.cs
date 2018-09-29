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
        public static BusinessUniversityContext context;

        public static IEnumerable<Project> GetProject()
        {
            using (var dbContext = new BusinessUniversityContext())
            {
                return dbContext.Projects.OrderByDescending(x => x.Start);
            }
        }

        public static Project GetRndmProject()
        {
                return context.Projects.OrderBy(x => rnd.Next()).FirstOrDefault();
        }

        public static bool PutProject(Project project)
        {
            try
            {
                using (var dbContext = new BusinessUniversityContext())
                {
                    dbContext.Projects.Add(project);
                    dbContext.SaveChanges();
                }
                return true;
            }
            catch
            {
                return false;
            }

        }

       
    }
}
