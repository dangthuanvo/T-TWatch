using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Dto
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public int? MenuId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
        public int Index { get; set; }
        public bool Active { get; set; }

        public MenuDto Menu { get; set; }
        public List<ArticleDto> Articles { get; set; }
        public List<ProductDto> Products { get; set; }
    }
}
