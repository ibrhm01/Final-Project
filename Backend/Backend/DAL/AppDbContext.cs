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
        public DbSet<UpcomingMovie> UpcomingMovies{ get; set; }
        public DbSet<Backend.Models.Type> Types { get; set; }
        public DbSet<Banner> Banners { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Service> Services { get; set; }
        public DbSet<BestSeries> BestSeries { get; set; }
        public DbSet<TrialTest> TrialTests { get; set; }

    }
}

