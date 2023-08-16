using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.CollectMoney
{
    /// <summary>
    /// Tạo Viet QR
    /// </summary>
    public class CreateVietQrDto
    {
        /// <summary>
        /// Số tài khoản
        /// </summary>
        public string BankAccount { get; set; }

        /// <summary>
        /// Chủ tài khoản
        /// </summary>
        public string OwnerAccount { get; set; }

        /// <summary>
        /// Số tiền
        /// </summary>
        public decimal AmountMoney { get; set; }

        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }
        private string _mId;
        public string MId
        {
            get => _mId;
            set => _mId = value?.Trim();
        }

        private string _tId;
        public string TId
        {
            get => _tId;
            set => _tId = value?.Trim();
        }
        private string _accessCode;
        public string AccessCode
        {
            get => _accessCode;
            set => _accessCode = value?.Trim();
        }
    }
}
