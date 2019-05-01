using ProjectDiploma.Entities;
using SharedLogic.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStore.Entities
{
    public class Project : IEntity, IDate, IMappable
    {
        public enum ProjectStage
        {
            Founding,
            Start,
            Prototype,
            PreProduction,
            Production
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(300)]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public string Risks { get; set; }

        [Required]
        public ProjectStage Stage { get; set; } // +

        public DateTime? StartDate { get; set; } // +

        public DateTime? FinishDate { get; set; } // +

        [Required]
        public decimal CostCurrent { get; set; } // +

        [Required]
        public decimal CostFull { get; set; } // +

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public bool IsClosed { get; set; }

        [Required]
        public University Initializer { get; set; } // +

        public string FileName { get; set; }

        public  ICollection<ProjectsCompanies> Sponsors { get; set; } = new HashSet<ProjectsCompanies>();

        public ICollection<ProjectsTags> Tags { get; set; } = new HashSet<ProjectsTags>(); //не более 10 тегов на проект
    }
}

