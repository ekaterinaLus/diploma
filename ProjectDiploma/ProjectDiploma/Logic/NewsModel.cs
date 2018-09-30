using DataStore.Entities;
using DataStore.Repositories.NewRepository;
using Diploma.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDiploma.Logic
{
    public class NewsModel
    {
        private NewsRepository _newsRepository;

        public NewsModel(BusinessUniversityContext dbContext)
        {
            _newsRepository = new NewsRepository(dbContext);
        }

        public IEnumerable<News> GetNews()
        {
            return _newsRepository.GetSortedNews();
        }

        public News GetRandomNews()
        {   
            return _newsRepository.GetRandomEntities().FirstOrDefault();
        }
    }
}
