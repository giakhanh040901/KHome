using EPIC.Utils.Validation;
using EPIC.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.User
{
    public class CreateUserByRootDto : CreateUserDto
    {
        private string _avatarImageUrl;
        /// <summary>
        /// Ảnh đại diện của user
        /// </summary>
        public string AvatarImageUrl 
        { 
            get => _avatarImageUrl; 
            set => _avatarImageUrl = value?.Trim(); 
        }
        public int? TradingProviderId { get; set; }
        public int? PartnerId { get; set; }
        public int InvestorId { get; set; }
        [StringRange(AllowableValues = new string[] { YesNo.YES, YesNo.NO }, ErrorMessage = "Chỉ được nhập trong các giá trị Y / N")]
        public string IsTempPassword { get; set; }
    }
}
