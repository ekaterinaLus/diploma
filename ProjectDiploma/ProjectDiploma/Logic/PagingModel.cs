using DataStore.Repositories.PagingRepository;
using ProjectDiploma.Entities;
using SharedLogic.Mapper;
using System.Collections.Generic;
using System.Linq;

namespace ProjectDiploma.Logic
{
    public abstract class PagingModel<TRepository, TDbEntity, TViewModel>
        where TRepository: IPagingRepository<TDbEntity>
        where TDbEntity: class, IEntity, IMappable
        where TViewModel: class, IMappable
    {
        protected abstract TRepository Repository { get; }
                
        public int GetItemsCount() => Repository.GetItemsCount();

        public IEnumerable<TViewModel> GetPagingItems(int pageIndex, int pageSize)
        {
            var items = Repository.GetPaging(pageIndex, pageSize).ToArray();
            return items.Select(item => item.ToType<TViewModel>());
        }
    }
}
