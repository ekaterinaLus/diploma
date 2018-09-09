
using ProjectDiploma.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace DataStore.Entities
{
    public class Tag : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public virtual ICollection<NewsTags> News { get; set; }

        public virtual ICollection<EventsTags> Events { get; set; }

        public Tag()
        {
            News = new HashSet<NewsTags>();
            Events = new HashSet<EventsTags>();
        }
    }
}
