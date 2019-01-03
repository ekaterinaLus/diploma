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
        public int TagsId { get; set; }

        public Tag Tags { get; set; }

        [NotMapped]
        public object Select { get; internal set; }
    }
}
