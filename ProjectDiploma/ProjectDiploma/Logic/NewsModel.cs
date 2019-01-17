using DataStore.Entities;
using DataStore.Repositories.NewRepository;
using Diploma.DataBase;
using ProjectDiploma.ViewModel;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDiploma.Logic
{
    public class NewsModel
    {
        private readonly NewsRepository _newsRepository;

        public NewsModel(BusinessUniversityContext dbContext)
        {
            _newsRepository = new NewsRepository(dbContext);
        }

        //public IEnumerable<News> GetNews()
        //{
        //    return null; //_newsRepository.GetSortedNews();
        //}

        //public News GetRandomNews()
        //{
        //    return null;//_newsRepository.GetRandomEntities().FirstOrDefault();
        //}
        //public IEnumerable<News> GetNews()
        //{
        //    using (var dbContext = new BusinessUniversityContext())
        //    {
        //        return dbContext.News.OrderByDescending(x => x.Header);
        //    }
        //}

        public int GetNewsCount()
        {
            return _newsRepository.GetItemsCount();
        }

        public IEnumerable<NewsViewModel> GetPagingNews(int pageIndex, int pageSize)
        {
            var items = _newsRepository.GetPaging(pageIndex, pageSize).ToArray();
            return items.Select(item => NewsViewModel.FromDbObject(item));
        }
    }
}
