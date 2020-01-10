using System;
using System.Collections.Generic;
using System.Timers;
using UnityEngine;


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
        public static int Count
        {
            get
            {
                return _timers.Count;
            }
            private set
            {

            }

        }

        internal static readonly HashSet<Timer> _timers = new HashSet<Timer>();

        public static Timer Create(double interval, TimerType timerType, Action callBack, int repeatRate = 0, bool startOnJoin = true)
        {
            var timer = new Timer(interval, timerType, callBack, startOnJoin, repeatRate);
            _timers.Add(timer);
            return timer;
        }

        public class Timer : IDisposable
        {
            private DateTime _startTime;

            private int _repeatRate;

            private int _repeated = 0;

            private readonly Action _callBack;

            private readonly double _interval;

            private readonly TimerType _timerType;

            private System.Timers.Timer _timer;


            public double TimeLeft
            {
                get
                {
                    return (DateTime.Now - _startTime).TotalMilliseconds;
                }
            }

            public Timer(double Interval, TimerType timerType, Action callBack, bool startOnJoin, int repeatRate)
            {
                _callBack = callBack;
                _interval = Interval;
                _timerType = timerType;

                if(startOnJoin)
                    this.Start();
            }

            public void Start()
            {
                Count++;
                _startTime = DateTime.Now.AddMilliseconds(_interval);
                _timer = new System.Timers.Timer();
                _timer.Interval = _interval;
                _timer.AutoReset = _timerType == TimerType.Once ? false : true;
                _timer.Enabled = true;
                _timer.Elapsed += (x, y) =>
                {
                    _callBack.Invoke();

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
                _timer.Stop();
                _timer.Dispose();
                _timers.Remove(this);
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
}
