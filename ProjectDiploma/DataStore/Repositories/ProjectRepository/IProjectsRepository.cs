using DataStore.Entities;
using DataStore.Repositories.OrderedRepository;
using DataStore.Repositories.PagingRepository;

namespace DataStore.Repositories.ProjectRepository
{
    public interface IProjectsRepository : IOrderedRepository<Project>, IPagingRepository<Project>
    {
    }
}
