using DataStore.Entities;
using DataStore.Repositories.PagingRepository;
using Diploma.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataStore.Repositories.ProjectRepository
{
    public class ProjectsRepository : DatePagingRepository<Project>, IProjectsRepository
    {
        public ProjectsRepository(BusinessUniversityContext dbContext) : base(dbContext) { }

        public override IQueryable<Project> GetAll()
        {
            return DbContext.Projects
                .Include(x => x.Sponsors).ThenInclude(x => x.Company)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.Initializer);
        }

        public override Project Get(int id)
        {
            return DbContext.Projects
                .Include(x => x.Sponsors).ThenInclude(x => x.Company)
                .Include(x => x.Tags).ThenInclude(x => x.Tag)
                .Include(x => x.Initializer)
                .FirstOrDefault(x => x.Id == id);
        }
    }
}
