using EPIC.PaymentEntities.Dto.MsbRequestPayment;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InterestPayment
{
    public class ApproveInterestPaymentRenewalsOrderDto : PrepareApproveRequestInterestPaymentDto
    {
        [IntegerRange(AllowableValues = new int[] { InterestPaymentStatus.DA_DUYET_CHI_TIEN, InterestPaymentStatus.DA_DUYET_KHONG_CHI_TIEN, InterestPaymentStatus.HUY_DUYET })]
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
