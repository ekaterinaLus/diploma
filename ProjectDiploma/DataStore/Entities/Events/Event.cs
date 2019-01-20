using ProjectDiploma.Entities;
using SharedLogic.Mapper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Threading;

namespace DataStore.Entities
{
    //Время дата заголовок описание адрес цена

    public class Event : IEntity, IDate, IMappable
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        [MaxLength(150)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [MaxLength(300)]
        public string Address { get; set; }

        public decimal? Cost { get; set; }

        public virtual ICollection<EventsTags> Tags { get; set; } = new HashSet<EventsTags>();
    }
}
