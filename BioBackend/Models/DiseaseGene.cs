namespace BioBackend.Models
{
    public class DiseaseGene
    {
        //public int Id { get; set; }
        public string DiseaseID { get; set; } = string.Empty;
        public Disease Disease { get; set; } = null!;
        public string GeneID { get; set; } = string.Empty;
        public Gene Gene { get; set; } = null!;
        public string EvidenceType { get; set; } = string.Empty;
        public double Strength { get; set; } = 1.0;
    }
}
