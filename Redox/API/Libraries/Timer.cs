using System;
using System.Timers;
using UnityEngine;

namespace Redox.API.Libraries
{
    public class Timer : System.Timers.Timer
    {
        private DateTime m_dueTime;

        public Timer() : base()
        {
            this.Elapsed += this.ElapsedAction;
        }

        protected new void Dispose()
        {
            this.Elapsed -= this.ElapsedAction;
            base.Dispose();
        }

        public double TimeLeft
        {
            get
            {
                return (DateTime.Now - this.m_dueTime).TotalMilliseconds;
            }
        }

        public new void Start()
        {
            this.m_dueTime = DateTime.Now.AddMilliseconds(this.Interval);
            base.Start();
        }

        private void ElapsedAction(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (this.AutoReset)
            {
                this.m_dueTime = DateTime.Now.AddMilliseconds(this.Interval);
            }
        }
    }
}
