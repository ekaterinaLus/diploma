using DataStore.Entities;
using DataStore.Repositories;
using Diploma.DataBase;
using Microsoft.EntityFrameworkCore;
using ProjectDiploma.ViewModel;
using SharedLogic.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectDiploma.Logic
{
    public class TagModel
    {
        private GenericRepository<Tag> Repository { get; }
        private BusinessUniversityContext DbContext { get; }

        public TagModel(BusinessUniversityContext dbContext)
        {
            Repository = new GenericRepository<Tag>(dbContext);
            DbContext = dbContext;
        }

        public Response<TagViewModel[]> Get(int count, string name)
        {
            var query = Repository
                            .GetAll()
                            .AsNoTracking();

            if (!string.IsNullOrEmpty(name))
            {
                query = query.Where(item => item.Name.StartsWith(name));
            }

            query = query
                        .OrderBy(item => name)
                        .Take(count);

            var result = query                            
                            .ToArray()
                            .Select(item => item.ToType<TagViewModel>())
                            .ToArray();
            return new Response<TagViewModel[]>(result);
        }
    }
}
