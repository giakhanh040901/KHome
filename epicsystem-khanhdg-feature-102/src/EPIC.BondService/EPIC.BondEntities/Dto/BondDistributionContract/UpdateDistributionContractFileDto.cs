using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.Entities.Dto.DistributionContract
{
    public class UpdateDistributionContractFileDto
    {
        public decimal FileId { get; set; }

        private string _title;
        [StringLength(512, ErrorMessage = "Tên file không được dài hơn 512 ký tự")]
        public string Title
        {
            get => _title;
            set => _title = value?.Trim();
        }

        private string _fileUrl;
        public string FileUrl
        {
            get => _fileUrl;
            set => _fileUrl = value?.Trim();
        }
    }
}
