using System.Collections.Generic;

namespace Web.Core.Dto
{
    public class OrderDetailVoucherDto
    {
        public List<OrderDetailDto> OrderDetails;
        public List<VoucherDto> Vouchers;
    }
}
