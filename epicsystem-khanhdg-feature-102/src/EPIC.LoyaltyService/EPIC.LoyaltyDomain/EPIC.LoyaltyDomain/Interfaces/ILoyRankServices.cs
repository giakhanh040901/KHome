using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.LoyaltyEntities.Dto.LoyRank;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.LoyaltyDomain.Interfaces
{
    public interface ILoyRankServices
    {
        /// <summary>
        /// Thêm config xếp hạng
        /// </summary>
        /// <param name="dto"></param>
        public void Add(AddRankDto dto);
        /// <summary>
        /// Cập nhật trạng thái
        /// </summary>
        /// <param name="dto"></param>
        public void UpdateStatus(UpdateStatusDto dto);
        /// <summary>
        /// Xoá mềm rank
        /// </summary>
        /// <param name="id"></param>
        public void DeleteRank(int id);
        /// <summary>
        /// Cập nhật rank 
        /// </summary>
        /// <param name="dto"></param>
        public void Update(UpdateRankDto dto);
        /// <summary>
        /// Lấy theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public ViewFindRankDto FindById(int id);
        /// <summary>
        /// Phân trang tìm kiếm cấu hình xếp hạng
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public PagingResult<ViewFindRankDto> FindAll(FindAllRankDto dto);

        /// <summary>
        /// App xem điểm và hạng
        /// </summary>
        /// <returns></returns>
        public AppViewRankPointDto FindByInvestorId(int? tradingProviderId);

    }
}
