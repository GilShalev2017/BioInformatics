using CsvHelper.Configuration;

namespace FlyMaps.Models
{
    public class GeneAliasCsvRecord
    {
        public string Symbol { get; set; } = string.Empty;
        public string Alias { get; set; } = string.Empty;
        public string SourceDbId { get; set; } = string.Empty;
        public string SourceDb { get; set; } = string.Empty;
    }

    public sealed class GeneAliasCsvMap : ClassMap<GeneAliasCsvRecord>
    {
        public GeneAliasCsvMap()
        {
            Map(m => m.Symbol).Index(0);
            Map(m => m.Alias).Index(1);
            Map(m => m.SourceDbId).Index(2);
            Map(m => m.SourceDb).Index(3);
        }
    }
}
