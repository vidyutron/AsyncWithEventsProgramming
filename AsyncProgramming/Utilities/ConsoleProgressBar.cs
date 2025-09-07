namespace AsyncProgramming.Utilities
{
    public class ConsoleProgressBar
    {
        private readonly int _width;
        private int _lastLength = 0;
        public ConsoleProgressBar(int width = 50)
        {
            _width = width;   
        }

        public void Report(StockLoadProgress progress)
        {
            // Clear entire line and return to start
            Console.Write($"\r{new string(' ', Console.WindowWidth - 1)}\r");

            // Build progress bar
            var percent = (int)progress.PercentageCompleted;
            var filled = Math.Min(50, (int)(progress.PercentageCompleted / 100 * 50));
            var bar = new string('█', filled) + new string('░', 50 - filled);

            // Use filename only, not full path
            var fileName = Path.GetFileName(progress.CurrentFile);

            var status = progress.HasError
                ? $"ERROR: {progress.ErorrMessage?[..Math.Min(30, progress.ErorrMessage.Length)]}"
                : $"Loading {fileName}";

            var output = $"[{bar}] {percent:D3}% ({progress.CompletedCount}/{progress.TotalCount}) {status}";

            Console.Write(output);

            if (progress.CompletedCount == progress.TotalCount)
            {
                Console.WriteLine("\n✅ All files processed!");
            }
        }
    }
}
