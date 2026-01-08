using FlyMaps.Configuration;
using FlyMaps.Controllers;
using FlyMaps.Data;
using FlyMaps.Models;
using Microsoft.Extensions.Options;

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

        public async Task ImportDataAsync()
        {
            try
            {
                var lines = File.ReadAllLines(_geneAliasesPath!).Skip(10);

                foreach (var line in lines)
                {
                    var splits = line.Split(',');

                    if(splits.Length != 4)
                    {
                        continue;
                    }

                    var symbol = splits[0];
                    var alias = splits[1];
                    var sourceDbId = splits[2];
                    var sourceDb = splits[3];
                    var newDbLink = new DbLink() { SourceDb = sourceDb, SourceDbId = sourceDbId };
                    var newGene = new Gene { Symbol = symbol, Aliases = new List<string> { alias }, DbLinks = new List<DbLink> { newDbLink } };

                    var foundGene = _bioDataDbContext.Genes.FirstOrDefault(gene => gene.Symbol == symbol);
                    if (foundGene == null)
                    {
                        _bioDataDbContext.Genes.Add(newGene);
                    }
                    else //Gene already exist
                    {
                        foundGene.Aliases.Add(alias);
                        var foundDbLink = foundGene.DbLinks.FirstOrDefault(dbLink => dbLink.SourceDb == sourceDb);
                        if(foundDbLink == null)
                        {
                            foundGene.DbLinks.Add(newDbLink);
                        }
                    }
                }

                await _bioDataDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Import Data failed: {ex.Message}");
                throw; // rethrow so you still see it in ASP.NET logs
            }
        }
    }
}
