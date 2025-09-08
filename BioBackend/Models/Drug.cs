namespace BioBackend.Models
{
    public class Drug
    {
        //public int Id { get; set; }  // <-- Primary Key
        public string DrugID { get; set; } = string.Empty;
        public string DrugName { get; set; } = string.Empty;
        public List<DrugGene> DrugGenes { get; set; } = new();
    }
}
