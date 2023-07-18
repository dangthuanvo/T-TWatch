using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Dto
{
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public int OrderId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImage { get; set; }
        public double? ProductPrice { get; set; }
        public double? ProductDiscountPrice { get; set; }
        public double? ProductDiscountPercent { get; set; }
        //public string Attribute { get; set; }
        public int? Qty { get; set; }
        public string Note { get; set; }
        public string UserDef1 { get; set; }
        public string UserDef2 { get; set; }
        public double? UserDef3 { get; set; }
        public bool? UserDef4 { get; set; }
        public DateTime? UserDef5 { get; set; }
    }
}
