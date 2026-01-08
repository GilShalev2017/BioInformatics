using FlyMaps.Configuration;
using FlyMaps.Controllers;
using FlyMaps.Data;
using FlyMaps.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Text.Json;
using System.Text.Json;
using System.Xml.Linq;

namespace FlyMaps.Services
{
    public interface IBioDataImporter
    {
        Task ImportDataAsync();
    }
    public class BioDataImporter : IBioDataImporter
    {
        private readonly ILogger<BioDataImporter> _logger;
        private readonly BioDataDbContext _bioDataDbContext;
        private readonly string _dataFilesFolderPath;
        private readonly string _geneAliasesPath;
        private readonly string _geneSummariesPath;
        public BioDataImporter(BioDataDbContext bioDbContextcontext, IOptions<AppSettings> options, ILogger<BioDataImporter> logger)
        {
            _bioDataDbContext = bioDbContextcontext;
            _dataFilesFolderPath = options.Value.DataFilesFolderPath;
            _geneAliasesPath = $"{_dataFilesFolderPath}/gene_aliases.csv";
            _geneSummariesPath = $"{_dataFilesFolderPath}/gene_descriptions.json";
            _logger = logger;
        }

        private async Task<Dictionary<string, GeneSummariesModel>> LoadSummariesAsync()
        {
            if (!File.Exists(_geneSummariesPath!))
                return new();

            var jsonString = await File.ReadAllTextAsync(_geneSummariesPath);

            if (string.IsNullOrWhiteSpace(jsonString))
                return new();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            // Deserialize directly to dictionary
            var result = JsonSerializer.Deserialize<Dictionary<string, GeneSummariesModel>>(jsonString, options) ?? new();

            return result;
        }

        public async Task ImportDataAsync()
        {
            try
            {
                Dictionary<string, GeneSummariesModel> summariesDic = await LoadSummariesAsync();

                var genes = await _bioDataDbContext.Genes
                                    .Include(g => g.Aliases)
                                    .Include(g => g.DbLinks)
                                    .Include(g => g.Summaries)
                                    .ToListAsync();

                var geneDict = genes.ToDictionary(g => g.Symbol);

                var lines = File.ReadLines(_geneAliasesPath).Skip(10);

                foreach (var line in lines)
                {
                    var parts = line.Split(',');
                    if (parts.Length != 4)
                        continue;

                    var symbol = parts[0].Trim();
                    var alias = parts[1].Trim();
                    var sourceDbId = parts[2].Trim();
                    var sourceDb = parts[3].Trim();

                    if (!geneDict.TryGetValue(symbol, out var gene))
                    {
                        gene = new Gene { Symbol = symbol };
                       
                        geneDict[symbol] = gene;
                        
                        _bioDataDbContext.Genes.Add(gene);

                        // Add summaries once per gene
                        if (summariesDic.TryGetValue(symbol, out var summariesModel))
                        {
                            foreach (var summary in summariesModel.Summaries)
                            {
                                gene.Summaries.Add(new GeneSummary
                                {
                                    Summary = summary.Summary,
                                    Source = summary.Source
                                });
                            }
                        }
                    }

                    // Alias (unique per gene + alias + source)
                    if (!gene.Aliases.Any(a => a.Alias == alias && a.Source == sourceDb))
                    {
                        gene.Aliases.Add(new GeneAlias
                        {
                            Alias = alias,
                            Source = sourceDb
                        });
                    }

                    // DbLink (unique per gene + source)
                    if (!gene.DbLinks.Any(d => d.SourceDb == sourceDb))
                    {
                        gene.DbLinks.Add(new DbLink
                        {
                            SourceDb = sourceDb,
                            SourceDbId = sourceDbId
                        });
                    }
                }

                await _bioDataDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Import failed");
                throw;
            }
        }
    }
}
