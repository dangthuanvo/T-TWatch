using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Core.Model
{
    [Table("Review")]
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int Star { get; set; }
        public string Content { get; set; }
        public string CustomerCode { get; set; }
        public int Active { get; set; }
        public string CustomerName { get; set; }
        public DateTime Created { get; set; }
        public virtual Product Product { get; set; }
        public virtual Customer Customer { get; set; }
    }
}
