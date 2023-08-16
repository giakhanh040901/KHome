using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateEntities.Dto.RstProductItemProjectPolicy
{
    public class RstProductItemProjectPolicyDto
    {
        /// <summary>
        /// Id của productItemProjectPolicy
        /// </summary>
        public int? Id { get; set; }
        /// <summary>
        /// Id của projectPolicy
        /// </summary>
        public int ProjectPolicyId { get; set; }
        /// <summary>
        /// Mã chính sách
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Tên chính sách
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Loại chính sách
        /// </summary>
        public int PolicyType { get; set; }
        /// <summary>
        /// Trạng thái
        /// </summary>
        public string Status { get; set; }
        /// <summary>
        /// Trạng thái của chính sách dự án
        /// </summary>
        public string ProjectPolicyStatus { get; set; }
        public string isProductItemSelected { get; set; }
    }
}
