// See https://aka.ms/new-console-template for more information
using AsyncProgramming.Handlers;
using AsyncProgramming.Utilities;
using System.Diagnostics;


//var fileGen = new FileGenerator();
//fileGen.CreateLargeTestFiles(new string[] { "MSFT", "AMZN", "TSLA", "META", "NVDA", "NFLX", "ADBE", "INTC" });

var stockHandler = new StockDataHandler();
var basePath = AppDomain.CurrentDomain.BaseDirectory;
var stockDirectory = Path.Combine(basePath, "Data");

var stockFiles = Directory.GetFiles(stockDirectory, "*.json");

Stopwatch sw = new();
sw.Start();

var eventOrchestrator = new StockEventOrchestrator();
await eventOrchestrator.RunStockMonitoringSystemAsync(stockFiles);

sw.Stop();
sw.Restart();

var orchestrator = new StockDataOrchestrator();
await orchestrator.ProcessStocksWithProducerConsumerAsync(stockFiles, 3);

var stocksData = stockFiles.Select(file => stockHandler.LoadStock(file)).ToList();
stocksData.ForEach(stock =>
{
    Console.WriteLine(stock.CompanyName);
});

sw.Stop();

Console.WriteLine($"Synchronous processing time: {sw.ElapsedMilliseconds} ms");
sw.Restart();

Console.WriteLine("----- ASYNC/AWAIT -----");
var tasks = stockFiles.Select(file => stockHandler.LoadStockAsync(file));
var asyncResults = await Task.WhenAll(tasks);
asyncResults.ToList().ForEach(x =>
{
    Console.WriteLine(x.CompanyName);
});
sw.Stop();
Console.WriteLine($"Asynchronous processing time: {sw.ElapsedMilliseconds} ms");

sw.Restart();
Console.WriteLine("----- ASYNC/AWAIT WITH PROGRESS -----");
var progressBar = new ConsoleProgressBar();
var progress = new Progress<StockLoadProgress>(progressBar.Report);
CancellationTokenSource cts = new CancellationTokenSource(5000);
CancellationToken ct = cts.Token;
await stockHandler.LoadAllStockWithProgressAsync(stockFiles, progress,ct);
sw.Stop();
Console.WriteLine($"Asynchronous processing time with progress: {sw.ElapsedMilliseconds} ms");

sw.Restart();
Console.WriteLine("----- CONCURRENT ASYNC/AWAIT WITH PROGRESS -----");
var concurrentProgressBar = new ConcurrentProgressBar();
var concurrentProgress = new Progress<StockLoadProgress>(concurrentProgressBar.Report);
await stockHandler.LoadAllStocksWithConcurrencyControlAsync(stockFiles,4, concurrentProgress, ct);
sw.Stop();
Console.WriteLine($"Concurrent Asynchronous processing time with progress: {sw.ElapsedMilliseconds} ms");

sw.Restart();
Console.WriteLine("----- CONCURRENT ASYNC/AWAIT WITH STREAMING -----");
await foreach (var stock in stockHandler.LoadStockAsTheyCompleteAsync(stockFiles, ct, 3))
{
    Console.WriteLine($"Streaming Stock -- {stock.Symbol}");
}
sw.Stop();
Console.WriteLine($"Concurrent Streaming processing time with progress: {sw.ElapsedMilliseconds} ms");



