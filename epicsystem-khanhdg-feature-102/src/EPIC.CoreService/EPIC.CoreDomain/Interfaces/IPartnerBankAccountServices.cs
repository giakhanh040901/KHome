using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.PartnerBankAccount;
using EPIC.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IPartnerBankAccountServices
    {
        /// <summary>
        /// Thêm tài khoản ngân hàng của đối tác
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PartnerBankAccount Add(CreatePartnerBankAccountDto input);
        /// <summary>
        /// Đổi trạng thái tài khoản ngân hàng của đối tác (1: kích hoạt, 2: không kích hoạt)
        /// </summary>
        /// <param name="id"></param>
        /// <param name="status"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        PartnerBankAccount ChangeStatus(int id, int? partnerId);
        /// <summary>
        /// Xoá tài khoản
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        void Delete(int id, int? partnerId);
        /// <summary>
        /// Danh sách tài khoản
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<PartnerBankAccountDto> FindAll(FilterPartnerBankAccountDto input);
        PartnerBankAccount FindById(int id, int? partnerId);
        /// <summary>
        /// Set tài khoản ngân hàng mặc định
        /// </summary>
        /// <param name="id"></param>
        /// <param name="partnerId"></param>
        /// <returns></returns>
        PartnerBankAccount SetDefault(int id, int? partnerId);
        /// <summary>
        /// Cập nhật tài khoản mặc định
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PartnerBankAccount Update(UpdatePartnerBankAccountDto input);
    }
}
