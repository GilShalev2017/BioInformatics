namespace BioBackend.Models
{
    public class DrugGene
    {
       public string DrugID { get; set; }= string.Empty;
       public Drug Drug { get; set; } = null!;
       public string GeneID { get; set; } = string.Empty;
       public Gene Gene { get; set; } = null!;
       public string Effect { get; set; } = string.Empty;
       public string ApprovalYear { get; set; } = string.Empty;
    }
}
