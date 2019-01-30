using System.ComponentModel.DataAnnotations;

namespace DataStore.Entities
{
    public class ProjectsCompanies
    {

        public int ProjectId { get; set; }
        public Project Project { get; set; }


        public int CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
