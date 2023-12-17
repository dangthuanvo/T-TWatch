using System.Collections.Generic;

namespace Web.Core.Dto
{
    public class ProductReviewDto
    {
        public ProductDto Product { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
