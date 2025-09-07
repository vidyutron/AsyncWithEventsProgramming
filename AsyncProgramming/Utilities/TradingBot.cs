using AsyncProgramming.Events;

namespace AsyncProgramming.Utilities
{
    public class TradingBot
    {
        private readonly string _name;
        public TradingBot(string name)
        {
            _name = name;
        }

        public void HandlePriceAlert(object? sender, StockAlertEventArgs e)
        {
            Console.WriteLine($"🤖 {_name}: PRICE ALERT for {e.Stock.Symbol}");
            Console.WriteLine($"   Price changed from ${e.PreviousPrice:F2} to ${e.CurrentPrice:F2}");
            Console.WriteLine($"   Alert: {e.AlertType} at {e.AlertTime:HH:mm:ss}");

            if(e.CurrentPrice > e.PreviousPrice)
            {
                Console.WriteLine($"Consider buying the stock - {e.Stock.Symbol}");
            }
            else
            {
                Console.WriteLine($"Consider selling the stock - {e.Stock.Symbol}");
            }
        }

        public void HandleVolumeAlert(object? sender, StockAlertEventArgs e)
        {
            Console.WriteLine($"🤖 {_name}: VOLUME SPIKE for {e.Stock.Symbol} - Investigating...");
        }
    }
}
