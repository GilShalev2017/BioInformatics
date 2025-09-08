namespace BioBackend.Models
{
    public class Relationships
    {
        public List<Gene> Genes { get; set; } = new();
        public List<Disease> Diseases { get; set; } = new();
        public List<Drug> Drugs { get; set; } = new();
    }
}
