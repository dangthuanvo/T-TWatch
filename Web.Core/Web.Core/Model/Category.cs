using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Model
{
    [Table("Category")]
    public class Category
    {
        public int Id { get; set; }
        public int? MenuId { get; set; }
        public string Name { get; set; }
        public string Alias { get; set; }
        public string Image { get; set; }
        public int Index { get; set; }
        public bool Active { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual ICollection<Article> Articles { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
