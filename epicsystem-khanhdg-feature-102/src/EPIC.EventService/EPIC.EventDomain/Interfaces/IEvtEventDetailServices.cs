using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtEventDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtEventDetailServices
    {
        #region Khung giờ
        /// <summary>
        /// Thêm khung giờ khung giờ sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EvtEventDetailDto AddEventDetail(CreateEvtEventDetailDto input);
        /// <summary>
        /// Cập nhật khung giờ sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EvtEventDetailDto UpdateEventDetail(UpdateEvtEventDetailDto input);
        /// <summary>
        /// Xóa khung giờ sự kiện
        /// </summary>
        /// <param name="id"></param>
        void DeleteEventDetail(int id);
        /// <summary>
        /// Danh sách khung giờ sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EvtEventDetailDto> FindAllEventDetail(FilterEvtEventDetailDto input);
        /// <summary>
        /// Thay đổi trạng thái khung giờ
        /// </summary>
        /// <param name="id"></param>
        void ChangeStatusEventDetail(int id);
        /// <summary>
        /// Find khung giờ theo Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EvtEventDetailDto FindDetailById(int id);
        /// <summary>
        /// Lấy khung giờ và vé đang kích hoạt cho đặt lệnh
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        IEnumerable<EvtEventDetailDto> FindDetailActiveById(int id, bool? isApp = null);
        #endregion

    }
}
