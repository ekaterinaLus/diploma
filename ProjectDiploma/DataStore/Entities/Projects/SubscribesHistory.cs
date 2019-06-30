using ProjectDiploma.Entities;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStore.Entities.Projects
{
    public class SubscribesHistory : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public string UserId { get; set; }

        public User User { get; set; }

        public int ViewsCount { get; set; }
    }
}
