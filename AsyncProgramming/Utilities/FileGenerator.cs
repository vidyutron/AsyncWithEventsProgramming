using AsyncProgramming.Models;
using System.Text.Json;

namespace AsyncProgramming.Utilities
{
    public class FileGenerator
    {

        public void CreateLargeTestFiles(string[] symbols)
        {
            var random = new Random();

            foreach (var symbol in symbols)
            {
                var stockData = new StockData
                {
                    Symbol = symbol,
                    CompanyName = $"{symbol} Corporation",
                    Sector = "Technology",
                    Currency = "USD",
                    LastUpdated = DateTime.Now,
                    DailyData = new List<DailyPrice>()
                };

                // Generate 365 days of data (larger files)
                for (int i = 0; i < 365; i++)
                {
                    var date = DateTime.Today.AddDays(-365 + i);
                    var basePrice = 100 + random.Next(1, 300);

                    stockData.DailyData.Add(new DailyPrice
                    (
                        date,
                        basePrice + (decimal)(random.NextDouble() * 10 - 5),
                        basePrice + (decimal)(random.NextDouble() * 10 - 5),
                        basePrice + (decimal)(random.NextDouble() * 15),
                        basePrice - (decimal)(random.NextDouble() * 10),
                        random.Next(1000000, 50000000),
                        basePrice
                    ));
                }

                var json = JsonSerializer.Serialize(stockData, new JsonSerializerOptions { WriteIndented = true });
                File.WriteAllText($"Data/{symbol}.json", json);
            }

            Console.WriteLine("Created large test files (~50-100KB each)");
        }
    }
}
