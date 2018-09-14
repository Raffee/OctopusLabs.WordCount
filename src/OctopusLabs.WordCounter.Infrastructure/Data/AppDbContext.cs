using Microsoft.EntityFrameworkCore;
using System.Linq;
using OctopusLabs.WordCounter.Core.Entities;
using OctopusLabs.WordCounter.Core.Interfaces;
using OctopusLabs.WordCounter.Core.SharedKernel;

namespace OctopusLabs.WordCounter.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<WordCount> WordCounts { get; set; }
    }
}