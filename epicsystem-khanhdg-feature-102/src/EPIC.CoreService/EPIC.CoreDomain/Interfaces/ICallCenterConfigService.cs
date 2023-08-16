using EPIC.CoreEntities.Dto.CallCenterConfig;
using EPIC.DataAccess.Models;
using System.Collections.Generic;

namespace EPIC.CoreDomain.Interfaces
{
    public interface ICallCenterConfigService
    {
        /// <summary>
        /// Danh sách phân trang cấu hình tài khoản trong call center (cấu hỉnh cho cả root và đại lý)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<CallCenterConfigDto> FindAllConfig(FilterCallCenterConfigDto input);
        /// <summary>
        /// Cập nhật danh sách tài khoản trong call center (cấu hỉnh cho cả root và đại lý)
        /// </summary>
        /// <param name="input"></param>
        void UpdateConfig(UpdateCallCenterConfigDto input);
        /// <summary>
        /// Lấy danh sách user trong call center cho investor hiện tại
        /// </summary>
        /// <returns></returns>
        IEnumerable<UserIdCallCenterDto> GetListUserIdCallCenter();
    }
}
