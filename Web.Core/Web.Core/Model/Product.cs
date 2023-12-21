using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Core.Model
{
    [Table("Product")]
    public class Product
    {
        public int Id { get; set; }

        public int? MenuId { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
        public int? Index { get; set; }
        public int? Status { get; set; }
        public double? Price { get; set; }
        public double Rate { get; set; }
        public int RateAmount { get; set; }
        public double? DiscountPrice { get; set; }
        public double? DiscountPercent { get; set; }
        public bool? Selling { get; set; }
        public string Tags { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string MetaDescription { get; set; }
        public string MetaRobots { get; set; }
        public string MetaRevisitAfter { get; set; }
        public int? Quantity { get; set; }
        public string MetaContentLanguage { get; set; }
        public string MetaContentType { get; set; }
        public string UserDef1 { get; set; }
        public string UserDef2 { get; set; }
        public double? UserDef3 { get; set; }
        public bool? UserDef4 { get; set; }
        public DateTime? UserDef5 { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Category Category { get; set; }
        public virtual ICollection<ProductAttribute> ProductAttributes { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductRelated> ProductRelateds { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
    }
}
