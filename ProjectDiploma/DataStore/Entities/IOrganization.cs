using SharedLogic.Mapper;
using System.Collections.Generic;

namespace DataStore.Entities
{
    public interface IOrganization : IMappable
    {
        string Name { get; set; }

        string ContactInformation { get; set; }

        ICollection<User> Employees { get; set; }
    }
}
