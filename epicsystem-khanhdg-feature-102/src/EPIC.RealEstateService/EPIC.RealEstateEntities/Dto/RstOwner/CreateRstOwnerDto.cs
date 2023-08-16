using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstOwner
{
    public class CreateRstOwnerDto
    {
        public int BusinessCustomerId { get; set; }
        public decimal? BusinessTurnover { get; set; }
        public decimal? BusinessProfit { get; set; }
        public decimal? Roa { get; set; }
        public decimal? Roe { get; set; }

        private string _website;
        public string Website 
        { 
            get => _website; 
            set => _website = value?.Trim(); 
        }

        private string _hotLine;
        public string Hotline 
        {
            get => _hotLine;
            set => _hotLine = value?.Trim();
        }

        public string _fanpage;
        public string Fanpage 
        {
            get => _fanpage;
            set => _fanpage = value?.Trim();
        }

        /// <summary>
        /// Loại nội dung miêu tả chủ đầu tư : MARKDOWN, HTML
        /// </summary>
        public string _descriptionContentType;
        [StringRange(AllowableValues = new string[] { ContentTypes.MARKDOWN, ContentTypes.HTML }, ErrorMessage = "Vui lòng chọn loại Type : MARKDOWN , HTML")]
        public string DescriptionContentType 
        {
            get => _descriptionContentType;
            set => _descriptionContentType = value?.Trim();
        }

        /// <summary>
        /// Nội dung miêu tả chủ đầu tư
        /// </summary>
        public string _descriptionContent;
        public string DescriptionContent 
        {
            get => _descriptionContent;
            set => _descriptionContent = value?.Trim(); 
        }
    }
}
