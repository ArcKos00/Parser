using System.Diagnostics;

namespace Parser.Business.Parser.Wrapper
{
    internal class StopWatchWrapper : IDisposable
    {
        private readonly Stopwatch _stopwatch;
        private bool _disposed = false;

        public StopWatchWrapper()
        {
            _stopwatch = new Stopwatch();
            Start();
        }

        public void Start()
        {
            Console.WriteLine("Start");
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
            Console.WriteLine("End");
            var ts = _stopwatch.Elapsed;
            Console.WriteLine(string.Format($"Time: {ts.Hours}:{ts.Minutes}:{ts.Seconds}.{ts.Milliseconds / 10}"));
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            _disposed = true;
            Stop();
            Console.WriteLine($"disposed object: {_disposed}");
        }
    }
}
