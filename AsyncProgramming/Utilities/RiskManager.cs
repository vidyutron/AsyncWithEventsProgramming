using AsyncProgramming.Events;

namespace AsyncProgramming.Utilities
{
    public class RiskManager
    {
        public void HandlePriceAlert(object? sender, StockAlertEventArgs e)
        {
            Console.WriteLine($"⚠️  Risk Manager: Evaluating risk for {e.Stock.Symbol}");

            var changePercent = Math.Abs((e.CurrentPrice - e.PreviousPrice) / e.PreviousPrice * 100);
            if (changePercent > 10)
            {
                Console.WriteLine($"   🚨 HIGH RISK: {changePercent:F1}% change detected!");
            }
        }
    }
}
