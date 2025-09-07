# 🚀 Async & Event Programming

> **A comprehensive, hands-on journey through advanced C# async programming, concurrency patterns, and event-driven architecture using real-world stock data analysis.**

[![MIT License](https://img.shields.io/badge/License-MIT-green.svg)](LICENSE)
[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/9.0)
[![C# 13](https://img.shields.io/badge/C%23-13.0-239120.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)

## 📖 What This Project Demonstrates

This isn't just another async programming tutorial. It's a **progressive learning system** that builds from basic concepts to enterprise-grade patterns, using stock market data analysis as a practical, engaging context.

### 🎯 Core Learning Objectives

- **Master async/await fundamentals** with real I/O operations
- **Control concurrency** without overwhelming system resources  
- **Stream data efficiently** using producer-consumer patterns
- **Build event-driven systems** with proper decoupling
- **Handle errors gracefully** in concurrent environments
- **Measure and optimize performance** of async operations

## 🏗️ Architecture Overview

┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐
│ Stock Data │───▶	│ Async I/O │───▶		│ Processing │
│ Files (JSON) │ │ Handler │ │ Pipeline │
└─────────────────┘ └─────────────────┘ └─────────────────┘
│
▼
┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐
│ Progress │◀───		│ Concurrency │───▶			│ Producer │
│ Reporting │ │ Control │ │ Consumer │
└─────────────────┘ └─────────────────┘ └─────────────────┘
│
▼
┌─────────────────┐ ┌─────────────────┐ ┌─────────────────┐
│	Event-Driven │◀───│ Real-time │───▶	│ Alert │
│ Monitoring │		│ Streaming │			│ System │
└─────────────────┘ └─────────────────┘ └─────────────────┘


## 🔧 Key Components

### 📁 **StockDataHandler** 
Basic async file I/O with proper resource management and error handling.

### 🎛️ **Concurrency Control**
SemaphoreSlim-based throttling to prevent system overload when processing multiple files.

### 📡 **Producer-Consumer System**
Channel-based streaming with backpressure control for real-time data processing.

### 🚨 **Event-Driven Monitoring**
Sophisticated event system with stock alerts, trading bots, and risk management subscribers.

## 🚀 Quick Start

Clone the repository
git clone https://github.com/vidyutron/AsyncWithEventsProgramming.git

Navigate to project directory
cd AsyncWithEventsProgramming

Run the application
dotnet run


## 💡 Learning Progression

### **Phase 1: Async Fundamentals**

From blocking I/O
```
var data = File.ReadAllText("stock.json");
```

To non-blocking async
```
var data = await File.ReadAllTextAsync("stock.json");
```


### **Phase 2: Concurrency Control** 

Controlled concurrent processing
```
using var semaphore = new SemaphoreSlim(5, 5);
await semaphore.WaitAsync();
try { await ProcessFileAsync(file); }
finally { semaphore.Release(); }
```


### **Phase 3: Streaming with Channels**
Real-time data streaming

```
var channel = Channel.CreateBounded<StockData>(10);
var producer = ProduceDataAsync(files, channel.Writer);
var consumer = ConsumeDataAsync(channel.Reader);
await Task.WhenAll(producer, consumer);
```



### **Phase 4: Event-Driven Architecture**
Reactive monitoring system
```
monitor.PriceAlert += tradingBot.HandlePriceAlert;
monitor.VolumeAlert += riskManager.HandleVolumeAlert;
```


## 🎪 Interactive Demonstrations

### **Concurrency Benchmarks**
Watch performance scale with different concurrency levels:

| Level | Time(ms) | Throughput |
|-------|----------|------------|
| 1     | 1200ms   | 10.8/s     |
| 5     | 250ms    | 52.0/s     |
| 10    | 280ms    | 46.4/s     |

### **Real-Time Event Processing**
See live alerts as data streams:
📤 Produced: AAPL
📥 Processed: AAPL (Total: 1)
🚨 PRICE ALERT: AAPL changed 7.3% ($275.23 → $251.47)
🤖 AlgoTrader-1: Considering BUY order for AAPL



## 📚 What You'll Learn

### **Enterprise Patterns**
- ✅ **Producer-Consumer** with backpressure control
- ✅ **Observer Pattern** through C# events
- ✅ **Resource pooling** with SemaphoreSlim
- ✅ **Graceful degradation** in error scenarios
- ✅ **Progress reporting** for long-running operations

### **Performance Optimization**
- ✅ **Concurrent vs. parallel** execution understanding
- ✅ **Memory-efficient streaming** without accumulation
- ✅ **Bottleneck identification** and resolution
- ✅ **Async overhead** vs. benefits trade-offs

### **Real-World Skills**
- ✅ **Financial data processing** patterns
- ✅ **Event-driven architecture** design
- ✅ **System monitoring** and alerting
- ✅ **Scalable async applications** that handle high throughput

## 🎯 Why This Approach Works

### **Learning by Building**
Instead of abstract examples, every concept is applied to a **realistic stock analysis system** that you might actually build in the real world.

### **Progressive Complexity**
Each phase builds naturally on the previous one, creating a **scaffolded learning experience** that prevents overwhelm.

### **Immediate Feedback**
Console output and performance metrics provide **instant validation** that concepts are working correctly.

### **Enterprise Ready**
The patterns demonstrated are used in **production systems** at financial firms, trading platforms, and data processing companies.

## 🔬 Technical Deep Dives

### **Async Streaming Performance**
Learn why `IAsyncEnumerable` can be simpler than channels, but channels provide better backpressure control for high-throughput scenarios.

### **Event System Design**
Discover the difference between delegates, events, and the observer pattern, with practical examples of decoupled architecture.

### **Concurrency Sweet Spots**
Understand why more concurrency isn't always better, and how to find optimal performance points for your system.

## 🎨 Innovation Highlights

- **Real-world context** makes abstract concepts tangible
- **Performance visualization** shows async benefits clearly
- **Error simulation** teaches resilient system design
- **Modular architecture** allows easy experimentation
- **Progressive disclosure** of complexity prevents cognitive overload

## 🤝 Contributing

This is a **learning-focused project**. Contributions that enhance the educational value are welcome:
- Additional async patterns and examples
- Performance optimizations with explanations
- Real-world scenario expansions
- Documentation improvements

## 📜 License

MIT License - See [LICENSE](LICENSE) for details.

---

### 💭 For Future Self & Others

This project represents a **complete learning journey** through advanced C# async programming. Each commit builds understanding systematically, from basic file I/O to sophisticated event-driven systems.

**Key insight**: Async programming isn't just about performance—it's about building **responsive, scalable systems** that can handle real-world complexity gracefully.

**Start here** if you want to truly understand async programming beyond the basics. **End here** with production-ready patterns you can apply immediately.

---

*Built with ❤️ for developers who want to master async programming through practical, hands-on experience.*


