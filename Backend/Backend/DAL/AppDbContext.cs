using System;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol.Plugins;

namespace Backend.DAL
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Bio> Bios { get; set; }
        public DbSet<Upcoming> Upcomings{ get; set; }
        public DbSet<Backend.Models.Type> Types { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<BestSeries> BestSeries { get; set; }
        public DbSet<TrialTest> TrialTests { get; set; }
        public DbSet<Streaming> Streamings { get; set; }
        public DbSet<TopRated> TopRateds { get; set; }
        public DbSet<TopRatedCategory> TopRatedCategories { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Introduction> Introductions { get; set; }
        public DbSet<Pricing> Pricings { get; set; }
        public DbSet<Information> Informations { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<TvSeries> TvSeries { get; set; }
        public DbSet<MovieCategory> MovieCategories { get; set; }
        public DbSet<TvSeriesCategories> TvSeriesCategories { get; set; }
        public DbSet<Blog> Blogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Blog>().Property(t => t.Date).HasDefaultValue(DateTime.Now);
        }

    }
}

