namespace AsyncProgramming.Utilities
{
    public class ConcurrentProgressBar
    {
        private readonly object _lock = new object();
        public void Report(StockLoadProgress progress) 
        {
            lock (_lock)
            {
                Console.Write($"\r{new string(' ', 100)}\r");

                var percent = (int)progress.PercentageCompleted;
                var filled = Math.Min(40, (int)(progress.PercentageCompleted / 100 * 40));
                var bar = new string('█', filled) + new string('░', 40 - filled);

                var fileName = Path.GetFileName(progress.CurrentFile);
                var status = progress.HasError ? "❌ ERROR" : "📁 Loading";

                Console.Write($"[{bar}] {percent:D3}% ({progress.CompletedCount}/{progress.TotalCount}) {status}: {fileName}");

                if (progress.CompletedCount == progress.TotalCount)
                {
                    Console.WriteLine();
                    Console.WriteLine($"✅ All files processed with controlled concurrency!");
                }
            }
        }
    }
}
