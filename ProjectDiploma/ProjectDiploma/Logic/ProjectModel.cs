using DataStore.Entities;
using DataStore.Repositories.ProjectRepository;
using Diploma.DataBase;
using ProjectDiploma.ViewModel;
using SharedLogic.Mapper;

namespace ProjectDiploma.Logic
{
    public class ProjectModel : PagingModel<ProjectsRepository, Project, ProjectViewModel>
    {
        protected override ProjectsRepository Repository { get; }
        //TODO:
        protected BusinessUniversityContext DbContext { get; }

        public ProjectModel(BusinessUniversityContext dbContext)
        {
            Repository = new ProjectsRepository(dbContext);
            DbContext = dbContext;
        }       

        //TODO: class - CRUDModel (CRUD - Create Read Update Delete)
        public Response Get(int id)
        {
            var item = Repository.Get(id);

            if (item == null)
            {
                return new Response()
                            .AddMessage(MessageType.ERROR, "Объект не найден в базе");
            }

            return new Response<ProjectViewModel>(item.ToType<ProjectViewModel>());
        }

        public Response Delete(int id)
        {
            var item = Repository.Get(id);

            if (item == null)
            {
                return new Response()
                            .AddMessage(MessageType.ERROR, "Объект не найден в базе");
            }

            Repository.Delete(item);

            DbContext.SaveChanges();

            return new Response();
        }

        public Response Add(ProjectViewModel projectViewModel)
        {
            var dbEntity = projectViewModel.ToType<Project>();

            dbEntity.Initializer = DbContext.Universities.Find(1);

            Repository.Create(dbEntity);

            DbContext.SaveChanges();

            return new Response<ProjectViewModel>(dbEntity.ToType<ProjectViewModel>());
        }

        public Response Update(ProjectViewModel projectViewModel)
        {
            var item = Repository.Get(projectViewModel.Id);

            if (item == null)
            {
                return new Response()
                            .AddMessage(MessageType.ERROR, "Объект не найден в базе");
            }

            Repository.Update(projectViewModel.ToType<Project>());

            DbContext.SaveChanges();

            return new Response<ProjectViewModel>(projectViewModel);
        }
    }
}
