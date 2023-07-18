using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Model
{
    [Table("EmailTemplate")]
    public class EmailTemplate
    {
        public int Id { get; set; }
        public string Type { get; set; }
        public string Subject { get; set; }
        public string CC { get; set; }
        public string BCC { get; set; }
        public string KeyGuild { get; set; }
        public string Content { get; set; }

    }
}
