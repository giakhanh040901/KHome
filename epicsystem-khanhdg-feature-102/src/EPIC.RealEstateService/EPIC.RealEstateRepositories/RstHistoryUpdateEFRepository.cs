using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.RealEstateEntities.DataEntities;
using EPIC.RealEstateEntities.Dto.RstHistoryUpdate;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace EPIC.RealEstateRepositories
{
    public class RstHistoryUpdateEFRepository : BaseEFRepository<RstHistoryUpdate>
    {
        public RstHistoryUpdateEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_REAL_ESTATE}.{RstHistoryUpdate.SEQ}")
        {
        }

        /// <summary>
        /// Thêm lịch sử thao tác dữ liệu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        public void Add(RstHistoryUpdate input, string username)
        {
            _logger.LogInformation($"{nameof(RstHistoryUpdateEFRepository)}->{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");
            input.Id = (int)NextKey();
            input.CreatedBy = username;
            _dbSet.Add(input);
        }

        /// <summary>
        /// Lịch sử cập nhật sản phẩm mở bán của sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="openSellDetailId"></param>
        /// <param name="username"></param>
        public void HistoryOpenSellDetail(int orderId, int? openSellDetailId, string username)
        {
            _logger.LogInformation($"{nameof(RstHistoryUpdateEFRepository)}->{nameof(HistoryOpenSellDetail)}: orderId = {orderId}, policyDetailId = {openSellDetailId}, username = {username} ");

            string oldValue = (from o in _epicSchemaDbContext.RstOrders
                               join op in _epicSchemaDbContext.RstOpenSellDetails on o.OpenSellDetailId equals op.Id
                               where o.Id == orderId && o.Deleted == YesNo.NO && op.Deleted == YesNo.NO
                               select (o.ContractCode + " - " + " (" + op.Id + ")")).FirstOrDefault();

            string newValue = (from op in _epicSchemaDbContext.RstOpenSellDetails
                               where op.Id == openSellDetailId && op.Deleted == YesNo.NO
                               select (op.OpenSellId + " (" + op.Id + ")")).FirstOrDefault();

            if (oldValue != newValue)
            {
                Add(new RstHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = RstFieldName.UPDATE_OPEN_SELL_DETAIL,
                    UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = "cập nhật sản phẩm mở bán của sổ lệnh",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        /// <summary>
        /// Lịch sử cập nhật chính sách tại thời điểm đặt lệnh của sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="distributionPolicyId"></param>
        /// <param name="username"></param>
        public void HistoryDistributionPolicy(int orderId, int? distributionPolicyId, string username)
        {
            _logger.LogInformation($"{nameof(RstHistoryUpdateEFRepository)}->{nameof(HistoryOpenSellDetail)}: orderId = {orderId}, policyDetailId = {distributionPolicyId}, username = {username} ");

            string oldValue = (from o in _epicSchemaDbContext.RstOrders
                               join dp in _epicSchemaDbContext.RstDistributionPolicys on o.DistributionPolicyId equals dp.Id
                               where o.Id == orderId && o.Deleted == YesNo.NO && dp.Deleted == YesNo.NO
                               select (dp.Name + " - " + " (" + dp.Id + ")")).FirstOrDefault();

            string newValue = (from dp in _epicSchemaDbContext.RstDistributionPolicys
                               where dp.Id == distributionPolicyId && dp.Deleted == YesNo.NO
                               select (dp.Name + " (" + dp.Id + ")")).FirstOrDefault();

            if (oldValue != newValue)
            {
                Add(new RstHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = RstFieldName.UPDATE_DISTRIBUTION_POLICY,
                    UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = "cập nhật chính sách tại thời điểm đặt lệnh của sổ lệnh",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        /// <summary>
        /// Lịch sử cập nhật sản phẩm/căn của sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="productItemId"></param>
        /// <param name="username"></param>
        public void HistoryProductItem(int orderId, int? productItemId, string username)
        {
            _logger.LogInformation($"{nameof(RstHistoryUpdateEFRepository)}->{nameof(HistoryOpenSellDetail)}: orderId = {orderId}, policyDetailId = {productItemId}, username = {username} ");

            string oldValue = (from o in _epicSchemaDbContext.RstOrders
                               join p in _epicSchemaDbContext.RstProductItems on o.ProductItemId equals p.Id
                               where o.Id == orderId && o.Deleted == YesNo.NO && p.Deleted == YesNo.NO
                               select (p.Name + " - " + " (" + p.Id + ")")).FirstOrDefault();

            string newValue = (from p in _epicSchemaDbContext.RstProductItems
                               where p.Id == productItemId && p.Deleted == YesNo.NO
                               select (p.Name + " (" + p.Id + ")")).FirstOrDefault();

            if (oldValue != newValue)
            {
                Add(new RstHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = RstFieldName.UPDATE_PRODUCT_ITEM,
                    UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = "cập nhật sản phẩm/căn của sổ lệnh",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        /// <summary>
        /// Lịch sử cập nhật mã giới thiệu
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="referralCode"></param>
        /// <param name="username"></param>
        public void HistoryReferralCode(int orderId, string referralCode, string username)
        {
            _logger.LogInformation($"{nameof(RstHistoryUpdateEFRepository)}->{nameof(HistoryReferralCode)}: orderId = {orderId}, referralCode = {referralCode}, username = {username} ");

            string oldValue = (from o in _epicSchemaDbContext.RstOrders
                               join i in _epicSchemaDbContext.Investors.DefaultIfEmpty() on o.SaleReferralCode equals i.ReferralCodeSelf
                               join ii in _epicSchemaDbContext.InvestorIdentifications on i.InvestorId equals ii.InvestorId
                               where o.Id == orderId && i.Deleted == YesNo.NO && o.Deleted == YesNo.NO
                               select (o.SaleReferralCode + " - " + i.Name + " (" + i.InvestorId + ")")).FirstOrDefault();


            if (oldValue == null)
            {
                oldValue = (from o in _epicSchemaDbContext.RstOrders
                            join bs in _epicSchemaDbContext.BusinessCustomers.DefaultIfEmpty() on o.SaleReferralCode equals bs.ReferralCodeSelf
                            where o.Id == orderId && o.Deleted == YesNo.NO && bs.Deleted == YesNo.NO
                            select (o.SaleReferralCode + " - " + bs.Name + " (" + bs.BusinessCustomerId + ")")).FirstOrDefault();
            }


            string newValue = (from i in _epicSchemaDbContext.Investors
                               join ii in _epicSchemaDbContext.InvestorIdentifications on i.InvestorId equals ii.InvestorId
                               join cs in _epicSchemaDbContext.Sales on i.InvestorId equals cs.InvestorId
                               where i.ReferralCodeSelf == referralCode && i.Deleted == YesNo.NO
                               select (i.ReferralCodeSelf + " - " + ii.Fullname + " (" + i.InvestorId + ")")).FirstOrDefault();

            if (newValue == null)
            {
                newValue = (from bs in _epicSchemaDbContext.BusinessCustomers
                            where bs.ReferralCodeSelf == referralCode && bs.Deleted == YesNo.NO
                            select (bs.ReferralCodeSelf + " - " + bs.Name + " (" + bs.BusinessCustomerId + ")")).FirstOrDefault();
            }

            if (oldValue != newValue)
            {
                Add(new RstHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = RstFieldName.UPDATE_SALE_REFERRAL_CODE,
                    UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = "cập nhật mã giới thiệu",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        /// <summary>
        /// Lấy thông tin lịch sử theo bảng 
        /// </summary>
        /// <param name="updateTable"></param>
        /// <returns></returns>
        public PagingResult<RstHistoryUpdate> FindAllByTable(FilterRstHistoryUpdateDto input, int[] uploadTable)
        {
            _logger.LogInformation($"{nameof(RstHistoryUpdateEFRepository)}->{nameof(FindAllByTable)}: input = {JsonSerializer.Serialize(input)}");
            var resultPaging = new PagingResult<RstHistoryUpdate>();

            var history = _dbSet.Where(g => uploadTable.Contains(g.UpdateTable) && (input.RealTableId == null || input.RealTableId == g.RealTableId));

            resultPaging.TotalItems = history.Count();
            if (input.PageSize != -1)
            {
                history = history.Skip(input.Skip).Take(input.PageSize);
            }
            resultPaging.Items = history.OrderByDescending(o => o.Id).ToList();
            return resultPaging;
        }

        /// <summary>
        /// Lịch sử chỉnh sửa Offline => Online  của hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="source"></param>
        /// <param name="username"></param>
        public void HistoryOrderSource(int orderId, string username)
        {
            _logger.LogInformation($"{nameof(RstHistoryUpdateEFRepository)}->{nameof(HistoryReferralCode)}: orderId = {orderId}, username = {username} ");

            var order = _epicSchemaDbContext.RstOrders.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO);

            string oldValue = null;

            if (order.Source == SourceOrder.OFFLINE)
            {
                oldValue = SourceOrderText.OFFLINE;
            }

            string newValue = SourceOrderText.ONLINE;

            if (oldValue != null && oldValue != newValue)
            {
                Add(new RstHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = RstFieldName.UPDATE_SOURCE,
                    UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = "Chuyển Online",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        /// <summary>
        /// Lịch sửa cập nhật thông tin giấy tờ của khách hàng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="investorIdenId"></param>
        /// <param name="username"></param>
        public void HistoryinvestorIdentification(int orderId, int? investorIdenId, string username)
        {
            _logger.LogInformation($"{nameof(RstHistoryUpdateEFRepository)}->{nameof(HistoryinvestorIdentification)}: orderId = {orderId}, investorIdenId = {investorIdenId}, username = {username} ");

            var investor = (from o in _epicSchemaDbContext.RstOrders
                            join c in _epicSchemaDbContext.CifCodes on o.CifCode equals c.CifCode
                            where o.Id == orderId
                            select c).FirstOrDefault();
            string oldValue = null;
            string newValue = null;
            if (investor != null)
            {
                oldValue = (from order in _epicSchemaDbContext.RstOrders
                            join iden in _epicSchemaDbContext.InvestorIdentifications
                            on order.InvestorIdenId equals iden.Id
                            where order.Id == orderId && order.Deleted == YesNo.NO && iden.Deleted == YesNo.NO
                            select (iden.IdNo + " (" + iden.Id + ")")).FirstOrDefault();

                newValue = (from iden in _epicSchemaDbContext.InvestorIdentifications
                            where iden.Id == investorIdenId && iden.Deleted == YesNo.NO
                            select (iden.IdNo + " (" + iden.Id + ")")).FirstOrDefault();
            }
            else
            {
                ThrowException(ErrorCode.InvestorNotFound);
            }

            if (oldValue != newValue)
            {
                Add(new RstHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = RstFieldName.UPDATE_INVESTOR_IDEN_ID,
                    UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = "Cập nhật giấy tờ khách hàng",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        public void HistoryContactAddress(int orderId, int? contactAddressId, string username)
        {
            _logger.LogInformation($"{nameof(HistoryContactAddress)}: orderId = {orderId}, investorIdenId = {contactAddressId}, username = {username} ");

            var investor = (from o in _epicSchemaDbContext.RstOrders
                            join c in _epicSchemaDbContext.CifCodes on o.CifCode equals c.CifCode
                            where o.Id == orderId
                            select c).FirstOrDefault();
            string oldValue = null;
            string newValue = null;
            if (investor != null)
            {
                oldValue = (from order in _epicSchemaDbContext.RstOrders
                            join contact in _epicSchemaDbContext.InvestorContactAddresses
                            on order.ContractAddressId equals contact.ContactAddressId
                            where order.Id == orderId && order.Deleted == YesNo.NO && contact.Deleted == YesNo.NO
                            select (contact.ContactAddress + " (" + contact.ContactAddressId + ")")).FirstOrDefault();

                newValue = (from contact in _epicSchemaDbContext.InvestorContactAddresses
                            where contact.ContactAddressId == contactAddressId && contact.Deleted == YesNo.NO
                            select (contact.ContactAddress + " (" + contact.ContactAddressId + ")")).FirstOrDefault();
            }
            else
            {
                ThrowException(ErrorCode.InvestorNotFound);
            }

            if (oldValue != newValue)
            {
                Add(new RstHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = RstFieldName.UPDATE_CONTACT_ADDRESS_ID,
                    UpdateTable = RstHistoryUpdateTables.RST_ORDER,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = "Cập nhật địa chỉ khách hàng",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        public void HistoryBuildingDensityId(int productItemId, int? buildingDensityId, string username)
        {
            _logger.LogInformation($"{nameof(HistoryContactAddress)}: productItemId = {productItemId}, buildingDensityId = {buildingDensityId}, username = {username} ");

            var projectStructure = _epicSchemaDbContext.RstProjectStructures.FirstOrDefault(o => o.Id == buildingDensityId && o.Deleted == YesNo.NO);

            string oldValue = null;
            string newValue = null;
            if (projectStructure != null)
            {
                var oldProjectStructure = (from productItem in _epicSchemaDbContext.RstProductItems
                                           join projectStructures in _epicSchemaDbContext.RstProjectStructures on productItem.BuildingDensityId equals projectStructures.Id
                                           where productItem.Deleted == YesNo.NO && projectStructures.Deleted == YesNo.NO
                                           select new { projectStructures.BuildingDensityType, projectStructures.Name }).FirstOrDefault();

                // giá trị cũ
                oldValue = RstBuildingDensityTypes.BuildingDensityTypes(oldProjectStructure.BuildingDensityType) + " " + oldProjectStructure.Name;

                // giá trị mới
                newValue = RstBuildingDensityTypes.BuildingDensityTypes(projectStructure.BuildingDensityType) + " " + projectStructure.Name;
            }
            else
            {
                ThrowException(ErrorCode.RstProjectStructureNotFound);
            }

            if (oldValue != newValue)
            {
                Add(new RstHistoryUpdate
                {
                    RealTableId = productItemId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = RstFieldName.UPDATE_BUILDING_DENSITY_ID,
                    UpdateTable = RstHistoryUpdateTables.RST_PRODUCT_ITEM,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = "Cập nhật mật độ xây dựng",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }
    }
}
