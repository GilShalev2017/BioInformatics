namespace FlyMaps.Models
{
    public class GeneSummaryDto
    {
        public string Summary { get; set; } = string.Empty;
        public string Source { get; set; } = string.Empty;
    }

    public class GeneSummariesModel
    {
        public string Symbol { get; set; } = string.Empty;
        public List<GeneSummaryDto> Summaries { get; set; } = new();
    }
}
