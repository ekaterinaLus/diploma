using System.ComponentModel.DataAnnotations;

namespace DataStore.Entities
{
    public class NewsTags
    {
        public int NewsId { get; set; }
  
        public News News { get; set; }

        public int TagId { get; set; }
 
        public Tag Tag { get; set; }
    }
}
