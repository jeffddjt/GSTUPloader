using GST.KPI.Service;
using GST.KPI.Utils;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace GST.KPI.Uploader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            UploadService service = new UploadService(AppConfig.ConnectionString);
            this.dataGridView1.DataSource = service.GetOrderList();
        }
    }
}
