namespace Backend.Models
{
    //No circular refrences
    public class FlatDiseaseIndexDto
    {
        public string DiseaseID { get; set; } = string.Empty;
        public string DiseaseName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public List<string> RelatedGeneIds { get; set; } = new();
    }

}
