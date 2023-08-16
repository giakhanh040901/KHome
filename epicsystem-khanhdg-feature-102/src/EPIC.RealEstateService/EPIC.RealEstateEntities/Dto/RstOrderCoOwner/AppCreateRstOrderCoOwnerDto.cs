using EPIC.Utils.Validation;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using EPIC.Utils.Attributes;

namespace EPIC.RealEstateEntities.Dto.RstOrderCoOwner
{
    public class AppCreateRstOrderCoOwnerDto
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
        [StringRange(AllowableValues = new string[] { CardTypesInput.CMND, CardTypesInput.CCCD, CardTypesInput.PASSPORT })]
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
        #endregion
    }
}
