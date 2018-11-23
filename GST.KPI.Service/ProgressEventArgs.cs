using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST.KPI.Service
{
    public class ProgressEventArgs:EventArgs
    {
        public double Progress { get; set; }
    }
}
