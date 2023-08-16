using EPIC.DataAccess.Models;
using EPIC.EventEntites.Dto.EvtTicket;
using EPIC.EventEntites.Dto.EvtTicketMedia;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.EventDomain.Interfaces
{
    /// <summary>
    /// Vé của một khung giờ trong sự kiện
    /// </summary>
    public interface IEvtTicketService
    {
        /// <summary>
        /// Thêm loại vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EvtTicketDto Add(CreateEvtTicketDto input);
        /// <summary>
        /// Cập nhật thông tin loại vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        EvtTicketDto Update(UpdateEvtTicketDto input);
        /// <summary>
        /// Xóa loại vé
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// Danh sách loại vé của khung giờ trong sự kiện
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<EvtTicketDto> FindAll(FilterEvtTicketDto input);
        /// <summary>
        /// Nhân bản vé
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        IEnumerable<EvtTicketDto> ReplicateTicket(CreateListRepicateTicketDto input);
        /// <summary>
        /// Xóa ảnh của vé
        /// </summary>
        /// <param name="id"></param>
        void DeleteTicketImage(int id);
        /// <summary>
        /// Cập nhật trạng thái của vé
        /// </summary>
        /// <param name="id"></param>
        void UpdateStatus(int id);
        /// <summary>
        /// Cập nhật trạng thái show app của vé
        /// </summary>
        /// <param name="id"></param>
        void ChangeShowApp(int id);
        /// <summary>
        /// Lấy thông tin của vé
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        EvtTicketDto FindById (int id);
        /// <summary>
        /// Update đường dẫn ảnh
        /// </summary>
        /// <param name="input"></param>
        void UpdateTicketImage(UpdateTicketImageDto input);
    }
}
