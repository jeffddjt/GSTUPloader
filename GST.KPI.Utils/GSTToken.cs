using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST.KPI.Utils
{
    public class GSTToken
    {
        public string Access_Token { get; set; }
        public int Expires_In { get; set; }
        public string Token_Type { get; set; }
    }
}
