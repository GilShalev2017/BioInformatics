namespace Backend.Models
{
    public class Gene
    {
        public int Id { get; set; }
        public string GeneID { get; set; } = string.Empty;
        public string GeneName { get; set; } = string.Empty;

        // Bi-directional navigation property
        public List<Disease> RelatedDiseases { get; set; } = new();
        public List<Drug> TargetedByDrugs { get; set; } = new();
    }
}
