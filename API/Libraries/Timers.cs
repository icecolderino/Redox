using System;
using System.Collections.Generic;
using System.Timers;


namespace Redox.API.Libraries
{
    public enum TimerType : ushort
    {
        Once = 0,
        Repeat = 1
    }

    public class Timers
    {
        /// <summary>
        /// The amount of running timers
        /// </summary>
        public int Count
        {
            get { return timers.Count; }
        }

        internal static readonly HashSet<Timer> timers = new HashSet<Timer>();

        /// <summary>
        /// Creates a new timer.
        /// </summary>
        /// <param name="interval">The timer interval in milliseconds.</param>
        /// <param name="timerType">The type of the timer.</param>
        /// <param name="callBack">The callback of the timer.</param>
        /// <param name="repeatRate">The times the timer should execute.</param>
        /// <param name="startOnJoin">Start the timer immediately after creation?</param>
        /// <returns></returns>
        public Timer Create(double interval, TimerType timerType, Action<Timer> callBack, int repeatRate = 0,
            bool startOnJoin = true)
        {
            var timer = new Timer(interval, timerType, callBack, startOnJoin, repeatRate);
            timers.Add(timer);
            return timer;
        }
    }

    public class Timer : IDisposable
    {
        private DateTime _startTime;

        private readonly int _repeatRate = 0;
        private int _repeated = 0;

        private readonly Action<Timer> _callBack;

        private readonly double _interval;

        private readonly TimerType _timerType;

        private System.Timers.Timer _timer;

        public bool IsDestroyed { get; private set; }

        public Dictionary<string, object> Data;

        public double TimeLeft
        {
            get
            {
                return (DateTime.Now - _startTime).TotalMilliseconds;
            }
        }

        public Timer(double interval, TimerType timerType, Action<Timer> callBack, bool startOnJoin, int repeatRate)
        {
            _callBack = callBack;
            _interval = interval;
            _repeatRate = repeatRate;
            _timerType = timerType;
            Data = new Dictionary<string, object>();
            if (startOnJoin)
                this.Start();
        }

        public void Start()
        {
            _startTime = DateTime.Now.AddMilliseconds(_interval);
            _timer = new System.Timers.Timer
            {
                Interval = _interval,
                AutoReset = _timerType == TimerType.Once ? false : true,
                Enabled = true
            };
            _timer.Elapsed += (x, y) =>
            {
                _callBack.Invoke(this);

                if (_timerType == TimerType.Once)
                    this.Stop();
                else if (_repeatRate > 0)
                {
                    if (_repeated == _repeatRate)
                        this.Stop();
                    else
                        _repeated++;
                }
            };
        }
        public void Stop()
        {
            IsDestroyed = true;
            _timer.Stop();
            _timer.Dispose();
            Timers.timers.Remove(this);
            this.Dispose();
        }

        ~Timer()
        {
            this.Dispose(false);
        }
        public void Dispose()
        {
            this.Dispose(true);
            System.GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
        }
    }
}
