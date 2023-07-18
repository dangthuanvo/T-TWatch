using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Model
{
    [Table("Article")]
    public class Article
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
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public bool Active { get; set; }
        public DateTime Created { get; set; }

        public virtual Menu Menu { get; set; }
        public virtual Category Category { get; set; }
    }
}
