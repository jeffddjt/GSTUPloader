using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace GST.KPI.Repositories.Models
{
    [Table("KPIOrder")]
    public class KPIOrder
    {
        public int ID { get; set; }
        public string No { get; set; }
        public string Customer_No { get; set; }
        public string Sales_Staff { get; set; }
        public string Sales_Staff_Name { get; set; }
        public string Customer_Name { get; set; }
        public DateTime Sales_Order_Create_Date { get; set; }
        public decimal Sales_Order_Sales_Amount { get; set; }
        public string Sales_Branch_Office_Name { get; set; }
        public decimal Sales_Order_Paid_Amount { get; set; }
        public decimal Sales_Order_Shipped_Amount { get; set; }
    }
}
