using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace GST.KPI.Utils
{
    public static class KPIExtesion
    {
        public static string ToText(this KPIStatus status)
        {
            switch (status)
            {
                case KPIStatus.Running:
                    return "运行";
                case KPIStatus.Stoped:
                    return "停止";
                default:
                    return "未知";
            }
        }
    }
}
