using BioBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BioBackend.Data
{
    public class BioDbContext : DbContext
    {
        public DbSet<Gene> Genes { get; set; } = null!;
        public DbSet<Drug> Drugs { get; set; } = null!;
        public DbSet<Disease> Diseases { get; set; } = null!;
        public DbSet<DiseaseGene> DiseaseGenes { get; set; } = null!;
        public DbSet<DrugGene> DrugGenes { get; set; } = null!;

        //Pay attention to the injecttion
        public BioDbContext(DbContextOptions<BioDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DiseaseGene>()
                .HasKey(dg => new { dg.DiseaseID, dg.GeneID }); // composite PK

            modelBuilder.Entity<DiseaseGene>()
                .HasOne(dg => dg.Disease)
                .WithMany(d => d.DiseaseGenes)
                .HasForeignKey(dg => dg.DiseaseID);

            modelBuilder.Entity<DiseaseGene>()
                .HasOne(dg => dg.Gene)
                .WithMany(g => g.DiseaseGenes)
                .HasForeignKey(dg => dg.GeneID);

            // DrugGene join table
            modelBuilder.Entity<DrugGene>()
                .HasKey(dg => new { dg.DrugID, dg.GeneID }); // composite PK

            modelBuilder.Entity<DrugGene>()
                .HasOne(dg => dg.Drug)
                .WithMany(d => d.DrugGenes)
                .HasForeignKey(dg => dg.DrugID);

            modelBuilder.Entity<DrugGene>()
                .HasOne(dg => dg.Gene)
                .WithMany(g => g.DrugGenes)
                .HasForeignKey(dg => dg.GeneID);
        }
    }
}
