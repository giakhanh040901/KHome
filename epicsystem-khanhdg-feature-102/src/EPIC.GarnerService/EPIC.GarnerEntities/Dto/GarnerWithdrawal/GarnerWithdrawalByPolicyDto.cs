using EPIC.CoreSharedEntities.Dto.BankAccount;
using System;
using System.Collections.Generic;

namespace EPIC.GarnerEntities.Dto.GarnerWithdrawal
{
    public class GarnerWithdrawalByPolicyDto
    {
        public long WithdrawalId { get; set; }

        /// <summary>
        /// Nguồn rút: 1 online 2 offline
        /// </summary>
        public int Source { get; set; }

        /// <summary>
        /// Tổng số tiền rút
        /// </summary>
        public decimal AmountMoney { get; set; }

        /// <summary>
        /// Phí rút
        /// </summary>
        public decimal WithdrawalFee { get; set; }

        /// <summary>
        /// Lợi tức rút
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// Tổng tiền thực nhận
        /// </summary>
        public decimal AmountReceived { get; set; }

        /// <summary>
        /// Lợi tức khấu trừ
        /// </summary>
        public decimal DeductibleProfit { get; set; }
        /// <summary>
        /// Thuế lợi nhuận
        /// </summary>
        public decimal Tax { get; set; }
        /// <summary>
        /// Lợi tức thực nhận
        /// </summary>
        public decimal ActuallyProfit { get; set; }

        /// <summary>
        /// Trạng thái
        /// </summary>
        public int Status { get; set; }

        /// <summary>
        /// Ngày tạo
        /// </summary>
        public DateTime? CreatedDate { get; set; }

        /// <summary>
        /// Ngày duyệt
        /// </summary>
        public DateTime? ApproveDate { get; set; }

        /// <summary>
        /// Người hủy duyệt
        /// </summary>
        public string CancelBy { get; set; }

        /// <summary>
        /// Ngày hủy duyệt
        /// </summary>
        public DateTime? CancelDate { get; set; }

        /// <summary>
        /// Ngày sửa    
        /// </summary>
        public DateTime? ModifiedDate { get; set; }

        /// <summary>
        /// Ngày tính rút tiền
        /// </summary>
        public DateTime? WithdrawalDate { get; set; }

        /// <summary>
        /// Chi tiết rút vốn
        /// </summary>
        public List<GarnerOrderWithdrawalDto> WithdrawalDetails { get; set; }

        /// <summary>
        /// Danh sách mã hợp đồng
        /// </summary>
        public List<string> ContractCodes { get; set; }

        /// <summary>
        /// Ngân hàng thụ hưởng của nhà đầu tư
        /// </summary>
        public BankAccountInfoDto InvestorBankAccount { get; set; }
    }
}
