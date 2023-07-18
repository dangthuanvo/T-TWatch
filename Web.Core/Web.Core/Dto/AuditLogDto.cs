using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Dto
{
    public class AuditLogDto
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public string CreateBy { get; set; }
    }
}
