using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Dto
{
    public class EmailTemplateDto
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
