namespace Web.Watch.Models
{
    public class PaymentInfomation
    {
        public PaymentInfomation() { }
        public PaymentInfomation(string _Amount,
                                string _BankCode,
                                string _BankTranNo,
                                string _CardType,
                                string _OrderInfo,
                                string _PayDate,
                                string _ResponseCode,
                                string _TransactionNo,
                                string _TransactionStatus)
        {
            Amount = _Amount;
            BankCode = _BankCode;
            BankTranNo = _BankTranNo;
            CardType = _CardType;
            OrderInfo = _OrderInfo;
            PayDate = _PayDate;
            ResponseCode = _ResponseCode;
            TransactionStatus = _TransactionStatus;
            TransactionNo = _TransactionNo;
        }
        public string Amount;
        public string BankCode;
        public string BankTranNo;
        public string CardType;
        public string OrderInfo;
        public string PayDate;
        public string ResponseCode;
        public string TransactionNo;
        public string TransactionStatus;
    }
}