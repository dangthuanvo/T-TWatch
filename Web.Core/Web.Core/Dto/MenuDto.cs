using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Dto
{
    public class MenuDto
    {
        public int Id { get; set; }
        public int? ParentMenu { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
        public int? Index { get; set; }
        public bool? ShowHomePage { get; set; }
        public string ViewType { get; set; }
        public bool Active { get; set; }
        public string MetaDescription { get; set; }
        public string MetaRobots { get; set; }
        public string MetaRevisitAfter { get; set; }
        public string MetaContentLanguage { get; set; }
        public string MetaContentType { get; set; }

        public MenuDto PMenu { get; set; }
        public List<MenuDto> SubMenus { get; set; }

        public List<ProductDto> Products { get; set; }
    }
}
