using GST.KPI.Repositories.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace GST.KPI.Repositories.DAO
{
    public class GstDbContext:DbContext
    {
        public GstDbContext(string connStr):base(connStr)
        {
            
        }

        public DbSet<KPIOrder> KPIOrder { get; set; }
    }
}
