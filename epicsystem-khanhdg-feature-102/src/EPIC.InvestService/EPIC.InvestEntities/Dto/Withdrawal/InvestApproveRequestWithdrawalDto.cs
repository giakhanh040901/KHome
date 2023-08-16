using EPIC.GarnerEntities.Dto.GarnerWithdrawal;
using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.Withdrawal
{
    public class InvestApproveRequestWithdrawalDto : PrepareApproveRequestWithdrawalDto
    {
        [IntegerRange(AllowableValues = new int[] { WithdrawalStatus.DUYET_DI_TIEN, WithdrawalStatus.HUY_YEU_CAU, WithdrawalStatus.DUYET_KHONG_DI_TIEN })]
        public int Status { get; set; }

        /// <summary>
        /// Ghi chú duyệt khi không đi tiền
        /// </summary>
        public int? ApproveNote { get; set; }

        private string _otp;
        public string Otp
        {
            get => _otp;
            set => _otp = value?.Trim();
        }

        /// <summary>
        /// Dữ liệu lấy từ bước chuẩn bị
        /// </summary>
        public MsbRequestPaymentWithErrorDto Prepare { get; set; }
    }
}
