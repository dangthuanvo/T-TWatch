namespace Web.Core.Dto
{
    public class VoucherDto
    {
        public int Id { get; set; }
        public string VoucherCode { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public decimal DiscountAmount { get; set; }
        public int IsActive { get; set; }
    }
}
