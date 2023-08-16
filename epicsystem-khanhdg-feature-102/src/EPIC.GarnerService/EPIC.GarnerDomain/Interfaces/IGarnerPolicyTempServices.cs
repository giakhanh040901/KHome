using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerPolicyTemp;
using System.Collections.Generic;

namespace EPIC.GarnerDomain.Interfaces
{
    public interface IGarnerPolicyTempServices
    {
        /// <summary>
        /// Thêm chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GarnerPolicyTemp Add(CreateGarnerPolicyTempDto input);

        /// <summary>
        /// Xóa chính sách mẫu
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);

        /// <summary>
        /// Danh sách chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<GarnerPolicyTempDto> FindAll(FilterPolicyTempDto input);

        /// <summary>
        /// Lấy chính sách mẫu theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        GarnerPolicyTempDto FindById(int id);

        /// <summary>
        /// Cập nhật chính sách mẫu
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        GarnerPolicyTemp Update(UpdateGarnerPolicyTempDto input);

        /// <summary>
        /// Danh sách chính sách mẫu
        /// </summary>
        /// <returns></returns>
        List<GarnerPolicyTempDto> FindAllNoPermission(string status);
        /// <summary>
        /// Hủy kích hoạt chính sách mẫu
        /// </summary>
        /// <returns></returns>
        void ChangeStatusPolicyTemp(int policyTempId);
    }
}
