using AsyncProgramming.Models;
using AsyncProgramming.Utilities;
using System.Runtime.CompilerServices;

namespace AsyncProgramming.Interface
{
    public interface IStockDataHandler
    {
        public StockData LoadStock(string fileName);
        public Task<StockData> LoadStockAsync(string fileName);
        public Task<StockData> LoadStockWithTimeoutAsync(string fileName, TimeSpan span);
        public Task<List<StockData>> LoadAllStockWithProgressAsync(string[] fileNames, 
            IProgress<StockLoadProgress>? progress = default, CancellationToken ct = default);
        public Task<List<StockData>> LoadAllStocksWithConcurrencyControlAsync(string[] fileNames,
            int maxConcurrency = 5, IProgress<StockLoadProgress>? progress = null,
            CancellationToken ct = default);

        public IAsyncEnumerable<StockData> LoadStockAsTheyCompleteAsync(string[] files,
            [EnumeratorCancellation] CancellationToken ect,
            int maxConcurrency = 5);
    }
}
