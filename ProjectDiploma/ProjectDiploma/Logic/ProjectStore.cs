using DataStore.Entities;
using DataStore.Repositories.ProjectRepository;
using Diploma.DataBase;
using ProjectDiploma.ViewModel;

namespace ProjectDiploma.Logic
{
    public class ProjectModel : PagingModel<ProjectsRepository, Project, ProjectViewModel>
    {
        private readonly ProjectsRepository _projectRepository;

        protected override ProjectsRepository Repository => _projectRepository;

        public ProjectModel(BusinessUniversityContext dbContext)
        {
            _projectRepository = new ProjectsRepository(dbContext);
        }       
    }
}
