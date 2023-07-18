using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Model
{
    [Table("ProductRelated")]
    public class ProductRelated
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductRelatedId { get; set; }
        public int? Index { get; set; }

        public virtual Product Product { get; set; }
    }
}
