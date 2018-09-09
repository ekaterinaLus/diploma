using ProjectDiploma.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace DataStore.Entities
{
    public class Project : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public DateTime? Start { get; set; }

        public DateTime? Finish { get; set; }

        [Required]
        public decimal Cost { get; set; }
    }
}

