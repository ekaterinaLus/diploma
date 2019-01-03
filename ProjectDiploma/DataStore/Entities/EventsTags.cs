using System.ComponentModel.DataAnnotations;

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
