using DataStore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diploma.DataBase
{
    public class BusinessUniversityContext : IdentityDbContext<User>
    {
        public struct RoleName
        {
            public const string ADMIN_ROLE_NAME = "ADMIN";
            public const string UNIVERSITY_ROLE_NAME = "UNIVERSITY";
            public const string BUSINESS_ROLE_NAME = "BUSINESS";

            public static bool CheckRole(string roleName)
            {
                switch (roleName)
                {
                    case ADMIN_ROLE_NAME:
                    case UNIVERSITY_ROLE_NAME:
                    case BUSINESS_ROLE_NAME:
                        return true;
                    default:
                        return false;
                }
            }
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsType> NewsTypes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }

        public BusinessUniversityContext(DbContextOptions<BusinessUniversityContext> options) : base(options)
        {}        

        public BusinessUniversityContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventsTags>()
                .HasKey(x => new { x.EventId, x.TagsId });

            modelBuilder.Entity<NewsTags>()
                .HasKey(x => new { x.NewsId, x.TagsId });
            base.OnModelCreating(modelBuilder);
        }
    }
}
 