using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataStore.Entities
{
    public class EventsTags
    {
        [Key]
        public int EventId { get; set; }

        public Event Events { get; set; }

        [Key]
        public int TagsId { get; set; }

        public Tag Tags { get; set; }

        
    }
    
}
