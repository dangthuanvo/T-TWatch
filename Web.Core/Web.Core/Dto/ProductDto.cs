using System;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Web.Core.Dto
{
    public class ProductDto
    {
        public int Id { get; set; }

        public int? MenuId { get; set; }
        public int? CategoryId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
        public int? Quantity { get; set; }
        public double Rate { get; set; }
        public int RateAmount { get; set; }
        public int? Index { get; set; }
        public int? Status { get; set; }
        public double? Price { get; set; }
        public double? DiscountPrice { get; set; }
        public double? DiscountPercent { get; set; }
        public bool? Selling { get; set; }
        public string Tags { get; set; }
        [AllowHtml]
        public string ShortDescription { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public DateTime Created { get; set; }
        public string MetaDescription { get; set; }
        public string MetaRobots { get; set; }
        public string MetaRevisitAfter { get; set; }
        public string MetaContentLanguage { get; set; }
        public string MetaContentType { get; set; }
        [AllowHtml]
        public string UserDef1 { get; set; }
        public string UserDef2 { get; set; }
        public double? UserDef3 { get; set; }
        public bool? UserDef4 { get; set; }
        public DateTime? UserDef5 { get; set; }

        public MenuDto Menu { get; set; }
        public CategoryDto Category { get; set; }
        public List<ProductAttributeDto> ProductAttributes { get; set; }
        public List<ProductImageDto> ProductImages { get; set; }
        public List<ProductRelatedDto> ProductRelateds { get; set; }
        public List<ReviewDto> Reviews { get; set; }
    }
}
