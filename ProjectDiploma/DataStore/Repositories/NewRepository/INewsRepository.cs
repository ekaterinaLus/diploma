using DataStore.Entities;
using DataStore.Repositories.RandomizeRepository;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataStore.Repositories.NewRepository
{
    public interface INewsRepository: IRandomizeRepository<News>
    {
        IEnumerable<News> GetSortedNews();
    }
}
