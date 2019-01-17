using ProjectDiploma.Entities;
using SharedLogic.Mapper;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStore.Entities
{
    public class Project : IEntity, IDate, IMappable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? Finish { get; set; }

        [Required]
        public decimal Cost { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}

