using GST.KPI.Repositories.DAO;
using GST.KPI.Repositories.Models;
using GST.KPI.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GST.KPI.Service
{
    public class UploadService
    {
        private readonly string connStr;
        private string url = AppConfig.URL;

        public event EventHandler<ProgressEventArgs> Processed;
        public event EventHandler<SuccEventArgs> Successed;
        public event EventHandler<ResponseEventArgs> Responsed;

        public UploadService(string connStr)
        {
            this.connStr = connStr;
        }

        public bool Running { get; set; }

        public IList<KPIOrder> GetOrderList()
        {
            using(GstDbContext context=new GstDbContext(this.connStr))
            {
                IList<KPIOrder> list = context.KPIOrder.Take(20).ToList();
                return list;
            }
        }

        public void UploadOrder()
        {
            Thread thread = new Thread(()=> 
            {
                this.Running = true;
                Stopwatch watch = Stopwatch.StartNew();
                this.Responsed?.Invoke(this, new ResponseEventArgs("已启动上传任务!"));
                TokenRequest request = new TokenRequest(AppConfig.UserName, AppConfig.Password);
                string result = string.Empty;
                GSTToken token = null;
                this.Responsed?.Invoke(this, new ResponseEventArgs("正在登录服务器......"));
                if (this.SendToCloud(request.ToUrlParameter(), this.url+"/connect/token", out result))
                {
                    token = JObject.Parse(result).ToObject<GSTToken>();
                }
                else
                {
                    this.Responsed?.Invoke(this, new ResponseEventArgs(result));
                    return;
                }
                int count = 0;
                int progress = 0;
                this.Responsed?.Invoke(this, new ResponseEventArgs("正在获取本地数据......"));
                using (GstDbContext context=new GstDbContext(this.connStr))
                {
                    int pageSize = AppConfig.ItemsOnce;
                    int pageNo = 1;
                    int total = context.KPIOrder.Count();
                    var list = context.KPIOrder.OrderBy(p => p.ID).Skip((pageNo - 1) * pageSize).Take(pageSize).ToList();
                    this.Responsed?.Invoke(this, new ResponseEventArgs("正在上传数据......"));
                    while (list.Count > 0)
                    {
                        try
                        {
                            var jsonStr = JsonConvert.SerializeObject(list, Formatting.None);
                            string data = DESEncrypt.Encrypt(jsonStr);
                            var transfer = new OrderTransfer { Content = data };
                            var dataString = JObject.FromObject(transfer).ToString();
                            string response = string.Empty;
                            this.SendToCloud(dataString, this.url+"/api/order/uploadorder", out response,token.Token_Type + " " + token.Access_Token);
                            try
                            {
                                ApiResult apiResult = JObject.Parse(response).ToObject<ApiResult>();
                                if (apiResult.Code == 0)
                                    count += list.Count;
                                else
                                    this.Responsed?.Invoke(this, new ResponseEventArgs(apiResult.Message));

                            }
                            catch(Exception ex)
                            {
                                this.Responsed?.Invoke(this, new ResponseEventArgs(response));
                            }
                            progress += list.Count;
                            list = context.KPIOrder.OrderBy(p => p.ID).Skip((++pageNo - 1) * pageSize).Take(pageSize).ToList();                            
                            this.Processed?.Invoke(this, new ProgressEventArgs() { Progress = progress*1.0 / total * 100 });                            
                        }
                        catch(Exception ex)
                        {
                            this.Responsed?.Invoke(this, new ResponseEventArgs(ex.Message));
                        }
                    }
                }
                watch.Stop();
                this.Successed?.Invoke(this, new SuccEventArgs(watch.Elapsed,count));
                this.Running = false;
            });
            thread.IsBackground = true;
            thread.Start();
        }
        private bool SendToCloud(byte[] data, string url, out string result, string header = null)
        {
            try
            {
                byte[] buf = data;
                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                request.ContentType = "application/x-www-form-urlencoded";
                if (header != null)
                    request.Headers.Add(HttpRequestHeader.Authorization, header);

                request.Method = "POST";
                //request.ContentLength = buf.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(buf, 0, buf.Length);
                stream.Close();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream res = response.GetResponseStream();
                string str = string.Empty;
                using (StreamReader sr = new StreamReader(res, Encoding.UTF8))
                {
                    str = sr.ReadToEnd();
                }
                result = str;
                return true;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                return false;
            }
        }
        private bool SendToCloud(string data, string url,out string result,string header=null)
        {
            try
            {
                byte[] buf = Encoding.UTF8.GetBytes(data);

                HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
                    request.ContentType = "application/x-www-form-urlencoded";
                if (header != null)
                {
                    request.Headers.Add(HttpRequestHeader.Authorization, header);
                    request.ContentType = "application/json";
                }
                request.Method = "POST";
                //request.ContentLength = buf.Length;
                Stream stream = request.GetRequestStream();
                stream.Write(buf, 0, buf.Length);
                stream.Close();
                HttpWebResponse response = request.GetResponse() as HttpWebResponse;
                Stream res = response.GetResponseStream();
                string str = string.Empty;
                using (StreamReader sr = new StreamReader(res, Encoding.UTF8))
                {
                    str = sr.ReadToEnd();
                }
                result = str;
                return true;
            }
            catch (Exception ex)
            {
                result = ex.ToString();
                return false;
            }
        }
    }
}
