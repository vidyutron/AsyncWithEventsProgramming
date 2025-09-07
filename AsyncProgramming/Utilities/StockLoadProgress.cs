namespace AsyncProgramming.Utilities
{
    public record StockLoadProgress(int CompletedCount, int TotalCount, string CurrentFile, string? ErorrMessage = null)
    {
        public double PercentageCompleted => TotalCount > 0 ? (double)CompletedCount / TotalCount * 100 : 0;
        public bool HasError => !string.IsNullOrEmpty(ErorrMessage);
    }
}
