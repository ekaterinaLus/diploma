using DataStore.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace DiplomaBU.DataBase
{
    public class BusinessUniversityContext : DbContext
    {
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

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=BusinessUniversity;Username=postgres;Password=1234");
        }
    }
}
 