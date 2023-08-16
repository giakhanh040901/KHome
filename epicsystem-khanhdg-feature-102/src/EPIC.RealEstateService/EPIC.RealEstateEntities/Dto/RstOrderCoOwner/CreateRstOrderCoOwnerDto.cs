using EPIC.Utils;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOrderCoOwner
{
    public class CreateRstOrderCoOwnerDto
    {
        /// <summary>
        /// Nếu là Giấy tờ của nhà đầu tư trên hệ thông
        /// </summary>
        public int? InvestorIdenId { get; set; }

        #region Thêm người đồng sở hữu
        public string IdFrontImageUrl { get; set; }
        public string IdBackImageUrl { get; set; }

        /// <summary>
        /// Loại giấy tờ
        /// </summary>
        //[StringRange(AllowableValues = new string[] { CardTypesInput.CMND, CardTypesInput.CCCD, CardTypesInput.PASSPORT })]
        public string IdType { get; set; }

        private string _fullname;
        public string Fullname
        {
            get => _fullname;
            set => _fullname = value?.Trim();
        }

        /// <summary>
        /// Số điện thoại liên hệ
        /// </summary>
        private string _phone;
        public string Phone
        {
            get => _phone;
            set => _phone = value?.Trim();
        }

        /// <summary>
        /// Địa chỉ liên hệ
        /// </summary>
        private string _address;
        public string Address
        {
            get => _address;
            set => _address = value?.Trim();
        }

        /// <summary>
        /// Số giấy tờ
        /// </summary>
        private string _idNo;
        public string IdNo
        {
            get => _idNo;
            set => _idNo = value?.Trim();
        }

        /// <summary>
        /// Nguyên quán
        /// </summary>
        private string _placeOfOrigin;
        public string PlaceOfOrigin
        {
            get => _placeOfOrigin;
            set => _placeOfOrigin = value?.Trim();
        }

        public DateTime? DateOfBirth { get; set; }

        public int OrderId { get; set; }
        #endregion
    }
}
