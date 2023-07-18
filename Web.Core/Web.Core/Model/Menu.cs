using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Model
{
    [Table("Menu")]
    public class Menu
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

        [ForeignKey("ParentMenu")]
        public virtual Menu PMenu { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Category> Categories { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
