using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST.KPI.Service
{
    public class ResponseEventArgs:EventArgs
    {
        public string Message { get; set; }
        public ResponseEventArgs(string message)
        {
            this.Message = message;
        }
    }
}
