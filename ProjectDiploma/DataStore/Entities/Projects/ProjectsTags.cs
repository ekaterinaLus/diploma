using System.ComponentModel.DataAnnotations;

namespace DataStore.Entities
{
    public class ProjectsTags
    {
        [Key]
        public int ProjectId { get; set; }

        public Project Project { get; set; }

        [Key]
        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
