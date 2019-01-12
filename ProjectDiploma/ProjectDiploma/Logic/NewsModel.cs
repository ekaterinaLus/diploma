using DataStore.Entities;
using DataStore.Repositories.NewRepository;
using Diploma.DataBase;
using System.Collections.Generic;

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
            return null; //_newsRepository.GetSortedNews();
        }

        public News GetRandomNews()
        {
            return null;//_newsRepository.GetRandomEntities().FirstOrDefault();
        }
    }
}
