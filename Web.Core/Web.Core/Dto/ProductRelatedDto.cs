using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Dto
{
    public class ProductRelatedDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int ProductRelatedId { get; set; }
        public int? Index { get; set; }

        public ProductDto ProductR { get; set; }
    }
}
