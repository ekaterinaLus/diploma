using ProjectDiploma.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace DataStore.Entities
{
    public class News : IEntity, IDate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Header { get; set; }

        public string Annotation { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Text { get; set; }

        public virtual ICollection<NewsTags> Tags { get; set; } = new HashSet<NewsTags>();

        public NewsType Section { get; set; }

        public int? SectionId { get; set; }
    }
}
