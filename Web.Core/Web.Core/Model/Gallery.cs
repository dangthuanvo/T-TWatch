using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Model
{
    [Table("Gallery")]
    public class Gallery
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Image { get; set; }
        public int? Index { get; set; }
        public bool? Active { get; set; }

    }
}
