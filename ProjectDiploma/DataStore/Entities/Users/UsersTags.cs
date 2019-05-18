namespace DataStore.Entities
{
    public class UsersTags
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public int TagId { get; set; }
        public Tag Tag { get; set; }
    }
}
