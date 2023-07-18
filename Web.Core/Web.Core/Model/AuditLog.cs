using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web.Core.Model
{
    [Table("AuditLog")]
    public class AuditLog
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public string Message { get; set; }
        public DateTime Created { get; set; }
        public string CreateBy { get; set; }

    }
}
