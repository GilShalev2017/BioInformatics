using Backend.Data;
using Backend.Models;
using Microsoft.EntityFrameworkCore;


namespace Backend.Services
{
    public interface ICsvImporter
    {
        Task ImportDataAsync();
    }

    public class CsvImporter : ICsvImporter
    {
        private readonly BioDbContext _bioDbContextcontext;
        private readonly IElasticService _elasticService;

        public CsvImporter(BioDbContext bioDbContextcontext, IElasticService elasticService)
        {
            _bioDbContextcontext = bioDbContextcontext;
            _elasticService = elasticService;
        }
        private async Task CleanTables()
        {
            await _bioDbContextcontext.Genes.ExecuteDeleteAsync();
            await _bioDbContextcontext.Diseases.ExecuteDeleteAsync();
            await _bioDbContextcontext.Drugs.ExecuteDeleteAsync();

            await _bioDbContextcontext.SaveChangesAsync();
        }

        public async Task ImportDataAsync()
        {
            try
            {
                await CleanTables();

                var genesFilePath = @"C:\\Development\\Demo\\Backend\\DataFiles\\Genes.csv";
                var diseasesFilePath = @"C:\\Development\\Demo\\Backend\\DataFiles\\Diseases.csv";
                var drugsFilePath = @"C:\\Development\\Demo\\Backend\\DataFiles\\Drugs.csv";

                await ImportGenesAsync(genesFilePath);
                await ImportDiseasesAsync(diseasesFilePath);
                await ImportDrugsAsync(drugsFilePath);

                //await _elasticService.IndexDiseasesAsync(_bioDbContextcontext.Diseases);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Import failed: {ex.Message}");
                throw; // rethrow so you still see it in ASP.NET logs
            }
        }


        private async Task ImportGenesAsync(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Skip(1); // skip header

            foreach (var line in lines)
            {
                var cols = line.Split(',');
                var gene = new Gene
                {
                    GeneID = cols[0],
                    GeneName = cols[1]
                };

                _bioDbContextcontext.Genes.Add(gene);
            }

           await _bioDbContextcontext.SaveChangesAsync();
        }
        //private async Task ImportDiseasesAsync(string filePath)
        //{
        //    var diseaseLines = File.ReadAllLines(filePath).Skip(1);

        //    foreach (var line in diseaseLines)
        //    {
        //        var cols = line.Split(',');

        //        var disease = new Disease
        //        {
        //            DiseaseID = cols[0],
        //            DiseaseName = cols[1],
        //            Description = cols[2]
        //        };

        //        // Parse gene IDs: [G1,G2]
        //        var geneIds = cols[3].Trim('[', ']').Split(',', StringSplitOptions.RemoveEmptyEntries);

        //        foreach (var gId in geneIds)
        //        {
        //            var gene = await _bioDbContextcontext.Genes.FirstOrDefaultAsync(x => x.GeneID == gId.Trim());

        //            if (gene != null)
        //            {
        //                disease.RelatedGenes.Add(gene); // EF will populate hidden join table automatically
        //            }
        //        }

        //        _bioDbContextcontext.Diseases.Add(disease);
        //    }

        //    await _bioDbContextcontext.SaveChangesAsync();

        //}
        //private async Task ImportDrugsAsync(string filePath)
        //{
        //    var lines = File.ReadAllLines(filePath).Skip(1); // skip header

        //    foreach (var line in lines)
        //    {
        //        var cols = line.Split(',');

        //        var drug = new Drug
        //        {
        //            DrugId = cols[0],
        //            DrugName = cols[1],
        //        };

        //        // Parse gene IDs: [G1,G2]
        //        var geneIds = cols[2].Trim('[', ']').Split(',', StringSplitOptions.RemoveEmptyEntries);

        //        foreach (var gId in geneIds)
        //        {
        //            var gene = await _bioDbContextcontext.Genes.FirstOrDefaultAsync(x => x.GeneID == gId.Trim());

        //            if (gene != null)
        //            {
        //                drug.TargetGenes.Add(gene); // EF will populate hidden join table automatically
        //            }
        //        }

        //        _bioDbContextcontext.Drugs.Add(drug);
        //    }

        //    await _bioDbContextcontext.SaveChangesAsync();

        //}

        private async Task ImportDiseasesAsync(string filePath)
        {
            var diseaseLines = File.ReadAllLines(filePath).Skip(1); // skip header

            foreach (var line in diseaseLines)
            {
                try
                {
                    var cols = line.Split(',');

                    if (cols.Length < 4) continue; // skip invalid rows

                    var disease = new Disease
                    {
                        DiseaseID = cols[0].Trim(),
                        DiseaseName = cols[1].Trim(),
                        Description = cols[2].Trim()
                    };

                    // Parse gene IDs: handle quotes, spaces, brackets
                    var geneIdsRaw = cols[3].Trim();
                    if (!string.IsNullOrEmpty(geneIdsRaw))
                    {
                        geneIdsRaw = geneIdsRaw.Trim('"'); // remove surrounding double quotes
                        var geneIds = geneIdsRaw
                            .Trim('[', ']') // remove brackets
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(g => g.Trim(' ', '\'', '"')) // remove spaces, single/double quotes
                            .ToList();

                        foreach (var gId in geneIds)
                        {
                            var gene = await _bioDbContextcontext.Genes
                                .FirstOrDefaultAsync(x => x.GeneID == gId);

                            if (gene != null)
                            {
                                disease.RelatedGenes.Add(gene); // EF Core populates join table
                            }
                        }
                    }

                    _bioDbContextcontext.Diseases.Add(disease);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Skipping line due to error: {line}. Error: {ex.Message}");
                    continue;
                }
            }

            await _bioDbContextcontext.SaveChangesAsync();
        }

        private async Task ImportDrugsAsync(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Skip(1); // skip header

            foreach (var line in lines)
            {
                try
                {
                    var cols = line.Split(',');

                    if (cols.Length < 3) continue; // skip invalid rows

                    var drug = new Drug
                    {
                        DrugId = cols[0].Trim(),
                        DrugName = cols[1].Trim(),
                    };

                    // Parse gene IDs: handle quotes, spaces, brackets
                    var geneIdsRaw = cols[2].Trim();
                    if (!string.IsNullOrEmpty(geneIdsRaw))
                    {
                        geneIdsRaw = geneIdsRaw.Trim('"'); // remove surrounding double quotes
                        var geneIds = geneIdsRaw
                            .Trim('[', ']')
                            .Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(g => g.Trim(' ', '\'', '"'))
                            .ToList();

                        foreach (var gId in geneIds)
                        {
                            var gene = await _bioDbContextcontext.Genes
                                .FirstOrDefaultAsync(x => x.GeneID == gId);

                            if (gene != null)
                            {
                                drug.TargetGenes.Add(gene); // EF Core populates join table
                            }
                        }
                    }

                    _bioDbContextcontext.Drugs.Add(drug);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Skipping line due to error: {line}. Error: {ex.Message}");
                    continue;
                }
            }

            await _bioDbContextcontext.SaveChangesAsync();
        }

    }
}
