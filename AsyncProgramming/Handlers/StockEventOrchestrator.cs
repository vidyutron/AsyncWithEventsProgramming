using AsyncProgramming.Events;
using AsyncProgramming.Models;
using AsyncProgramming.Utilities;

namespace AsyncProgramming.Handlers
{
    public class StockEventOrchestrator
    {
        public async Task RunStockMonitoringSystemAsync(string[] files)
        {
            // Create event publisher
            var monitor = new StockMonitor();

            // Create event subscribers
            var tradingBotOne = new TradingBot("AlgoTrader-1");
            var tradingBotTwo = new TradingBot("AlgoTrader-2");
            var tradingBotThree = new TradingBot("AlgoTrader-3");

            var riskManager = new RiskManager();
            var notificationService = new NotificationService();

            //Subscribe to events
            monitor.PriceAlert += tradingBotOne.HandlePriceAlert;
            monitor.PriceAlert += tradingBotTwo.HandlePriceAlert;
            monitor.PriceAlert += riskManager.HandlePriceAlert;

            monitor.VolumeAlert += tradingBotThree.HandleVolumeAlert;

            monitor.SystemStatus += notificationService.HandleSystemStatus;

            // Load and stream stock data
            var stockHandler = new StockDataHandler();
            var stockStream = LoadStocksAsStreamAsync(files, stockHandler);

            // Start Monitoring 
            await monitor.MonitorStocksAsync(stockStream);
        }

        private async IAsyncEnumerable<StockData> LoadStocksAsStreamAsync(string[] files, StockDataHandler stockHandler)
        {
            foreach (var file in files)
            {
                var stock = await stockHandler.LoadStockAsync(file);
                yield return stock;

                await Task.Delay(100);
            }
        }
    }
}
