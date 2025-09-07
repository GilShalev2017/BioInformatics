using Backend.Models;
using Nest;

namespace Backend.Services
{
    public interface IElasticService
    {
        Task IndexDiseasesAsync(IEnumerable<Disease> diseases);
        Task<IEnumerable<FlatDiseaseIndexDto>?> ElasticSearchDiseasesAsync(string query);
    }

    public class ElasticService : IElasticService
    {
        private readonly ElasticClient _elasticClient;

        public ElasticService()
        {
            var settings = new ConnectionSettings(new Uri("http://localhost:9200"))
                .DisableDirectStreaming() // <-- captures request/response in errors
                .DefaultIndex("diseases");

            _elasticClient = new ElasticClient(settings);
        }

        public async Task IndexDiseasesAsync(IEnumerable<Disease> diseases)
        {
            try
            {
                // Map EF entities → flat DTOs
                var docs = diseases.Select(d => new FlatDiseaseIndexDto
                {
                    DiseaseID = d.DiseaseID,
                    DiseaseName = d.DiseaseName,
                    Description = d.Description,
                    RelatedGeneIds = d.RelatedGenes?.Select(g => g.GeneID).ToList() ?? new List<string>()
                }).ToList();

                // Ensure index exists with mapping
                var exists = await _elasticClient.Indices.ExistsAsync("diseases");
                if (!exists.Exists)
                {
                    var createIndexResponse = await _elasticClient.Indices.CreateAsync("diseases", c => c
                        .Map<FlatDiseaseIndexDto>(m => m.AutoMap())
                    );

                    if (!createIndexResponse.IsValid)
                        throw new Exception($"Failed to create index: {createIndexResponse.ServerError}");
                }

                // Index docs
                var bulkResponse = await _elasticClient.BulkAsync(b => b
                    .Index("diseases")
                    .IndexMany(docs)
                );

                if (bulkResponse.Errors)
                {
                    var errors = string.Join(Environment.NewLine,
                        bulkResponse.ItemsWithErrors.Select(e => $"{e.Id}: {e.Error.Reason}"));
                    throw new Exception($"Failed to index some records: {errors}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Indexing failed: {ex.Message}");
                throw;
            }
        }

        public async Task<IEnumerable<FlatDiseaseIndexDto>?> ElasticSearchDiseasesAsync(string query)
        {
            var response = await _elasticClient.SearchAsync<FlatDiseaseIndexDto>(s => s
                .Query(q => q
                    .MultiMatch(m => m
                        .Fields(f => f
                            .Field(ff => ff.DiseaseID)
                            .Field(ff => ff.DiseaseName)
                            .Field(ff => ff.Description)
                        )
                        .Query(query)
                    )
                )
                .Size(50)
            );

            if (!response.IsValid)
                throw new Exception(response.OriginalException?.Message);

            return response.Documents;
        }

    }
}
