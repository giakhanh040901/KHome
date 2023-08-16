using EPIC.InvestEntities.Dto.ProjectInformationShare;
using EPIC.InvestEntities.Dto.ProjectOverViewFile;
using EPIC.InvestEntities.Dto.ProjectOverviewOrg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.InvestEntities.Dto.InvestProject
{
    public class AppOverViewProjectDto
    {
        public string OverviewImageUrl { get; set; }
        public string ContentType { get; set; }
        public string OverviewContent { get; set; }
        
        /// <summary>
        /// Tổng số người tham gia tích lũy (hợp đồng active)
        /// </summary>
        public int TotalParticipants { get; set; }
        /// <summary>
        /// Tỷ lệ đánh giá
        /// </summary>
        public decimal RatingRate { get; set; }

        /// <summary>
        /// Tổng số người tham giá đánh giá
        /// </summary>
        public int TotalReviewers { get; set; }
        public List<ProjectOverviewFileDto> ProjectOverviewFiles { get; set; }
        public List<ProjectOverviewOrgDto> ProjectOverviewOrgs { get; set; }
        public List<AppInvProjectInformationShareDto> ProjectInformationShare { get; set; }
    }
}
