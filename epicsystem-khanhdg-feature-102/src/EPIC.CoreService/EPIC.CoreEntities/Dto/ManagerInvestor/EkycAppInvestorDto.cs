using EPIC.Utils.Validation;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreEntities.Dto.ManagerInvestor
{
    /// <summary>
    /// cms thực hiện ekyc cho tài khoản khách hàng chưa ekyc trên app
    /// </summary>
    public class UpdateAppInvestorIdentificationDto
    {
        private string _fullname { get; set; }
        private string _idNo { get; set; }
        private string _nationality { get; set; }
        private string _personalIdentification { get; set; }
        private string _idIssuer { get; set; }
        private string _placeOrigin { get; set; }
        private string _placeOfResidence { get; set; }
        private string _bankAccount { get; set; }
        private string _ownerAccount { get; set; }
        private string _address { get; set; }


        [Required(ErrorMessage = "Loại giấy tờ không được bỏ trống")]

        [StringRange(AllowableValues = new string[] { IDTypes.CCCD, IDTypes.CMND, IDTypes.PASSPORT })]
        public string IdType { get; set; }

        [Required(ErrorMessage = "Mã số không được để trống")]
        public string IdNo { get => _idNo; set => _idNo = value?.Trim(); }

        [Required(ErrorMessage = "Họ tên không được để trống")]
        public string Fullname { get => _fullname; set => _fullname = value?.Trim(); }

        [Required(ErrorMessage = "Ngày sinh không được để trống")]
        public DateTime? DateOfBirth { get; set; }

        [Required(ErrorMessage = "Quốc tịch không được để trống")]
        public string Nationality { get => _nationality; set => _nationality = value?.Trim(); }

        public string PersonalIdentification { get => _personalIdentification; set => _personalIdentification = value?.Trim(); }
        [Required(ErrorMessage = "Nơi cấp không được để trống")]
        public string IdIssuer { get => _idIssuer; set => _idIssuer = value?.Trim(); }

        [Required(ErrorMessage = "Ngày cấp không được để trống")]
        public DateTime? IdDate { get; set; }

        [Required(ErrorMessage = "Ngày hết hạn không được để trống")]
        [DateFuture(ErrorMessage = "Giấy tờ đã hết hạn")]
        public DateTime? IdExpiredDate { get; set; }

        public string PlaceOfOrigin { get => _placeOrigin; set => _placeOrigin = value?.Trim(); }
        [Required(ErrorMessage = "Địa chỉ thường trú không được để trống")]
        public string PlaceOfResidence { get => _placeOfResidence; set => _placeOfResidence = value?.Trim(); }

        public string Sex { get; set; }

        public string IdFrontImageUrl { get; set; }

        public string IdBackImageUrl { get; set; }

        public int BankId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Số tài khoản không được để trống")]
        public string BankAccount { get => _bankAccount; set => _bankAccount = value?.Trim(); }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Tên chủ tài khoản không được để trống")]
        public string OwnerAccount { get => _ownerAccount; set => _ownerAccount = value?.Trim(); }

        public string Address { get => _address; set => _address = value?.Trim(); }
        public int InvestorId { get; set; }
    }
}
