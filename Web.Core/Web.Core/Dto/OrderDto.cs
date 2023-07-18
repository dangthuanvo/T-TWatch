using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Dto
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string CustomerCode { get; set; }
        public DateTime OrderDate { get; set; }
        public string OrderTime { get; set; }
        public double? TotalAmount { get; set; }
        public int Status { get; set; }
        public string PaymentMethod { get; set; }
        public string Note { get; set; }
        public DateTime Created { get; set; }

        public CustomerDto Customer { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
    }
}
