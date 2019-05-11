namespace DataStore.Entities.Projects
{
    public class ProjectRate 
    {
        public string UserId { get; set; }
        public User User { get; set; }

        public Project Project { get; set; }
        public int ProjectId { get; set; }

        public int Rate { get; set; }
    }
}
