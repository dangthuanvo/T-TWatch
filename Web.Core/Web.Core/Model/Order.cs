using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Model
{
    [Table("Order")]
    public class Order
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

        public virtual Customer Customer { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
