namespace Backend.Models
{
    public class Disease
    {
        public int Id { get; set; }
        public string DiseaseID { get; set; } = string.Empty;
        public string DiseaseName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        // Navigation property to genes
        public List<Gene> RelatedGenes { get; set; } = new();
    }
}
