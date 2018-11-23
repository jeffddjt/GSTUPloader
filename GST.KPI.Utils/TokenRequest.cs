using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST.KPI.Utils
{
    public class TokenRequest
    {
        public string Client_ID { get; set; }
        public string Client_Secret { get; set; }
        public string Grant_Type { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public TokenRequest()
        {
            this.Client_ID = "client2";
            this.Client_Secret = "secret";
            this.Grant_Type = "password";
        }
        public TokenRequest(string username,string password):this()
        {
            this.Username = username;
            this.Password = password;
        }

        public string ToUrlParameter()
        {
            Type type = this.GetType();
            List<string> param = new List<string>();
            foreach(var property in type.GetProperties())
            {
                param.Add(string.Format("{0}={1}", property.Name.ToLower(), property.GetValue(this,null)));
            }
            return string.Join("&", param);
        }
    }
}
