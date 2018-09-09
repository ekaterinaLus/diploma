using ProjectDiploma.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

namespace DataStore.Entities
{
    //Время дата заголовок описание адрес цена

    public class Event : IEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public string Adress { get; set; }

        public decimal? Cost { get; set; }

        public virtual ICollection<EventsTags> Tags { get; set; }

        public Event()
        {
            Tags = new HashSet<EventsTags>();
        }
    }
}
