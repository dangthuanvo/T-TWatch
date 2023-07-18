using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int? Star { get; set; }
        public string Content { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }
        public string CreatedBy { get; set; }
        public string UserDef1 { get; set; }
        public string UserDef2 { get; set; }
        public double? UserDef3 { get; set; }
        public bool? UserDef4 { get; set; }
        public DateTime? UserDef5 { get; set; }

        public ProductDto Product { get; set; }
    }
}
