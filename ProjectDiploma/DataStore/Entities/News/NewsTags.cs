using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DataStore.Entities
{
    public class NewsTags
    {
        [Key]
        public int NewsId { get; set; }

        public News News { get; set; }

        [Key]
        public int TagId { get; set; }

        public Tag Tag { get; set; }
    }
}
