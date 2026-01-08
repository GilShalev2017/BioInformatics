using FlyMaps.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyMaps.Data
{
    public class BioDataDbContext : DbContext
    {
        public DbSet<Gene> Genes { get; set; } = null!;
        public BioDataDbContext(DbContextOptions<BioDataDbContext> options) : base(options)
        {
            
        }
    }
}
