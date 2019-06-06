using System.Diagnostics;

namespace CUtility
{
    public class JTimer
    {
        public long DeltaMS
        {
            get
            {
                long elapsed = _lastMs;
                _lastMs = _stopwatch.ElapsedMilliseconds;
                return _lastMs - elapsed;
            }
        }

        private readonly Stopwatch _stopwatch = new Stopwatch();

        private long _lastMs;

        public void Start()
        {
            _stopwatch.Start();
        }

        public void Stop()
        {
            _stopwatch.Stop();
        }
    }
}
