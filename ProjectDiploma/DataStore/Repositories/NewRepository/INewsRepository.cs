using DataStore.Entities;
using DataStore.Repositories.OrderedRepository;
using DataStore.Repositories.PagingRepository;

namespace DataStore.Repositories.NewRepository
{
    public interface INewsRepository : IOrderedRepository<News>, IPagingRepository<News>
    { 
    }
}
