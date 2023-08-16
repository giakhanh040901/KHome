using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.BondInfo;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BondShared;
using EPIC.Entities.Dto.CoreApprove;
using System;
using System.Collections.Generic;

namespace EPIC.BondDomain.Interfaces
{
    public interface IBondProductBondInfoService
    {
        #region cms
        PagingResult<ProductBondInfoDto> FindAll(int pageSize, int pageNumber, string keyword, string status, string isCheck, DateTime? issueDate, DateTime? dueDate);
        ProductBondInfoDto FindById(int id);
        BondInfo Add(CreateProductBondInfoDto input);
        int Update(int id, UpdateProductBondInfoDto input);
        int Delete(int id);
        CouponDto FindCouponById(int id);
        void Request(RequestStatusDto input);
        void Approve(ApproveStatusDto input);
        void Check(CheckStatusDto input);
        void Cancel(CancelStatusDto input);
        void CloseOpen(int id);
        #endregion

        
    }
}
