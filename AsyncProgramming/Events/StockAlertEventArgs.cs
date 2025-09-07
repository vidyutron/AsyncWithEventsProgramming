using AsyncProgramming.Models;

namespace AsyncProgramming.Events
{
    public class StockAlertEventArgs:EventArgs
    {
        public StockData? Stock { get; }
        public decimal CurrentPrice { get; }
        public decimal PreviousPrice { get; }

        public DateTime AlertTime { get;}
        public string? AlertType { get; }

        public StockAlertEventArgs(StockData stock, 
            decimal currentPrice, 
            decimal previousPrice, 
            string  alertType)
        {
            Stock = stock;
            CurrentPrice = currentPrice;
            PreviousPrice = previousPrice;
            AlertType = alertType;
            AlertTime = DateTime.UtcNow;
        }
    }
}
