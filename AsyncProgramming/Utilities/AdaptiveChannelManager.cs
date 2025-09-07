using System.Runtime.InteropServices.ObjectiveC;
using System.Threading.Channels;

namespace AsyncProgramming.Utilities
{
    public class AdaptiveChannelManager<T>
    {
        private Channel<T> _channel;
        private int _currentCapacity = 10;
        private readonly int _maxCapacity = 1000;
        private readonly Timer _adaptionTimer;

        public AdaptiveChannelManager()
        {
            RecreateChannel();
            _adaptionTimer = new Timer(AdaptCapacity, null, TimeSpan.FromSeconds(5),TimeSpan.FromSeconds(5));
        }

        private void AdaptCapacity(object state)
        {
            var queueLength = _channel.Reader.Count; // Approximate

            if (queueLength > _currentCapacity * 0.8) // 80% full - increase capacity
            {
                _currentCapacity = Math.Min(_maxCapacity, _currentCapacity * 2);
                RecreateChannel();
                Console.WriteLine($"📈 Increased channel capacity to {_currentCapacity}");
            }
            else if (queueLength < _currentCapacity * 0.2) // 20% full - decrease capacity
            {
                _currentCapacity = Math.Max(10, _currentCapacity / 2);
                RecreateChannel();
                Console.WriteLine($"📉 Decreased channel capacity to {_currentCapacity}");
            }
        }

        private void RecreateChannel()
        {
            _channel = Channel.CreateBounded<T>(_currentCapacity);
        }
    }
}
