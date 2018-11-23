using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST.KPI.Utils
{
    public class ApiResult
    {
        public int Code { get; set; }
        public string Message { get; set; }
        public object Data { get; set; }
        public ApiResult()
        {

        }
        public ApiResult(int code, string message)
        {
            this.Code = code;
            this.Message = message;
        }
        public ApiResult(int code, string message, object data) : this(code, message)
        {
            this.Data = data;
        }
    }
}
