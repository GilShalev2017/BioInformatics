using BioBackend.Data;
using BioBackend.Models;
using Microsoft.EntityFrameworkCore;

namespace BioBackend.Services
{
    public interface ICsvImporter
    {
        Task ImportDataAsync();
    }
    public class CsvImporter : ICsvImporter
    {
        private readonly BioDbContext _bioDbContextcontext;

        public CsvImporter(BioDbContext bioDbContextcontext)// IElasticService elasticService)
        {
            _bioDbContextcontext = bioDbContextcontext;
        }
        private async Task CleanTables()
        {
            await _bioDbContextcontext.Genes.ExecuteDeleteAsync();
            await _bioDbContextcontext.Diseases.ExecuteDeleteAsync();
            await _bioDbContextcontext.Drugs.ExecuteDeleteAsync();
            await _bioDbContextcontext.DrugGenes.ExecuteDeleteAsync();
            await _bioDbContextcontext.DiseaseGenes.ExecuteDeleteAsync();

            await _bioDbContextcontext.SaveChangesAsync();
        }
        public async Task ImportDataAsync()
        {
            try
            {
                await CleanTables();

                var genesFilePath = @"C:\\Development\\Demo\\BioBackend\\DataFiles\\Genes.csv";
                var diseasesFilePath = @"C:\\Development\\Demo\\BioBackend\\DataFiles\\Diseases.csv";
                var drugsFilePath = @"C:\\Development\\Demo\\BioBackend\\DataFiles\\Drugs.csv";
                var drugGenesFilePath = @"C:\\Development\\Demo\\BioBackend\\DataFiles\\DrugGenes.csv";
                var diseaseGeneaFilePath = @"C:\\Development\\Demo\\BioBackend\\DataFiles\\DiseaseGene.csv";

                await ImportGenesAsync(genesFilePath);
                await ImportDiseasesAsync(diseasesFilePath);
                await ImportDrugsAsync(drugsFilePath);
                await ImportDrugGenesAsync(drugGenesFilePath);
                await ImportDiseaseGenesAsync(diseaseGeneaFilePath);
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
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // track duplicates in current file

            foreach (var line in lines)
            {
                try
                {
                    var cols = line.Split(',');

                    //if (cols.Length < 2)
                    if (cols.Length != 2)
                    {
                        Console.WriteLine($"Skipping line (not enough columns): {line}");
                        continue;
                    }

                    var rawId = cols[0].Trim();
                    var rawName = cols[1].Trim();

                    // Basic validation - Missing specific fields
                    if (string.IsNullOrWhiteSpace(rawId))
                    {
                        Console.WriteLine($"Skipping line (missing GeneID): {line}");
                        continue;
                    }

                    if (string.IsNullOrWhiteSpace(rawName))
                    {
                        Console.WriteLine($"Skipping line (missing GeneName): {line}");
                        continue;
                    }

                    // Normalize GeneID (e.g., gene10 → G10)
                    var normalizedId = rawId.ToUpperInvariant();
                    if (normalizedId.StartsWith("GENE"))
                    {
                        normalizedId = "G" + normalizedId.Substring(4);
                    }

                    // Detect duplicate inside same import file
                    if (!seenIds.Add(normalizedId))
                    {
                        Console.WriteLine($"Skipping line (duplicate in file): {line}");
                        continue;
                    }

                    // Check if it already exists in DB
                    bool existsInDb = await _bioDbContextcontext.Genes
                        .AnyAsync(g => g.GeneID == normalizedId);

                    if (existsInDb)
                    {
                        Console.WriteLine($"Skipping line (already exists in DB): {line}");
                        continue;
                    }

                    var gene = new Gene
                    {
                        GeneID = normalizedId,
                        GeneName = rawName
                    };

                    _bioDbContextcontext.Genes.Add(gene);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    Console.WriteLine($"Bad input line: {line}");
                }
            }

            await _bioDbContextcontext.SaveChangesAsync();
        }
        private async Task ImportDiseasesAsync(string filePath)
        {
            var diseaseLines = File.ReadAllLines(filePath).Skip(1);
           
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // track duplicates in current file

            foreach (var line in diseaseLines)
            {
                try
                {
                    var cols = line.Split(',');

                    if (cols.Length < 4)
                    {
                        Console.WriteLine($"Skipping line (bad column count): {line}");
                        continue;
                    }

                    var rawDiseaseID = cols[0].Trim();
                    var rawDiseaseName = cols[1].Trim();
                    var rawDescription = cols[2].Trim();
                    //var rawRelatedGenes = cols[3].Trim();

                    if (string.IsNullOrWhiteSpace(rawDiseaseID) ||
                        string.IsNullOrWhiteSpace(rawDiseaseName) ||
                        string.IsNullOrWhiteSpace(rawDescription))
                    {
                        Console.WriteLine($"Skipping line (missing required field): {line}");
                        continue;
                    }

                    // Detect duplicate inside same import file
                    if (!seenIds.Add(rawDiseaseID))
                    {
                        Console.WriteLine($"Skipping line (duplicate in file): {line}");
                        continue;
                    }

                    // Only add Disease (ignore RelatedGenes here!)
                    if (!await _bioDbContextcontext.Diseases.AnyAsync(d => d.DiseaseID == rawDiseaseID))
                    {
                        var disease = new Disease
                        {
                            DiseaseID = rawDiseaseID,
                            DiseaseName = rawDiseaseName,
                            Description = rawDescription
                        };

                        _bioDbContextcontext.Diseases.Add(disease);
                    }
                    else
                    {
                        Console.WriteLine($"Skipping duplicate disease: {rawDiseaseID}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Skipping line (error: {ex.Message}): {line}");
                }
            }

            await _bioDbContextcontext.SaveChangesAsync();
        }
        private async Task ImportDrugsAsync(string filePath)
        {
            var drugLines = File.ReadAllLines(filePath).Skip(1); // skip header

            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase); // track duplicates in current file

            foreach (var line in drugLines)
            {
                try
                {
                    var cols = line.Split(',');

                    if (cols.Length < 3)
                    {
                        Console.WriteLine($"Skipping line (bad column count): {line}");
                        continue;
                    }

                    var rawDrugId = cols[0].Trim();
                    var rawDrugName = cols[1].Trim();

                    // Detect duplicate inside same import file
                    if (!seenIds.Add(rawDrugId))
                    {
                        Console.WriteLine($"Skipping line (duplicate in file): {line}");
                        continue;
                    }

                    // Only add Drug (ignore DrugGenes here!)
                    if (!await _bioDbContextcontext.Drugs.AnyAsync(d => d.DrugID == rawDrugId))
                    {
                        var drug = new Drug
                        {
                            DrugID = rawDrugId,
                            DrugName = rawDrugName
                        };

                        _bioDbContextcontext.Drugs.Add(drug);
                    }
                    else
                    {
                        Console.WriteLine($"Skipping duplicate disease: {rawDrugId}");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Skipping line (error: {ex.Message}): {line}");
                }
            }
            await _bioDbContextcontext.SaveChangesAsync();
        }
        private async Task ImportDrugGenesAsync(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Skip(1); // skip header
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var line in lines)
            {
                try
                {
                    var cols = line.Split(',');

                    if (cols.Length < 4)
                    {
                        Console.WriteLine($"Skipping line (not enough columns): {line}");
                        continue;
                    }

                    var rawDrugId = cols[0].Trim();
                    var rawGeneId = cols[1].Trim();
                    var rawEffect = cols[2].Trim();
                    var rawApprovalYear = cols[3].Trim();

                    // Basic validation
                    if (string.IsNullOrWhiteSpace(rawDrugId) ||
                        string.IsNullOrWhiteSpace(rawGeneId) ||
                        string.IsNullOrWhiteSpace(rawEffect) ||
                        string.IsNullOrWhiteSpace(rawApprovalYear))
                    {
                        Console.WriteLine($"Skipping line (missing required field): {line}");
                        continue;
                    }

                    // Detect duplicate inside same file
                    if (!seenIds.Add($"{rawDrugId}:{rawGeneId}"))
                    {
                        Console.WriteLine($"Skipping line (duplicate in file): {line}");
                        continue;
                    }

                    // Check if it already exists in DB (PK composed of DrugID + GeneID)
                    bool existsInDb = await _bioDbContextcontext.DrugGenes
                        .AnyAsync(dg => dg.DrugID == rawDrugId && dg.GeneID == rawGeneId);

                    if (existsInDb)
                    {
                        Console.WriteLine($"Skipping line (already exists in DB): {line}");
                        continue;
                    }

                    var foundDrug = await _bioDbContextcontext.Drugs
                        .FirstOrDefaultAsync(d => d.DrugID == rawDrugId);

                    var foundGene = await _bioDbContextcontext.Genes
                        .FirstOrDefaultAsync(g => g.GeneID == rawGeneId);

                    if (foundDrug == null || foundGene == null)
                    {
                        Console.WriteLine($"Skipping line (missing drug or gene): {line}");
                        continue;
                    }

                    var drugGene = new DrugGene
                    {
                        DrugID = rawDrugId,
                        Drug = foundDrug,
                        GeneID = rawGeneId,
                        Gene = foundGene,
                        Effect = rawEffect,
                        ApprovalYear = rawApprovalYear
                    };

                    _bioDbContextcontext.DrugGenes.Add(drugGene);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    Console.WriteLine($"Bad input line: {line}");
                }
            }

            await _bioDbContextcontext.SaveChangesAsync();
        }
        private async Task ImportDiseaseGenesAsync(string filePath)
        {
            var lines = File.ReadAllLines(filePath).Skip(1); // skip header
          
            var seenIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var line in lines)
            {
                try
                {
                    var cols = line.Split(',');

                    if (cols.Length < 4)
                    {
                        Console.WriteLine($"Skipping line (not enough columns): {line}");
                        continue;
                    }

                    var rawDiseaseId = cols[0].Trim();
                    var rawGeneId = cols[1].Trim();
                    var rawEvidenceType = cols[2].Trim();
                    var rawStrength = cols[3].Trim();

                    // Basic validation
                    if (string.IsNullOrWhiteSpace(rawDiseaseId) ||
                        string.IsNullOrWhiteSpace(rawGeneId) ||
                        string.IsNullOrWhiteSpace(rawEvidenceType) ||
                        string.IsNullOrWhiteSpace(rawStrength))
                    {
                        Console.WriteLine($"Skipping line (missing required field): {line}");
                        continue;
                    }

                    // Detect duplicate inside same file
                    if (!seenIds.Add($"{rawDiseaseId}:{rawGeneId}"))
                    {
                        Console.WriteLine($"Skipping line (duplicate in file): {line}");
                        continue;
                    }

                    // Check if it already exists in DB (PK composed of DiseaseID + GeneID)
                    bool existsInDb = await _bioDbContextcontext.DiseaseGenes
                        .AnyAsync(dg => dg.DiseaseID == rawDiseaseId && dg.GeneID == rawGeneId);

                    if (existsInDb)
                    {
                        Console.WriteLine($"Skipping line (already exists in DB): {line}");
                        continue;
                    }

                    var foundDisease = await _bioDbContextcontext.Diseases
                        .FirstOrDefaultAsync(d => d.DiseaseID == rawDiseaseId);

                    var foundGene = await _bioDbContextcontext.Genes
                        .FirstOrDefaultAsync(g => g.GeneID == rawGeneId);

                    if (foundDisease == null || foundGene == null)
                    {
                        Console.WriteLine($"Skipping line (missing disease or gene): {line}");
                        continue;
                    }

                    var diseaseGene = new DiseaseGene
                    {
                        DiseaseID = rawDiseaseId,
                        Disease = foundDisease,
                        GeneID = rawGeneId,
                        Gene = foundGene,
                        EvidenceType = rawEvidenceType,
                        Strength = Convert.ToDouble(rawStrength)
                    };

                    _bioDbContextcontext.DiseaseGenes.Add(diseaseGene);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    Console.WriteLine($"Bad input line: {line}");
                }
            }

            await _bioDbContextcontext.SaveChangesAsync();
        }
    }
}
