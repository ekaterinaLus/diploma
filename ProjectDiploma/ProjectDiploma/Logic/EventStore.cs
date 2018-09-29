using DataStore.Entities;
using Diploma.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDiploma.Logic
{
    public class EventStore
    {
        public static Random rnd = new Random();
        public static BusinessUniversityContext context;
        public static Event GetRndmEvent()
        {
                return context.Events.OrderBy(x => rnd.Next()).FirstOrDefault();
        }
        public static IEnumerable<Event> GtRndmEvnt()
        {
            using (var dbContext = new BusinessUniversityContext())
            {
                return dbContext.Events.OrderBy(x => rnd.Next()).Take(2);
            }
        }
    }
}
