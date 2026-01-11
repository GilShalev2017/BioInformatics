using FlyMaps.Configuration;
using FlyMaps.Data;
using FlyMaps.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace FlyMaps.Services
{
    public interface IBioDataService
    {
        Task<Gene?> GetGeneDetailsAsync(string symbol);
        Task<List<Gene>> SearchGenesAsync(string query);
    }

    public class BioDataService : IBioDataService
    {
        private readonly ILogger<BioDataService> _logger;
        private readonly BioDataDbContext _bioDataDbContext;

        public BioDataService(BioDataDbContext bioDbContextcontext, IOptions<AppSettings> options, ILogger<BioDataService> logger)
        {
            _bioDataDbContext = bioDbContextcontext;
            _logger = logger;
        }
        public async Task<Gene?> GetGeneDetailsAsync(string symbol)
        {
            var gene = await _bioDataDbContext.Genes
                            .AsNoTracking()
                            .Include(g => g.Summaries)
                            .Include(g => g.Aliases)
                            .Include(g => g.DbLinks)
                            .FirstOrDefaultAsync(g => g.Symbol == symbol);

            return gene;
        }

        public async Task<List<Gene>> SearchGenesAsync(string query)
        {
            if (string.IsNullOrWhiteSpace(query))
                return new List<Gene>();

            query = query.Trim();

            var genes = await _bioDataDbContext.Genes
                      .AsNoTracking()
                      .Include(g => g.Aliases)
                      .Where(g =>
                          g.Symbol.Contains(query) ||
                          g.Aliases.Any(a => a.Alias.Contains(query))
                      )
                      .OrderBy(g => g.Symbol)
                      .Take(20)
                      .ToListAsync();

            return genes;
        }
    }
}
