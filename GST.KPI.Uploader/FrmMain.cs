using GST.KPI.Components;
using GST.KPI.Service;
using GST.KPI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace GST.KPI.Uploader
{
    public partial class FrmMain : Form
    {
        private UploadService uploadService;

        public FrmMain()
        {
            InitializeComponent();
            this.lbStatus.Text = this.lpStatus.Status.ToText();
        }

        private void lpStatus_StatusChanged(object sender, EventArgs e)
        {
            var str = (sender as Lamp).Status.ToText();
            this.lbStatus.Text = str;
            this.stLable.Text = str;
        }

        private void btnStart_Click(object sender, EventArgs e)
        {
            if (this.lpStatus.Status != KPIStatus.Running)
            {
                (sender as Button).Text = "停止";
                this.lpStatus.Status = KPIStatus.Running;
                this.dateTimePicker1.Enabled = false;
                this.kpiTimer.Start();
            } else
            {
                (sender as Button).Text = "运行";
                this.lpStatus.Status = KPIStatus.Stoped;
                this.dateTimePicker1.Enabled = true;
                this.kpiTimer.Stop();
            }
        }

        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.lbNow.Text = DateTime.Now.ToString("现在时刻:yyyy年MM月dd日 HH:mm:ss");
            this.uploadService = new UploadService(AppConfig.ConnectionString);
            this.uploadService.Processed += UploadService_Processed;
            this.uploadService.Successed += UploadService_Successed;
            this.uploadService.Responsed += UploadService_Responsed;
            new Thread(() =>
            {
                while (true)
                {
                    Thread.Sleep(500);
                    this.setCurrentTime();
                }
            })
            {
                IsBackground = true
            }.Start();
        }

        private void UploadService_Responsed(object sender, ResponseEventArgs e)
        {
            this.setResponse(e.Message);
        }
        private delegate void setResponseDelegate(string message);
        private void setResponse(string message)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new setResponseDelegate(setResponse), message);
            else
            {
                this.textBox1.AppendText(message + Environment.NewLine);
            }
        }

        private void UploadService_Successed(object sender, SuccEventArgs e)
        {
            this.success(e.Duration, e.Count);
        }

        private delegate void successDelegate(TimeSpan duration, int count);
        private void success(TimeSpan duration, int count)
        {
            if (this.InvokeRequired)
            {
                this.BeginInvoke(new successDelegate(success), duration, count);
            }
            else
            {
                string msg = string.Format("成功上传{0}条数据，共计用时:{1}", count, duration);
                this.textBox1.AppendText(msg + Environment.NewLine);
                this.tsProgressBar.Value = 0;
                this.tsProgressLabel.Visible = false;
                this.tsProgressBar.Visible = false;
                this.btnStart.Enabled = true;
            }
        }

        private void UploadService_Processed(object sender, ProgressEventArgs e)
        {
            this.setProgress(e.Progress);
        }
        private delegate void setProgressDelegate(double progress);
        private void setProgress(double progress)
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new setProgressDelegate(setProgress), progress);
            else
            {
                this.tsProgressBar.Value = (int)progress;
                this.tsProgressLabel.Text = string.Format("{0:0.00}%", progress);
            }
        }

        private delegate void setCurrentTimeDelegate();
        private void setCurrentTime()
        {
            if (this.InvokeRequired)
                this.BeginInvoke(new setCurrentTimeDelegate(setCurrentTime));
            else
            {
                DateTime now = DateTime.Now;
                this.lbNow.Text = now.ToString("现在时刻:yyyy年MM月dd日 HH:mm:ss");
            }
        }

        private void kpiTimer_Tick(object sender, EventArgs e)
        {
            DateTime now = DateTime.Now;
            DateTime startTime = this.dateTimePicker1.Value;
            if (startTime.Hour == now.Hour && startTime.Minute == now.Minute && startTime.Second == now.Second&&!this.uploadService.Running)
            {
                this.tsProgressBar.Visible = true;
                this.uploadService.UploadOrder();
                this.btnStart.Enabled = false;
                this.tsProgressLabel.Visible = true;
                this.tsProgressLabel.Text = "0%";
            }
        }
    }
}
