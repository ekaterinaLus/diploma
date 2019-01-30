using DataStore.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Diploma.DataBase
{
    public class BusinessUniversityContext : IdentityDbContext<User>
    {
        public enum RoleValues
        {
            ADMIN,
            UNIVERSITY,
            BUSINESS
        }

        public DbSet<Event> Events { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<NewsType> NewsTypes { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<University> Universities { get; set; }
        public DbSet<Company> Companies { get; set; }

        public BusinessUniversityContext(DbContextOptions<BusinessUniversityContext> options) : base(options)
        {}        

        public BusinessUniversityContext()
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<EventsTags>()
                .HasKey(x => new { x.EventId, x.TagId });

            modelBuilder.Entity<EventsTags>()
            .HasOne(pt => pt.Event)
            .WithMany(p => p.Tags)
            .HasForeignKey(pt => pt.EventId);

            modelBuilder.Entity<EventsTags>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.Events)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<NewsTags>()
                .HasKey(x => new { x.NewsId, x.TagId });

            modelBuilder.Entity<NewsTags>()
           .HasOne(pt => pt.News)
           .WithMany(p => p.Tags)
           .HasForeignKey(pt => pt.NewsId);

            modelBuilder.Entity<NewsTags>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.News)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<ProjectsTags>()
                .HasKey(x => new { x.ProjectId, x.TagId });

            modelBuilder.Entity<ProjectsTags>()
           .HasOne(pt => pt.Project)
           .WithMany(p => p.Tags)
           .HasForeignKey(pt => pt.ProjectId);

            modelBuilder.Entity<ProjectsTags>()
                .HasOne(pt => pt.Tag)
                .WithMany(t => t.Projects)
                .HasForeignKey(pt => pt.TagId);

            modelBuilder.Entity<ProjectsCompanies>()
                .HasKey(x => new { x.ProjectId, x.CompanyId });

            modelBuilder.Entity<ProjectsCompanies>()
    .HasOne(pt => pt.Project)
    .WithMany(p => p.Sponsors)
    .HasForeignKey(pt => pt.ProjectId);

            modelBuilder.Entity<ProjectsCompanies>()
                .HasOne(pt => pt.Company)
                .WithMany(t => t.Projects)
                .HasForeignKey(pt => pt.CompanyId);

            base.OnModelCreating(modelBuilder);
         
        }


    }
}
 