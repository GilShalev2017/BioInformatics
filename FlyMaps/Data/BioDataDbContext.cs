using FlyMaps.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyMaps.Data
{
    public class BioDataDbContext : DbContext
    {
        public DbSet<Gene> Genes { get; set; }
        public DbSet<GeneAlias> GeneAliases { get; set; }
        public DbSet<GeneSummary> GeneSummaries { get; set; }
        public DbSet<DbLink> DbLinks { get; set; }

        public BioDataDbContext(DbContextOptions<BioDataDbContext> options) : base(options)
        {

        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Gene>()
                .HasIndex(g => g.Symbol)
                .IsUnique();

            modelBuilder.Entity<GeneAlias>()
                .HasIndex(a => new { a.GeneId, a.Alias, a.Source })
                .IsUnique();

            modelBuilder.Entity<DbLink>()
                .HasIndex(d => new { d.GeneId, d.SourceDb })
                .IsUnique();
        }
    }
}
