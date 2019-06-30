using ProjectDiploma.Entities;
using SharedLogic.Mapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStore.Entities
{
    public class ProjectViewHistory : IEntity, IMappable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }

        public int ProjectId { get; set; }
        public Project Project { get; set; }

        public DateTime ViewDate { get; set; }
    }
}
