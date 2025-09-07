using AsyncProgramming.Interface;
using AsyncProgramming.Models;
using AsyncProgramming.Utilities;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace AsyncProgramming.Handlers
{
    public class StockDataHandler : IStockDataHandler
    {
        public StockData LoadStock(string fileName)
        {
            try
            {
                using (FileStream fs = new(fileName, FileMode.Open, FileAccess.Read))
                {
                    var stockData = JsonSerializer.Deserialize<StockData>(fs);
                    return stockData!;

                }
            }
            catch(FileNotFoundException ex)
            {
                throw;
            }
            catch(JsonException ex)
            {
                throw;
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public async Task<StockData> LoadStockAsync(string fileName)
        {
            try
            {
                using (FileStream fs = new(fileName, FileMode.Open, FileAccess.Read))
                {
                    var stockData = await JsonSerializer.DeserializeAsync<StockData>(fs);
                    return stockData!;
                }
            }
            catch (FileNotFoundException ex)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<StockData> LoadStockWithTimeoutAsync(string fileName, 
            TimeSpan timeSpan)
        {
            CancellationTokenSource cts = new(timeSpan);

            try
            {
                using (FileStream fs = new(fileName, FileMode.Open, FileAccess.Read))
                {
                    var stockData = await JsonSerializer.DeserializeAsync<StockData>(fs, cancellationToken:cts.Token);
                    return stockData!;
                }
            }
            catch (OperationCanceledException ex)
            {
                throw;
            }
            catch (FileNotFoundException ex)
            {
                throw;
            }
            catch (JsonException ex)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<List<StockData>> LoadAllStockWithProgressAsync(string[] fileNames, 
            IProgress<StockLoadProgress>? progress = null, 
            CancellationToken ct = default)
        {
            var results = new List<StockData>();
            var errors = new List<string>();
            var totalFiles = fileNames.Length;

            for (int i = 0; i < fileNames.Length; i++)
            {
                var currentFile = fileNames[i];
                progress?.Report(new StockLoadProgress(i, totalFiles, currentFile));
                try
                {
                    var stockData = await LoadStockAsync(currentFile);
                    results.Add(stockData);

                    progress?.Report(new StockLoadProgress(i + 1, totalFiles, currentFile));
                }
                catch (Exception ex)
                {
                    errors.Add($"Error loading {currentFile}: {ex.Message}");
                    progress?.Report(new StockLoadProgress(i + 1, totalFiles, currentFile, ex.Message));
                }
            }

            return results;
        }

        public async Task<List<StockData>> LoadAllStocksWithConcurrencyControlAsync(string[] fileNames,
            int maxConcurrency = 5, IProgress<StockLoadProgress>? progress = null, 
            CancellationToken ct = default)
        {
            using var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);
            var results = new ConcurrentBag<StockData>();
            var errors = new ConcurrentBag<string>();
            var totalFiles = fileNames.Length;
            long completed = 0;

            var tasks = fileNames.Select(async (file, index) => {
                await semaphore.WaitAsync(ct);
                try
                {
                    var currentCompleted = Interlocked.Read(ref completed);
                    progress?.Report(new StockLoadProgress((int)currentCompleted, totalFiles, file));

                    var stockData = await LoadStockAsync(file);
                    results.Add(stockData);

                    var newCompleted = Interlocked.Increment(ref completed);
                    progress?.Report(new StockLoadProgress((int)newCompleted, totalFiles, file));
                }
                catch(Exception ex)
                {
                    errors.Add($"FileName: {file}");
                    var newCompleted = Interlocked.Increment(ref completed);
                    progress?.Report(new StockLoadProgress((int)newCompleted, totalFiles, file,ex.Message));
                }
                finally
                {
                    semaphore.Release();
                }
            });

            await Task.WhenAll(tasks);
            return results.ToList();
        }

        public async Task<StockAnalysis> CalculateTechnicalIndicatorsAsync(StockData stockData)
        {
            var analysis = await Task.Run(() => new StockAnalysis
            {
                Symbol = stockData.Symbol,
                TargetHigh = stockData.DailyData.Average(x=>x.High),
                TargetLow = stockData.DailyData.Average(x => x.Low),
                Reporter = "Rand Graham"
            });

            return analysis;
        }

        public async IAsyncEnumerable<StockData> LoadStockAsTheyCompleteAsync(string[] files, 
            [EnumeratorCancellation] CancellationToken ect,int maxConcurrency = 5)
        {
            using var semaphore = new SemaphoreSlim(maxConcurrency, maxConcurrency);

            var tasks = files.Select(async file =>
            {
                await semaphore.WaitAsync();

                try
                {
                    await Task.Delay(500);
                    return await LoadStockAsync(file);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToList(); // Force the tasks execution, else LINQ is deferred execution.

            while (tasks.Any())
            {
                var completed = await Task.WhenAny(tasks);
                tasks.Remove(completed);

                yield return await completed;
            }
        }
    }
}
