using AsyncProgramming.Models;
using System.Threading.Channels;

namespace AsyncProgramming.Handlers
{
    public class StockDataProducer
    {
        private StockDataHandler _stockHandler;

        public StockDataProducer(StockDataHandler handler)
        {
            _stockHandler = handler;
        }
        public async Task ProduceStockDataAsync(
            string[] files,
            ChannelWriter<StockData> writer,
            int maxConcurrency = 5,
            CancellationToken ct = default)           
        {
            using var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
            var produceTasks = files.Select(async file =>
            {
                await semaphore.WaitAsync(ct);
                try
                {
                    var stockData = await _stockHandler.LoadStockAsync(file);

                    // Write to channel with backpressure
                    await writer.WriteAsync(stockData,ct);
                    Console.WriteLine($"Produced data for: {stockData.Symbol}");
                }
                catch (Exception ex) { Console.WriteLine($"Failed to produce for - {file}: {ex.Message}"); }
                finally { semaphore.Release(); }
            });

            await Task.WhenAll( produceTasks );
            writer.Complete(); //Signal: No more data coming in 
        }
    }
}
