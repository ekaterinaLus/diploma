using DataStore.Entities;
using DataStore.Repositories.OrderedRepository;
using DataStore.Repositories.PagingRepository;

namespace DataStore.Repositories.NewsRepository
{
    public interface INewsRepository : IOrderedRepository<News>, IPagingRepository<News>
    { 
    }
}
