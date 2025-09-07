using AsyncProgramming.Models;
using System.Threading.Channels;

namespace AsyncProgramming.Handlers
{
    public class StockAnalysisPipeline
    {
        private StockDataProducer _producer;
        private StockDataConsumer _consumer;
        private StockDataOrchestrator _orchestrator;
        private StockDataHandler _stockHandler;

        public StockAnalysisPipeline() {
            _stockHandler = new StockDataHandler();
            _producer = new StockDataProducer(_stockHandler);
            _consumer = new StockDataConsumer();
            _orchestrator = new StockDataOrchestrator();
            
        }

        public async Task RunAnalysisPipelineAsync(string[] files)
        {
            // Stage 1: Load Stock Data
            var loadChannel = Channel.CreateBounded<StockData>(50);

            // Stage 2: Calculate Indicators
            var analysisChannel = Channel.CreateBounded<StockAnalysis>(30);

            // Stage 3: Generate Reports
            var reportChannel = Channel.CreateBounded<StockData>(20);

            // Start all stages concurrently 
            var loader = LoadStocksAsync(files, loadChannel.Writer);
            var analyzer = AnalyzeStocksAsync(loadChannel.Reader, analysisChannel.Writer);

            await Task.WhenAll(loader,analyzer);
            Console.WriteLine("Complete pipeline finished");
        }

        private async Task LoadStocksAsync(string[] files, ChannelWriter<StockData> writer)
        {
            await _stockHandler.LoadAllStocksWithConcurrencyControlAsync(files);
            Console.WriteLine("🔄 Stage 1: Loading stock data...");
        }

        private async Task AnalyzeStocksAsync(ChannelReader<StockData> reader, ChannelWriter<StockAnalysis> writer)
        {
            Console.WriteLine("🔄 Stage 2: Analyzing stocks...");
            await foreach (var stock in reader.ReadAllAsync())
            {
                var analysis = await _stockHandler.CalculateTechnicalIndicatorsAsync(stock);
                await writer.WriteAsync(analysis);
            }
            writer.Complete();
        }

        //private async Task GenerateReportsAsync(ChannelReader<StockAnalysis> reader, ChannelWriter<StockReport> writer)
        //{
        //    Console.WriteLine("🔄 Stage 3: Generating reports...");
        //    // Implementation...
        //}

        //private async Task SaveReportsAsync(ChannelReader<StockReport> reader)
        //{
        //    Console.WriteLine("🔄 Stage 4: Saving reports...");
        //    // Final stage - save to database/files
        //}
    }
}
