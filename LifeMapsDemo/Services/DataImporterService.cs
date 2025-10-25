using Microsoft.AspNetCore.Mvc;

namespace LifeMapsDemo.Services
{
    public interface IImportDataService
    {
        void ImportDataAsync();
    }
    public class ImportDataService : IImportDataService
    {
        public ImportDataService()
        {
            
        }

        public void ImportDataAsync()
        {
            try
            {
                var csvFilePath = "C:\\Development\\Demo\\LifeMapsDemo\\DataFiles\\gene_aliases.csv";
                var jsonFilePath = "C:\\Development\\Demo\\LifeMapsDemo\\DataFiles\\gene_descriptions.json";

                ImportCSVAsync(csvFilePath);
                ImportJsonAsync(jsonFilePath);

            }
            catch (Exception ex)
            {

            }
        }

        private void ImportCSVAsync(string filePath)
        {

        }

        private void ImportJsonAsync(string filePath)
        {

        }
    }
}
