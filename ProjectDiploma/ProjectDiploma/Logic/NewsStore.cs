﻿using DataStore.Entities;
using Diploma.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDiploma.Logic
{
    public class NewsStore
    {
        public static Random rnd = new Random();
        public static BusinessUniversityContext context;

        public static IEnumerable<News> GetNews()
        {
            using (var dbContext = new BusinessUniversityContext())
            {
                return dbContext.News.OrderByDescending(x => x.Header);
            }
        }

        public static  News GetRndmNews()
        {   
            return context.News.OrderBy(x => rnd.Next()).FirstOrDefault();
        }
    }
}
