using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtEventDetail;
using EPIC.EventEntites.Dto.EvtHistoryUpdate;
using EPIC.RealEstateEntities.Dto.RstProject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    /// <summary>
    /// Sự kiện
    /// </summary>
    public interface IEvtManagerEventService
    {
        /// <summary>
        /// Thêm sự kiện 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EvtEventDto Add(CreateEvtEventDto input);
        /// <summary>
        /// Cập nhật sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EvtEventDto Update(UpdateEvtEventDto input);
        /// <summary>
        /// Xóa sự kiện
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// Danh sách sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EvtEventDto> FindAll(FilterEvtEventDto input);
        /// <summary>
        /// Cập nhật trạng thái sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        void UpdateStatus (int id, int status);

        /// <summary>
        /// Tạm dừng sự kiện và lý do tạm dừng
        /// </summary>
        /// <param name="input"></param>
        Task PauseEvent(CreateHistoryUpdateDto input);
        /// <summary>
        /// Hủy sự kiện và lý do hủy
        /// </summary>
        /// <param name="input"></param>
        void CancelEvent(CreateHistoryUpdateDto input);
        /// <summary>
        /// Bật/tắt show app sự kiện
        /// </summary>
        /// <param name="id"></param>
        void ChangeIsShowApp(int id);
        /// <summary>
        /// Cập nhật mô tả sự kiện
        /// </summary>
        /// <param name="input"></param>
        void UpdateEventOverviewContent(UpdateEventOverviewContentDto input);
        EvtEventDto FindById(int id);
        IEnumerable<EvtEventDto> FindEventActive(bool? isApp = null);
        /// <summary>
        /// Thay đổi trạng thái check của IsCheck
        /// </summary>
        /// <param name="id"></param>
        void ChangeIsCheck(int id);

        IEnumerable<ViewEventByTradingDto> FindEventSellBehalfByTrading();
    }
}
