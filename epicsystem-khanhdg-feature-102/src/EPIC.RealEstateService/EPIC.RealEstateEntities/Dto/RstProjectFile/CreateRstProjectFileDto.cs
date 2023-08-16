using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProjectFile
{
    public class CreateRstProjectFileDto
    {
        public int ProjectId { get; set; }

        private string _name;
        [Required(ErrorMessage = "Mô tả không được để trống")]
        [StringLength(256, ErrorMessage = "Mô tả không được dài hơn 256 ký tự")]
        public string Name
        {
            get => _name;
            set => _name = value?.Trim();
        }

        private string _url;
        [Required(ErrorMessage = "Đường dẫn file không được để trống")]
        [StringLength(512, ErrorMessage = "Đường dẫn file không được dài hơn 512 ký tự")]
        public string Url
        {
            get => _url;
            set => _url = value?.Trim();
        }
    }
}
