namespace AsyncProgramming.Models
{
    public class StockAnalysis
    {
        public required string Symbol { get; set; }
        public decimal TargetHigh { get; set; }
        public decimal TargetLow { get; set; }
        public string? Reporter { get; set; }
    }
}
