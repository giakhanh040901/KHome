using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.GarnerEntities.Dto.GarnerProductOverview
{
    public class AppProductOverviewFileDto
    {
        /// <summary>
        /// Id của file
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Loại tài liệu
        /// </summary>
        public int DocumentType { get; set; }

        /// <summary>
        /// Tiêu đề
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Đường dẫn
        /// </summary>
        public string Url { get; set; }
    }
}
