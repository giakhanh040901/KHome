using EPIC.DataAccess.Models;
using EPIC.EntitiesBase.Dto;
using EPIC.EventEntites.Dto.EvtEvent;
using EPIC.EventEntites.Dto.EvtEventDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    public interface IEvtAppEventService
    {
        /// <summary>
        /// Tìm kiếm sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<AppEvtSearchEventDto> SearchEvent(AppFilterEventDto input);

        /// <summary>
        /// chi tiết sự kiện
        /// </summary>
        /// <param name="id"></param>
        /// <param name="isSaleView"></param>
        /// <returns></returns>
        AppEventDetailsDto FindEventDetailsById(int id, bool isSaleView);

        /// <summary>
        /// event liên quan
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<AppRelatedEventsDto> FindRelatedEventsById(AppFilterEventDto input);

        /// <summary>
        /// Danh sách sự kiện nổi bật
        /// </summary>
        /// <param name="isSaleView"></param>
        /// <returns></returns>
        AppEvtEventDto AppFindHighlightEvent(bool isSaleView);

        /// <summary>
        /// Danh sách sự kiện đang và sắp diễn ra
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<AppViewEventDto> AppFindEvent(AppFilterEvtEventDto input);

        /// <summary>
        /// thêm lịch sử tìm kiếm
        /// </summary>
        /// <param name="eventId"></param>
        void AddSearchHistoryEvent(int eventId);

        /// <summary>
        /// xóa tất cả lịch sử tìm kiếm của investor
        /// </summary>
        void DeleteSearchHistoryEvent();

        /// <summary>
        /// danh sách lịch sử tìm kiếm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<AppEvtSearchEventDto> SearchHistoryEvent(PagingRequestBaseDto input);
    }
}
