using EPIC.CoreEntities.DataEntities;
using EPIC.CoreEntities.Dto.PartnerMsbPrefixAccount;
using EPIC.DataAccess.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.CoreDomain.Interfaces
{
    public interface IPartnerMsbPrefixAccountServices
    {
        /// <summary>
        /// Thêm tài khoản đối tác
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PartnerMsbPrefixAccount Add(CreatePartnerMsbPrefixAccountDto input);
        /// <summary>
        /// Xoá tài khoản
        /// </summary>
        /// <param name="id"></param>
        void Delete(int id);
        /// <summary>
        /// Danh sách tài khoản
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        PagingResult<PartnerMsbPrefixAccountDto> FindAll(FilterPartnerMsbPrefixAccountDto input);
        /// <summary>
        /// Tìm tài khoản theo id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        PartnerMsbPrefixAccount FindById(int id);
        /// <summary>
        /// Tìm tài khoản theo tài khoản ngân hàng của đối tác
        /// </summary>
        /// <param name="partnerBankAccId"></param>
        /// <returns></returns>
        PartnerMsbPrefixAccount FindByPartnerBankId(int partnerBankAccId);
        /// <summary>
        /// Cập nhật
        /// </summary>
        /// <param name="input"></param>
        void Update(UpdatePartnerMsbPrefixAccountDto input);
    }
}
