using System.ComponentModel.DataAnnotations;

namespace DataStore.Entities
{
    public class ProjectsCompanies
    {
        [Key]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        [Key]
        public int CompanyId { get; set; }

        public Company Company { get; set; }
    }
}
