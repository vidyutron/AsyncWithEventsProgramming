using AsyncProgramming.Models;
using System.Threading.Channels;

namespace AsyncProgramming.Handlers
{
    public class StockDataOrchestrator
    {
        private StockDataProducer _producer;
        private StockDataConsumer _consumer;

        public StockDataOrchestrator()
        {
            var handler = new StockDataHandler();
            _producer = new StockDataProducer(handler);
            _consumer = new StockDataConsumer();
        }
        public async Task ProcessStocksWithProducerConsumerAsync(string[] files, int maxCapacity=5)
        {
            var options = new BoundedChannelOptions(capacity: maxCapacity)
            {
                FullMode = BoundedChannelFullMode.Wait, // BackPressure - block producer when full
                SingleReader = false,//multiple consumers
                SingleWriter = false,//multiple producers
            };

            var channel = Channel.CreateBounded<StockData>(options);

            using var cts = new CancellationTokenSource();
            var producerTask = _producer.ProduceStockDataAsync(files, channel.Writer, maxConcurrency: 3, cts.Token);
            var consumerTask = _consumer.ConsumeStockDataAsync(channel.Reader,cts.Token);

            await Task.WhenAll(producerTask,consumerTask);
            Console.WriteLine("Producer - Consumer pipeline completed");            
        }

        public async Task ProcessStocksInBatchesAsync(
            string[] files,
            int batchSize = 10,
            int maxConcurrency = 3)
        {
            var channel = Channel.CreateBounded<StockData>(batchSize);
            var producer = _producer.ProduceStockDataAsync(files, channel.Writer, maxConcurrency);
            var consumer = ProcessStocksInBatchesAsync(channel.Reader, batchSize);
            await Task.WhenAll(producer,consumer);

            Console.WriteLine("Batch Processing pipeline completed");
        }

        private async Task ProcessStocksInBatchesAsync(ChannelReader<StockData> reader, int batchSize)
        {
            var batch = new List<StockData>();
            var batchNumber = 1;
            
            await foreach(var stock in reader.ReadAllAsync())
            {
                batch.Add(stock);
                if(batch.Count > batchSize)
                {
                    await ProcessBatchAsync(batch, batchNumber++);
                    batch.Clear();
                }
            }

            if (batch.Any())
                await ProcessBatchAsync(batch, batchNumber);
        }

        private async Task ProcessBatchAsync(List<StockData> batch, int batchNumber)
        {
            Console.WriteLine($"Processing Batch - {batchNumber} with {batch.Count} stocks");

            var processingTasks = batch.Select(async stock =>
            {
                await _consumer.ProcessSingleStockAsync(stock);
                Console.WriteLine($"Completed processing of {stock.Symbol}");
            });

            await Task.WhenAll(processingTasks);
            Console.WriteLine($"Completed Batch Number - {batchNumber}");
        }
    }
}
