using System;

namespace Web.Core.Dto
{
    public class ImportHistoryDto
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
