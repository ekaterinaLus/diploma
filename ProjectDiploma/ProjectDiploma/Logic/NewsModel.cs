using DataStore.Entities;
using DataStore.Repositories.NewsRepository;
using Diploma.DataBase;
using ProjectDiploma.ViewModel;
using SharedLogic.Mapper;

namespace ProjectDiploma.Logic
{
    public class NewsModel : PagingModel<NewsRepository, News, NewsViewModel> //PagingModel<NewsRepository, News, NewsViewModel> 
    {
        protected override NewsRepository Repository { get; }

        //TODO:
        private BusinessUniversityContext DbContext { get; } 
       
        public NewsModel(BusinessUniversityContext dbContext)
        {
            Repository = new NewsRepository(dbContext);
            DbContext = dbContext;
        }

        //TODO:

        public NewsViewModel Get(int id)
        {
            var res = Repository.Get(id);
            return res.ToType<NewsViewModel>();
        }

    }
}
