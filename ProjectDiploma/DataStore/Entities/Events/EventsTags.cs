using System.ComponentModel.DataAnnotations;

namespace DataStore.Entities
{
    public class EventsTags
    {
        [Key]
        public int EventId { get; set; }

        public Event Event { get; set; }

        [Key]
        public int TagId { get; set; }

        public Tag Tag { get; set; }        
    }
    
}
