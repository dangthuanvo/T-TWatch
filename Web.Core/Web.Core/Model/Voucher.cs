using System.ComponentModel.DataAnnotations.Schema;

namespace Web.Core.Model
{
    [Table("Voucher")]
    public class Voucher
    {
        public int Id { get; set; }
        public string VoucherCode { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public decimal DiscountAmount { get; set; }
        public int IsActive { get; set; }

    }
}
