using ProjectDiploma.Entities;
using SharedLogic.Mapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStore.Entities
{
    public class Company : IEntity, IOrganization, IMappable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(2000)]
        public string ContactInformation { get; set; }

        public ICollection<ProjectsCompanies> Projects { get; set; } = new HashSet<ProjectsCompanies>();
        public virtual ICollection<User> Employees { get; set; } = new HashSet<User>();
    }
}
