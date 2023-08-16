using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.AppOrder;
using EPIC.BondEntities.Dto.InterestPayment;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondInterestPaymentService
    {
        void InterestPaymentAdd(InterestPaymentCreateListDto input);
        PagingResult<DanhSachChiTraDto> FindAll(int pageSize, int pageNumber, string keyword, int? status, string phone, string contractCode);
        BondInterestPayment FindById(int id);

        /// <summary>
        /// Đổi trạng thái chi trả, nếu hợp động chọn tái tục vốn thì xử lý tái tục vốn
        /// </summary>
        /// <param name="input"></param>
        Task ChangeEstablishedWithOutPayingToPaidStatus(ChangeStatusDto input);
    }
}
