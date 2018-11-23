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
        static AppConfig()
        {
            var filename = Environment.CurrentDirectory + "\\" + configFileName;
            string jsonStr = string.Empty;
            FileStream fs = new FileStream(filename, FileMode.Open, FileAccess.Read);
            using (StreamReader sr = new StreamReader(fs, Encoding.UTF8))
            {
                jsonStr = sr.ReadToEnd();
            }
            JObject json = JObject.Parse(jsonStr);
            ConnectionString= json.GetValue("connectionString").ToString();
            DESEncryptKey= json.GetValue("DESEncryptKey").ToString();
            URL = json.GetValue("URL").ToString();
            ItemsOnce = (int)json.GetValue("ItemsOnce");
            UserName = json.GetValue("UserName").ToString();
            Password = json.GetValue("Password").ToString();
        }
        public static string ConnectionString;
        public static string DESEncryptKey;
        public static string URL;
        public static int ItemsOnce;
        public static string UserName;
        public static string Password;
    }
}
