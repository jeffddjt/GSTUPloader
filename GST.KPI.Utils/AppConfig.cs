using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace GST.KPI.Utils
{
    public static class AppConfig
    {
        private static string configFileName = "gstconfig.json";
        public static string ConnectionString
        {
            get
            {
                var filename = Environment.CurrentDirectory + "\\" + configFileName;
                string jsonStr = string.Empty;
                FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
                using(StreamReader sr=new StreamReader(fs, Encoding.UTF8))
                {
                    jsonStr = sr.ReadToEnd();
                }
                JObject json = JObject.Parse(jsonStr);
                return json.GetValue("connectionString").ToString();
            }
        }
    }
}
