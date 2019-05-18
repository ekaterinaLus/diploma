using ProjectDiploma.Entities;
using SharedLogic.Mapper;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DataStore.Entities
{
    public class Tag : IEntity, IMappable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [MaxLength(30)]
        public string Name { get; set; }

        public virtual ICollection<NewsTags> News { get; set; } = new HashSet<NewsTags>();

        public virtual ICollection<EventsTags> Events { get; set; } = new HashSet<EventsTags>();

        public virtual ICollection<ProjectsTags> Projects { get; set; } = new HashSet<ProjectsTags>();

        public virtual ICollection<UsersTags> Users { get; set; } = new HashSet<UsersTags>();
    }
}
