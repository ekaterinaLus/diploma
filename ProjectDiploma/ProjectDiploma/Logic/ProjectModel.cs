using DataStore;
using DataStore.Entities;
using DataStore.Repositories.ProjectRepository;
using Diploma.DataBase;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Primitives;
using ProjectDiploma.ViewModel;
using SharedLogic.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ProjectDiploma.Logic
{
    public class ProjectModel : PagingModel<ProjectsRepository, Project, ProjectViewModel>
    {
        private static CancellationTokenSource _resetCacheToken = new CancellationTokenSource();

        public User User { get; set; }

        /// <summary>
        /// Признак того, нужно ли использовать нейронку при выводе результатов
        /// Например, для университета и для админа нейронка не используется.
        /// </summary>
        public bool IsBusiness { get; set; }

        private IMemoryCache MemoryCache { get; }

        protected override ProjectsRepository Repository { get; }
        protected BusinessUniversityContext DbContext { get; }

        public ProjectModel(BusinessUniversityContext dbContext, IMemoryCache memCache)
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

            if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested && _resetCacheToken.Token.CanBeCanceled)
            {
                _resetCacheToken.Cancel();
                _resetCacheToken.Dispose();
            }
            _resetCacheToken = new CancellationTokenSource();

            DbContext.SaveChanges();

            return new Response();
        }

        public Response Subscribe(int id)
        {
            var project = Repository.Get(id);

            if (project == null)
            {
                return new Response()
                            .AddMessage(MessageType.ERROR, "Объект не найден в базе");
            }

            if (project.Sponsors.Count > 0 && project.Sponsors.Any(x => x.CompanyId == (DbContext.Companies.FirstOrDefault(y => y.Employees.Contains(User))?.Id ?? -1)))
            {

            }
            else
            {
                project.Sponsors.Add(new ProjectsCompanies
                {
                    Company = DbContext.Companies.FirstOrDefault(x => x.Employees.Contains(User)),
                    Project = project
                });
            }

            DbContext.SaveChanges();

            return new Response();
        }

        public Response Add(ProjectViewModel projectViewModel)
        {
            var entityTags = projectViewModel.Tags.Select(x => x.ToType<Tag>());

            DbContext.Tags.AddUniqueElements(entityTags);
            DbContext.SaveChanges();

            var dbTags = DbContext.Tags.GetDatabaseElements(entityTags);

            foreach (var tag in dbTags)
            {
                var viewModelTag = projectViewModel.Tags.FirstOrDefault(x => x.Name.ToLower() == tag.Name.ToLower());
                viewModelTag.Id = tag.Id;
            }

            var dbEntity = projectViewModel.ToType<Project>();

            dbEntity.Initializer = DbContext.Universities.FirstOrDefault(x => x.Employees.Contains(User));

            Repository.Create(dbEntity);

            DbContext.SaveChanges();

            if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested && _resetCacheToken.Token.CanBeCanceled)
            {
                _resetCacheToken.Cancel();
                _resetCacheToken.Dispose();
            }
            _resetCacheToken = new CancellationTokenSource();

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

            if (_resetCacheToken != null && !_resetCacheToken.IsCancellationRequested && _resetCacheToken.Token.CanBeCanceled)
            {
                _resetCacheToken.Cancel();
                _resetCacheToken.Dispose();
            }
            _resetCacheToken = new CancellationTokenSource();

            return new Response<ProjectViewModel>(projectViewModel);
        }

        public IEnumerable<ProjectViewModel> GetProjectByUser()
        {
            var result = Repository.GetAll().Where(x => x.Initializer == DbContext.Universities.FirstOrDefault(y => y.Employees.Contains(User)));
            return result.Select(x => x.ToType<ProjectViewModel>());
        }

        public override IEnumerable<ProjectViewModel> GetPagingItems(int pageIndex, int pageSize)
        {
            if (User == null || !IsBusiness)
            {
                return base.GetPagingItems(pageIndex, pageSize); // Если сидим под анонимным пользователем - получаем проекты по-обычному
            }

            if (MemoryCache.TryGetValue(User.Id, out List<ProjectViewModel> cacheResult))
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
                }
                .AddExpirationToken(new CancellationChangeToken(_resetCacheToken.Token));
                
                foreach (var project in projects)
                {
                    var features = NeuralNetworkModel.ExtractFeatures(User, project);

                    var evalResult = nn.Evaluate(new NeuralNetwork.NeuralNetworkData
                    {
                        Features = features
                    });

                    nnResult.Add((project, evalResult));
                }

                var rates = DbContext.ProjectsRates.Where(x => x.UserId == User.Id);

                var result = nnResult
                        .OrderByDescending(x => x.Item2)
                        .Select(x =>
                        {
                            var res = x.Item1.ToType<ProjectViewModel>();
                            res.Rate = rates.FirstOrDefault(y => y.ProjectId == res.Id)?.Rate ?? -1;
                            return res;
                        })
                        .ToList();

                MemoryCache.Set(User.Id, result, options);

                return result.Skip(pageIndex * pageSize).Take(pageSize);
            }
        }
    }
}
