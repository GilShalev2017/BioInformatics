using BioBackend.Data;
using BioBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BioBackend.Services
{
    public interface IBioinformaticsService
    {
        // Disease methods
        Task<Disease?> FindDiseaseAndRelatedGenesAsync(string diseaseId);
        Task<IEnumerable<Disease>> GetAllDiseasesAsync();
        Task<IEnumerable<Disease>> SearchDiseasesAsync(string query);
        Task<(IEnumerable<Disease> Items, int TotalCount)> GetDiseasesPagedAsync(int pageNumber, int pageSize, string? searchQuery);

        // Gene methods
        Task<Gene?> FindGeneAndRelatedEntitiesAsync(string geneId);
        Task<IEnumerable<Gene>> GetAllGenesAsync();
        Task<IEnumerable<Gene>> SearchGenesAsync(string query);

        // Drug methods
        Task<Drug?> FindDrugAndRelatedEntitiesAsync(string drugId);
        Task<IEnumerable<Drug>> GetAllDrugsAsync();
        Task<IEnumerable<Drug>> SearchDrugsAsync(string query);

        Task<Relationships> GetRelationships();


        // Association methods
        Task<IEnumerable<Gene>> GetGenesForDiseaseAsync(string diseaseId);
        Task<IEnumerable<Disease>> GetDiseasesForGeneAsync(string geneId);
        Task<IEnumerable<Drug>> GetDrugsForGeneAsync(string geneId);
        Task<IEnumerable<Gene>> GetGenesForDrugAsync(string drugId);
    }

    public class BioinformaticsService : IBioinformaticsService
    {
        private readonly BioDbContext _bioDbContext;
        private readonly ILogger<BioinformaticsService> _logger;
        public BioinformaticsService(BioDbContext bioDbContext, ILogger<BioinformaticsService> logger)
        {
            _bioDbContext = bioDbContext;
            _logger = logger;
        }

        #region Disease Methods

        public async Task<Disease?> FindDiseaseAndRelatedGenesAsync(string diseaseId)
        {
            var disease = await _bioDbContext.Diseases
                .Include(d => d.DiseaseGenes)
                .FirstOrDefaultAsync(d => d.DiseaseID == diseaseId);

            return disease;
        }

        public async Task<IEnumerable<Disease>> GetAllDiseasesAsync()
        {
            var diseases = await _bioDbContext.Diseases
                .Include(d => d.DiseaseGenes)
                .ToListAsync();

            return diseases;
        }

        public async Task<IEnumerable<Disease>> SearchDiseasesAsync(string query)
        {
            var diseases = await _bioDbContext.Diseases
                .Include(d => d.DiseaseGenes)
                .Where(d => d.DiseaseName.Contains(query) ||
                           d.Description.Contains(query) ||
                           d.DiseaseID.Contains(query))
                .ToListAsync();

            return diseases;
        }
        public async Task<(IEnumerable<Disease> Items, int TotalCount)> GetDiseasesPagedAsync(int pageNumber, int pageSize, string? searchQuery)
        {
            var query = _bioDbContext.Diseases
                .Include(d => d.DiseaseGenes) // include the join data
                .AsQueryable();

            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(d =>
                    d.DiseaseName.Contains(searchQuery) ||
                    d.Description.Contains(searchQuery) ||
                    d.DiseaseGenes.Any(g => g.Gene.GeneName.Contains(searchQuery))
                );
            }

            var totalCount = await query.CountAsync();

            var items = await query
                .OrderBy(d => d.DiseaseName) // deterministic ordering
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }


        #endregion

        #region Gene Methods

        public async Task<Gene?> FindGeneAndRelatedEntitiesAsync(string geneId)
        {
            var gene = await _bioDbContext.Genes
                .Include(g => g.DiseaseGenes)
                .Include(g => g.DrugGenes)
                .FirstOrDefaultAsync(g => g.GeneID == geneId);

            return gene;
        }

        public async Task<IEnumerable<Gene>> GetAllGenesAsync()
        {
            var genes = await _bioDbContext.Genes
                .Include(g => g.DiseaseGenes)
                .Include(g => g.DrugGenes)
                .ToListAsync();

            return genes;
        }

        public async Task<IEnumerable<Gene>> SearchGenesAsync(string query)
        {
            var genes = await _bioDbContext.Genes
                .Include(g => g.DiseaseGenes)
                .Include(g => g.DrugGenes)
                .Where(g => g.GeneName.Contains(query) ||
                           g.GeneID.Contains(query))
                .ToListAsync();

            return genes;
        }

        #endregion

        #region Drug Methods

        public async Task<Drug?> FindDrugAndRelatedEntitiesAsync(string drugId)
        {
            var drug = await _bioDbContext.Drugs
                .Include(d => d.DrugGenes)
                .FirstOrDefaultAsync(d => d.DrugID == drugId);

            return drug;
        }

        public async Task<IEnumerable<Drug>> GetAllDrugsAsync()
        {
            var drugs = await _bioDbContext.Drugs
                .Include(d => d.DrugGenes)
                .ToListAsync();

            return drugs;
        }

        public async Task<IEnumerable<Drug>> SearchDrugsAsync(string query)
        {
            var drugs = await _bioDbContext.Drugs
                // .Include(d => d.RelatedGenes)
                .Include(d => d.DrugGenes)
                .Where(d => d.DrugName.Contains(query) ||
                           d.DrugID.Contains(query))
                .ToListAsync();

            return drugs;
        }

        #endregion

        #region Relationships
        public async Task<Relationships> GetRelationships()
        {
            var genes = await _bioDbContext.Genes
              .Include(g => g.DiseaseGenes)
              .Include(g => g.DrugGenes)
              .ToListAsync();

            var diseases = await _bioDbContext.Diseases
                .Include(d => d.DiseaseGenes)
                .ToListAsync();

            var drugs = await _bioDbContext.Drugs
                .Include(dr => dr.DrugGenes)
                .ToListAsync();

            var relationships = new Relationships { Genes = genes, Diseases = diseases, Drugs = drugs };

            return relationships;
        }

        #endregion

        // Association methods
        public async Task<IEnumerable<Gene>> GetGenesForDiseaseAsync(string diseaseId)
        {
            var disease = await _bioDbContext.Diseases
                .Include(d => d.DiseaseGenes)
                .ThenInclude(dg => dg.Gene)
                .FirstOrDefaultAsync(d => d.DiseaseID == diseaseId);

            return disease?.DiseaseGenes.Select(dg => dg.Gene) ?? Enumerable.Empty<Gene>();
        }

        public async Task<IEnumerable<Disease>> GetDiseasesForGeneAsync(string geneId)
        {
            var gene = await _bioDbContext.Genes
                .Include(g => g.DiseaseGenes)
                .ThenInclude(dg => dg.Disease)
                .FirstOrDefaultAsync(g => g.GeneID == geneId);

            return gene?.DiseaseGenes.Select(dg => dg.Disease) ?? Enumerable.Empty<Disease>();
        }

        public async Task<IEnumerable<Drug>> GetDrugsForGeneAsync(string geneId)
        {
            var gene = await _bioDbContext.Genes
                .Include(g => g.DrugGenes)
                .ThenInclude(dg => dg.Drug)
                .FirstOrDefaultAsync(g => g.GeneID == geneId);

            return gene?.DrugGenes.Select(dg => dg.Drug) ?? Enumerable.Empty<Drug>();
        }

        public async Task<IEnumerable<Gene>> GetGenesForDrugAsync(string drugId)
        {
            var drug = await _bioDbContext.Drugs
                .Include(d => d.DrugGenes)
                .ThenInclude(dg => dg.Gene)
                .FirstOrDefaultAsync(d => d.DrugID == drugId);

            return drug?.DrugGenes.Select(dg => dg.Gene) ?? Enumerable.Empty<Gene>();
        }
    }
}
