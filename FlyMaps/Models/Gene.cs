namespace FlyMaps.Models
{
    public class DbLink
    {
        public int Id { get; set; }
        public string SourceDbId { get; set; } = string.Empty;
        public string SourceDb { get; set; } = string.Empty;
    }
    public class GeneSummary
    {
        public int Id { get; set; }
        public string Summary { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }
    public class Gene
    {
        public int Id { get; set; }
        public string Symbol { get; set; } = string.Empty;
        public List<string> Aliases { get; set; } = new();
        public List<DbLink> DbLinks { get; set; } = new();
        public List<GeneSummary> Summaries { get; set; } = new List<GeneSummary>();
    }
}
