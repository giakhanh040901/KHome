using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.SaleInvestor
{
    public class SaleInvestorConfirmUpdateDto
    {
        private string _name { get; set; }
        private string _idNo { get; set; }
        private string _idIssuer { get; set; }
        private string _placeOfOrigin { get; set; }
        private string _placeOfResidence { get; set; }
        private string _nationality { get; set; }

        public int InvestorId { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Họ tên không được bỏ trống")]
        public string Name { get => _name; set => _name = value?.Trim(); }

        public DateTime BirthDate { get; set; }

        [StringRange(AllowableValues = new string[] { null, Genders.FEMALE, Genders.MALE })]
        public string Sex { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Mã số không được bỏ trống")]
        public string IdNo { get => _idNo; set => _idNo = value?.Trim(); }

        public DateTime IdDate { get; set; }

        public DateTime? IdExpiredDate { get; set; }

        public string IdIssuer { get => _idIssuer; set => _idIssuer = value?.Trim(); }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Quê quán không được bỏ trống")]
        public string PlaceOfOrigin { get => _placeOfOrigin; set => _placeOfOrigin = value?.Trim(); }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Nơi thường trú không được bỏ trống")]
        public string PlaceOfResidence { get => _placeOfResidence; set => _placeOfResidence = value?.Trim(); }

        public string Nationality { get => _nationality; set => _nationality = value?.Trim(); }
    }
}
