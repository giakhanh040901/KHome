using EPIC.Utils;
using EPIC.Utils.Validation;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.Investor
{
    public class EKYCOcrDto : AddIdentificationDto
    {
        private string _phone;

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số điện thoại không được bỏ trống")]
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }
    }

    public class EKYCOcrResultDto
    {
        /// <summary>
        /// Họ tên
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Ngày sinh
        /// </summary>
        public DateTime? DateOfBirth { get; set; }
        /// <summary>
        /// Giới tính
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        /// số id: số cmnd, số cccd, passport number
        /// </summary>
        public string IdNo { get; set; }
        /// <summary>
        /// Ngày cấp
        /// </summary>
        public DateTime? IdIssueDate { get; set; }
        /// <summary>
        /// Ngày hết hạn
        /// </summary>
        public DateTime? IdIssueExpDate { get; set; }
        /// <summary>
        /// Nơi cấp, đơn vị cấp
        /// </summary>
        public string IdIssuer { get; set; }
        /// <summary>
        /// Quốc tịch
        /// </summary>
        public string Nationality { get; set; }
        /// <summary>
        /// Quê quán, nơi sinh (place of birth)
        /// </summary>
        public string PlaceOfOrigin { get; set; }
        /// <summary>
        /// Nơi ở thường trú
        /// </summary>
        public string PlaceOfResidence { get; set; }
        /// <summary>
        /// Passport id number
        /// </summary>
        public string PassportIdNumber { get; set; }
        /// <summary>
        /// Id của giấy tờ
        /// </summary>
        public int IdentificationId { get; set; }
    }
}
