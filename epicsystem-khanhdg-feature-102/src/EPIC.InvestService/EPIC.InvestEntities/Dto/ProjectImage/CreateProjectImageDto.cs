using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.ProjectImage
{
    public class CreateProjectImageDto
    {
        public int ProjectId { get; set; }
        public List<string> ProjectImages { get; set; }
    }

    public class ProjectImageDto
    {
        public string Title { get; set; }

        [Required(ErrorMessage = "Đường dẫn hình ảnh dự án đầu tư không được bỏ trống")]
        public string Url { get; set; }
    }
}
