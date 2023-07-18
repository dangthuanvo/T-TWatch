using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace Web.Core.Dto
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public int? MenuId { get; set; }
        public int? CategoryId { get; set; }
        public string Type { get; set; }
        public string Title { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
        public int? Index { get; set; }
        public int Status { get; set; }
        [AllowHtml]
        public string ShortDescription { get; set; }
        [AllowHtml]
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }

        public MenuDto Menu { get; set; }
        public CategoryDto Category { get; set; }
    }
}
