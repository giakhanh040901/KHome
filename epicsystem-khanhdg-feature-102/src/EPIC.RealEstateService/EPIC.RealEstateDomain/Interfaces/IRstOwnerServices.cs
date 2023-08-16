using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstOwner;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.RealEstateDomain.Interfaces
{
    public interface IRstOwnerServices
    {
        /// <summary>
        /// Thêm chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        RstOwner Add(CreateRstOwnerDto input);

        /// <summary>
        /// Thay đổi trạng thái chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        RstOwner ChangeStatus(int id, string status);

        /// <summary>
        /// Xoá chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Tìm danh sách chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<ViewRstOwnerDto> FindAll(FilterRstOwnerDto input);

        /// <summary>
        /// Xem danh sách chủ đầu tư theo đối tác
        /// </summary>
        /// <returns></returns>
        List<ViewRstOwnerDto> GetAllOwnerByPartner();
        /// <summary>
        /// Xem danh sách chủ đầu tư theo đại lý
        /// </summary>
        /// <returns></returns>
        List<ViewRstOwnerDto> GetAllOwnerByTrading();

        /// <summary>
        /// Tìm chủ đầu tư
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ViewRstOwnerDto FindById(int id);

        /// <summary>
        /// Cập nhật chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        RstOwner Update(UpdateRstOwnerDto input);

        /// <summary>
        /// Cập nhật nội dung miêu tả chủ đầu tư
        /// </summary>
        /// <param name="input"></param>
        void UpdateOwnerDescriptionContent(UpdateRstOwnerDescriptionDto input);
    }
}
