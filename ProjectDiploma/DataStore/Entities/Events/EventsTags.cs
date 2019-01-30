using System.ComponentModel.DataAnnotations;

namespace DataStore.Entities
{
    public class EventsTags
    {
       
        public int EventId { get; set; }
        public Event Event { get; set; }


        public int TagId { get; set; } 
        public Tag Tag { get; set; }        
    }
    
}
