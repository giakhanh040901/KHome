using EPIC.EventEntites.Dto.EvtEventDescriptionMedia;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.Validation;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventEntites.Dto.EvtEvent
{
    public class CreateEvtEventDto
    {
        /// <summary>
        /// Tên sự kiện
        /// </summary>
        private string _name;
        [Required(ErrorMessage = "Tên sự kiện không được để trống")]
        [MaxLength(256)]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }
        /// <summary>
        /// Ban tổ chức
        /// </summary>
        private string _organizator;
        [Required(ErrorMessage = "Ban tổ chức không được để trống")]
        [MaxLength(128)]
        public string Organizator
        {
            get => _organizator;
            set => _organizator = value?.Trim();
        }
        /// <summary>
        /// Loại sự kiện (Chọn nhiều)
        /// </summary>
        [Required(ErrorMessage = "Loại sự kiện không được bỏ trống")]
        public List<int> EventTypes { get; set; }

        public List<UpdateEvtEventDescriptionMediaDto> EventDescriptionMedias { get; set; }
        
    }
}
