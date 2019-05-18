using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DataStore.Entities
{
    public class User : IdentityUser
    {
        public ICollection<UsersTags> Tags { get; set; } = new HashSet<UsersTags>(); //не более 10 тегов на пользователя
    }
}
