using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Model
{
    [Table("ProductAttribute")]
    public class ProductAttribute
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public string ValueString { get; set; }
        public double? ValueNumber { get; set; }
        public int? Index { get; set; }
        public string UserDef1 { get; set; }
        public string UserDef2 { get; set; }
        public double? UserDef3 { get; set; }
        public bool? UserDef4 { get; set; }
        public DateTime? UserDef5 { get; set; }

        public virtual Product Product { get; set; }
        public virtual Attribute Attribute { get; set; }
    }
}
