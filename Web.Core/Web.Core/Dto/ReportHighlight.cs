using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Dto
{
    public class ReportHighlight
    {
        public int TotalNewOrder { get; set; }
        public double DailySales { get; set; }
        public int TotalOrder { get; set; }
        public double SalesRevenue { get; set; }

        public List<OrderDto> TenOrderLastest { get; set; }

        public List<double> Revenues { get; set; }
    }
}
