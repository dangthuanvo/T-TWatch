using System;

namespace Web.Core.Dto
{
    public class ReviewDto
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Star { get; set; }
        public string Content { get; set; }
        public string CustomerCode { get; set; }
        public int Active { get; set; }
        public string CustomerName { get; set; }
        public DateTime Created { get; set; }
        public ProductDto Product { get; set; }
        public CustomerDto Customer { get; set; }

    }
}
