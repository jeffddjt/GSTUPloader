using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST.KPI.Service
{
    public class SuccEventArgs:EventArgs
    {
        public TimeSpan Duration { get; set; }
        public int Count { get; set; }

        public SuccEventArgs(TimeSpan duration,int count)
        {
            this.Duration = duration;
            this.Count = count;
        }
    }
}
