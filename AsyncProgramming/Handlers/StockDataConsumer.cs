using AsyncProgramming.Models;
using System.Threading.Channels;

namespace AsyncProgramming.Handlers
{
    public class StockDataConsumer
    {
        public async Task ConsumeStockDataAsync(
            ChannelReader<StockData> reader,
            CancellationToken ct = default)
        {
            var processedCount = 0;
            await foreach(var stockData in reader.ReadAllAsync(ct))
            {
                try
                {
                    await ProcessSingleStockAsync(stockData);
                    processedCount++;
                    Console.WriteLine($"Processed: {stockData.Symbol} - Total:{processedCount}");

                    // simulate delay
                    await Task.Delay(100, ct);
                }
                catch (Exception ex) { }
            };          
        }

        public async Task ProcessSingleStockAsync(StockData stockData)
        {
            // Real processing logic: calculate moving averages, detect patterns, etc.
            var avgPrice = stockData.DailyData.Average(d => d.Close);
            var avgVolume = stockData.DailyData.Average(d => d.Volume);

            // Could save to database, send notifications, update caches, etc.
            await Task.Delay(50); // Simulate I/O operation
        }
    }
}
