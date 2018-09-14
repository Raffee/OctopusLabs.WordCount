using OctopusLabs.WordCounter.Core.Entities;
using OctopusLabs.WordCounter.Infrastructure.Data;

namespace OctopusLabs.WordCounter.Web
{
    public static class SeedData
    {
        public static void PopulateTestData(AppDbContext dbContext)
        {
            var wordCounts = dbContext.WordCounts;
            foreach (var item in wordCounts)
            {
                dbContext.Remove(item);
            }
            dbContext.SaveChanges();
            dbContext.SaveChanges();
        }

    }
}
