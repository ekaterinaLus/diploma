using DataStore.Entities;
using DataStore.Repositories.NewRepository;
using Diploma.DataBase;
using ProjectDiploma.ViewModel;

namespace ProjectDiploma.Logic
{
    public class NewsModel : PagingModel<NewsRepository, News, NewsViewModel>
    {
        private readonly NewsRepository _newsRepository;

        protected override NewsRepository Repository => _newsRepository;

        public NewsModel(BusinessUniversityContext dbContext)
        {
            _newsRepository = new NewsRepository(dbContext);
        }     
    }
}
