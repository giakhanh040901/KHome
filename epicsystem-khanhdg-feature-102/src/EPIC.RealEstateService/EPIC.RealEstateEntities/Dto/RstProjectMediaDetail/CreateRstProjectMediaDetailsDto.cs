using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectMediaDetail
{
    public class CreateRstProjectMediaDetailsDto
    {
        public int ProjectId { get; set; }

        private string _groupTitle;
        [StringLength(256, ErrorMessage = "Tên nhóm không được dài hơn 256 ký tự")]
        public string GroupTitle
        {
            get => _groupTitle;
            set => _groupTitle = value?.Trim();
        }

        public List<CreateRstProjectMediaDetailDto> RstProjectMediaDetails { get; set; }
    }
}
