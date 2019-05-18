using DataStore.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DataStore
{
    public static class DBHelper
    {
        private static readonly Dictionary<Type, dynamic> _comparersList = new Dictionary<Type, dynamic>
        {
            { typeof(Tag), new GenericComparer<Tag> { GetComparableField = x => x.Name.ToLower() } },
            { typeof(News), new GenericComparer<News> { GetComparableField = x => x.Header } },
            { typeof(Event), new GenericComparer<Event> { GetComparableField = x => x.Title } },
            { typeof(NewsType), new GenericComparer<NewsType> { GetComparableField = x => x.Name } },
            { typeof(Project), new GenericComparer<Project> { GetComparableField = x => x.Name }},
            { typeof(University), new GenericComparer<University> { GetComparableField = x => x.Name } }
        };

        public static async Task AddUniqueElementsAsync<T>(this DbSet<T> @this, IEnumerable<T> addingElements)
            where T : class
        {
            IEqualityComparer<T> comparer = _comparersList[typeof(T)];
            IEnumerable<T> elementsInDb = await @this.AsNoTracking().ToArrayAsync();
            await @this.AddRangeAsync(addingElements.Except(elementsInDb.Intersect(addingElements, comparer), comparer));
        }

        public static async Task<IEnumerable<T>> GetDatabaseElementsAsync<T>(this DbSet<T> @this, IEnumerable<T> localElements)
            where T : class
        {
            IEqualityComparer<T> comparer = _comparersList[typeof(T)];
            IEnumerable<T> elementsInDb = await @this.ToArrayAsync();
            return elementsInDb.Where(item => localElements.Contains(item, comparer));
        }

        public static void AddUniqueElements<T>(this DbSet<T> @this, IEnumerable<T> addingElements)
            where T : class
        {
            IEqualityComparer<T> comparer = _comparersList[typeof(T)];
            IEnumerable<T> elementsInDb = @this.AsNoTracking().ToArray();
            @this.AddRange(addingElements.Except(elementsInDb.Intersect(addingElements, comparer), comparer));
        }

        public static IEnumerable<T> GetDatabaseElements<T>(this DbSet<T> @this, IEnumerable<T> localElements)
            where T : class
        {
            IEqualityComparer<T> comparer = _comparersList[typeof(T)];
            IEnumerable<T> elementsInDb = @this.ToArray();
            return elementsInDb.Where(item => localElements.Contains(item, comparer));
        }
    }
}
