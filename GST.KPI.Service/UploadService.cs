using GST.KPI.Repositories.DAO;
using GST.KPI.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST.KPI.Service
{
    public class UploadService
    {
        private readonly string connStr;

        public UploadService(string connStr)
        {
            this.connStr = connStr;
        }

        public IList<KPIOrder> GetOrderList()
        {
            using(GstDbContext context=new GstDbContext(this.connStr))
            {
                IList<KPIOrder> list = context.KPIOrder.Take(20).ToList();
                return list;
            }
        }
    }
}
