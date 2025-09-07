using AsyncProgramming.Models;

namespace AsyncProgramming.Events
{
    public class StockMonitor
    {
        public event EventHandler<StockAlertEventArgs>? PriceAlert;
        public event EventHandler<StockAlertEventArgs>? VolumeAlert;
        public event Action<string>? SystemStatus;

        private readonly Dictionary<string, decimal> _lastPrices = new();

        public async Task MonitorStocksAsync(IAsyncEnumerable<StockData> stocksStream)
        {
            OnSystemStatus($"Starting Stock Monitoring");

            await foreach(var stock in stocksStream)
            {
                AnalyzeStockForAlerts(stock);
            }

            OnSystemStatus($"Stock Monitoring Completed");
        }

        private void AnalyzeStockForAlerts(StockData stock)
        {
            var currentPrice = stock.DailyData.Last().Close;
            var currentVolume = stock.DailyData.Last().Volume;

            // SIMPLE FIX: Pre-populate some mock previous prices for demo
            if (!_lastPrices.ContainsKey(stock.Symbol))
            {
                // First time seeing this stock, add a mock "previous" price
                var random = new Random();
                var mockPreviousPrice = (double)currentPrice * (1 + (random.NextDouble() - 0.5) * (double)0.15m); // ±7.5% variation
                _lastPrices[stock.Symbol] = (decimal)mockPreviousPrice;

                Console.WriteLine($"📊 Mock previous price for {stock.Symbol}: ${mockPreviousPrice:F2}");
            }

            // Check for price alerts
            if (_lastPrices.TryGetValue(stock.Symbol,out var lastPrice))
            {
                var changedPercentage = Math.Abs((lastPrice - currentPrice)/lastPrice*100);
                if (changedPercentage > 5)
                {
                    OnPriceAlert(new StockAlertEventArgs(stock, currentPrice, lastPrice, "Significant Price Movement"));
                }
            }

            // Check for volume alerts
            var avgVolume = stock.DailyData.Take(5).Average(d => d.Volume);
            if(currentVolume > avgVolume)
            {
                OnVolumeAlert(new StockAlertEventArgs(stock, currentPrice, 0, "High Volume"));
            }

            _lastPrices[stock.Symbol] = currentPrice;
        }

        protected virtual void OnVolumeAlert(StockAlertEventArgs e)
        {
            VolumeAlert?.Invoke(this, e);
        }

        protected virtual void OnPriceAlert(StockAlertEventArgs e)
        {
            PriceAlert?.Invoke(this, e);
        }

        protected virtual void OnSystemStatus(string message)
        {
            SystemStatus?.Invoke(message);
        }
    }
}
