using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    public class BioDbContext : DbContext
    {
        public DbSet<Gene> Genes { get; set; } = null!;
        public DbSet<Drug> Drugs { get; set; } = null!;
        public DbSet<Disease> Diseases { get; set; } = null!;

        //Pay attention to the injecttion
        public BioDbContext(DbContextOptions<BioDbContext> options) : base(options)
        {
            
        }
    }
}
