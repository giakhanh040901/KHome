namespace EPIC.Entities.Dto.Order
{
    /// <summary>
    /// Thông tin tài sản thụ hưởng của đại lý sơ cấp
    /// </summary>
    public class AppPaymentInfoDto
    {
        public int? BusinessCustomerBankId { get; set; }
        public string BankAccName { get; set; }
        public string BankAccNo { get; set; }
        public string BankName { get; set; }
        public string PaymentNote { get; set; }
    }
}
