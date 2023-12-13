using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Core.Model
{
    [Table("ImportHistory")]
    public class ImportHistory
    {
        public int Id { get; set; }
        public int ProductID { get; set; }
        public DateTime ImportDate { get; set; }
        public string ImportedBy { get; set; }
        public int Quantity { get; set; }
        public int Price { get; set; }
        public string Note { get; set; }

    }
}
