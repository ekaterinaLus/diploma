using DataStore.Entities;
using DataStore.Repositories.ProjectRepository;
using Diploma.DataBase;
using Microsoft.Extensions.Caching.Memory;
using ProjectDiploma.ViewModel;
using SharedLogic.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDiploma.Logic
{
    public class ProjectModel : PagingModel<ProjectsRepository, Project, ProjectViewModel>
    {
        public string UserId { get; set; }

        private MemoryCache MemoryCache { get; }

        protected override ProjectsRepository Repository { get; }
        //TODO:
        protected BusinessUniversityContext DbContext { get; }

        public ProjectModel(BusinessUniversityContext dbContext, MemoryCache memCache)
        { 
            Repository = new ProjectsRepository(dbContext);
            DbContext = dbContext;
            MemoryCache = memCache;
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

            if (MemoryCache.TryGetValue(UserId, out List<ProjectViewModel> cachingResult))
            {
                cachingResult.RemoveAll(x => x.Id == id);

                MemoryCache.Set(UserId, cachingResult, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            DbContext.SaveChanges();

            return new Response();
        }

        public Response Add(ProjectViewModel projectViewModel)
        {
            var dbEntity = projectViewModel.ToType<Project>();

            dbEntity.Initializer = DbContext.Universities.Find(1);

            Repository.Create(dbEntity);

            DbContext.SaveChanges();

            if (MemoryCache.TryGetValue(UserId, out List<ProjectViewModel> cachingResult))
            {
                cachingResult.Add(dbEntity.ToType<ProjectViewModel>());

                MemoryCache.Set(UserId, cachingResult, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

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

            if (MemoryCache.TryGetValue(UserId, out List<ProjectViewModel> cachingResult))
            {
                cachingResult.Add(item.ToType<ProjectViewModel>());

                MemoryCache.Set(UserId, cachingResult, new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                });
            }

            return new Response<ProjectViewModel>(projectViewModel);
        }

        public override IEnumerable<ProjectViewModel> GetPagingItems(int pageIndex, int pageSize)
        {
            if (MemoryCache.TryGetValue(UserId, out List<ProjectViewModel> cacheResult))
            {
                return cacheResult.Skip(pageIndex * pageSize).Take(pageSize);
            }
            else
            {
                var projects = Repository.GetAllOrderedDesc().ToList();
                var nn = NeuralNetwork.NeuralNetwork.GetNeuralNetwork();
                var nnResult = new List<(Project, double)>(projects.Count);
                var options = new MemoryCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(10)
                };

                foreach (var project in projects)
                {
                    var features = NeuralNetworkModel.ExtractFeatures(UserId, project);

                    var evalResult = nn.Evaluate(new NeuralNetwork.NeuralNetworkData
                    {
                        Features = features
                    });

                    nnResult.Add((project, evalResult));
                }

                var result = nnResult.OrderByDescending(x => x.Item2).Select(x => x.Item1.ToType<ProjectViewModel>()).ToList();

                MemoryCache.Set(UserId, result, options);

                return result.Skip(pageIndex * pageSize).Take(pageSize);
            }
        }
    }
}
