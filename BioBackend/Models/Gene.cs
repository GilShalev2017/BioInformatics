namespace BioBackend.Models
{
    public class Gene
    {
        public string GeneID { get; set; } = string.Empty;
        public string GeneName { get; set; } = string.Empty;
      
        public List<DiseaseGene> DiseaseGenes { get; set; } = new();
        public List<DrugGene> DrugGenes { get; set; } = new();
    }
}
