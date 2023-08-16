using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.MSB.Dto.PayMoney
{
    /// <summary>
    /// Yêu cầu chi
    /// </summary>
    public class MsbRequestPayMoneyDto
    {
        /// <summary>
        /// Request id
        /// </summary>
        public long RequestId { get; set; }
        /// <summary>
        /// Tiền tố
        /// </summary>
        public string PrefixAccount { get; set; }
        /// <summary>
        /// Chi tiết yêu cầu
        /// </summary>
        public List<MsbRequestPayMoneyItemDto> Details { get; set; }

        private string _tId;
        public string TId
        {
            get => _tId;
            set => _tId = value != null ? value.Trim() : throw new ArgumentException(nameof(TId));
        }

        private string _mId;
        public string MId
        {
            get => _mId;
            set => _mId = value != null ? value.Trim() : throw new ArgumentException(nameof(MId));
        }

        private string _accessCode;
        public string AccessCode
        {
            get => _accessCode;
            set => _accessCode = value != null ? value.Trim() : throw new ArgumentException(nameof(AccessCode));
        }
    }

    /// <summary>
    /// Chi tiết yêu cầu chi
    /// </summary>
    public class MsbRequestPayMoneyItemDto
    {
        /// <summary>
        /// Id chi tiết
        /// </summary>
        public long DetailId { get; set; }
        /// <summary>
        /// Ghi chú
        /// </summary>
        public string Note { get; set; }
        /// <summary>
        /// Tên chủ tài khoản
        /// </summary>
        [Required]
        public string OwnerAccount { get; set; }
        /// <summary>
        /// Số tài khoản
        /// </summary>
        [Required]
        public string BankAccount { get; set; }
        /// <summary>
        /// Mã bin
        /// </summary>
        [Required]
        public string ReceiveBankBin { get; set; }
        /// <summary>
        /// Số tiền chi
        /// </summary>
        public decimal AmountMoney { get; set; }
    }
}
