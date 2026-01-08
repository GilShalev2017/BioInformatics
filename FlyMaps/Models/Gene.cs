namespace FlyMaps.Models
{
    public class Gene
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public ICollection<GeneAlias> Aliases { get; set; } = new List<GeneAlias>();
        public ICollection<GeneSummary> Summaries { get; set; } = new List<GeneSummary>();
        public ICollection<DbLink> DbLinks { get; set; } = new List<DbLink>();
    }
    public class GeneAlias
    {
        public int Id { get; set; }
        public string Alias { get; set; } = string.Empty;
        public int GeneId { get; set; }
        public Gene Gene { get; set; } = null!;
        public string Source { get; set; } = string.Empty;
    }
    public class GeneSummary
    {
        public int Id { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
        public int GeneId { get; set; }
        public Gene Gene { get; set; } = null!;
    }
    public class DbLink
    {
        public int Id { get; set; }
        public string SourceDb { get; set; } = string.Empty;
        public string SourceDbId { get; set; } = string.Empty;
        public int GeneId { get; set; }
        public Gene Gene { get; set; } = null!;
    }
  
}
