using DataStore.Entities;
using Diploma.DataBase;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DataStore.Repositories
{
    public class ProjectViewHistoryRepository : GenericRepository<ProjectViewHistory>
    {
        public ProjectViewHistoryRepository(BusinessUniversityContext dbContext) : base(dbContext)
        {
        }

        public override IQueryable<ProjectViewHistory> GetAll()
        {
            return DbContext.ProjectViewHistories
                .Include(x => x.Company)
                .Include(x => x.Project);
        }
    }
}
