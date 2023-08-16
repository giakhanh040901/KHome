using DocumentFormat.OpenXml.Vml.Office;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerHistory;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using Microsoft.EntityFrameworkCore.Query.SqlExpressions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace EPIC.GarnerRepositories
{
    public class GarnerHistoryUpdateEFRepository : BaseEFRepository<GarnerHistoryUpdate>
    {
        public GarnerHistoryUpdateEFRepository(EpicSchemaDbContext dbContext, ILogger logger) : base(dbContext, logger, $"{DbSchemas.EPIC_GARNER}.{GarnerHistoryUpdate.SEQ}")
        {
        }

        /// <summary>
        /// Thêm lịch sử thao tác dữ liệu
        /// </summary>
        /// <param name="input"></param>
        /// <param name="username"></param>
        public void Add(GarnerHistoryUpdate input, string username)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}, username = {username}");
            input.Id = (int)NextKey();
            input.CreatedBy= username;
            input.CreatedDate = DateTime.Now;
            _dbSet.Add(input);
        }

        public void HistoryBankAcc(long orderId, int? investorBankAccId, string username)
        {
            _logger.LogInformation($"{nameof(HistoryBankAcc)}: orderId = {orderId}, investorBankAccId = {investorBankAccId}, username = {username} ");
            var investor = (from o in _epicSchemaDbContext.GarnerOrders
                           join c in _epicSchemaDbContext.CifCodes on o.CifCode equals c.CifCode
                           where o.Id == orderId
                           select c.InvestorId).FirstOrDefault();

            string oldValue;
            string newValue;
            if (investor == null)
            {
                oldValue = (from o in _epicSchemaDbContext.GarnerOrders
                               join b in _epicSchemaDbContext.BusinessCustomerBanks
                               on o.BusinessCustomerBankAccId equals b.BusinessCustomerBankAccId
                               where o.Id == orderId && o.Deleted == YesNo.NO && b.Deleted == YesNo.NO
                               select (b.BankAccNo + " - " + b.BankName + " (" + b.BusinessCustomerBankAccId + ")")).FirstOrDefault();

                newValue = (from bcb in _epicSchemaDbContext.BusinessCustomerBanks
                               join cb in _epicSchemaDbContext.CoreBanks on bcb.BankId equals cb.BankId
                               where bcb.BusinessCustomerBankAccId == investorBankAccId && bcb.Deleted == YesNo.NO
                               select (bcb.BankAccNo + " - " + cb.BankName + " (" + bcb.BusinessCustomerBankAccId + ")")).FirstOrDefault();
            }
            else
            {
                oldValue = (from o in _epicSchemaDbContext.GarnerOrders
                               join iba in _epicSchemaDbContext.InvestorBankAccounts on o.InvestorBankAccId equals iba.Id
                               join cb in _epicSchemaDbContext.CoreBanks on iba.BankId equals cb.BankId
                               where iba.Deleted == YesNo.NO && o.Id == orderId && o.Deleted == YesNo.NO
                               select (iba.BankAccount + " - " + cb.BankName + " (" + iba.Id + ")")).FirstOrDefault();
                                

                newValue = (from iba in _epicSchemaDbContext.InvestorBankAccounts
                               join cb in _epicSchemaDbContext.CoreBanks on iba.BankId equals cb.BankId
                               where iba.Id == investorBankAccId && iba.Deleted == YesNo.NO
                               select (iba.BankAccount + " - " + cb.BankName + " (" + iba.Id + ")")).FirstOrDefault();
            }
            if(oldValue != newValue)
            {
                Add(new GarnerHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = GarnerFieldName.UPDATE_BUSINESS_BANK_ACC,
                    UpdateTable = GarnerHistoryUpdateTables.GAN_ORDER,
                    Action = GarnerHistoryAction.CAP_NHAT,
                    Summary = " ",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        /// <summary>
        /// lịch sử cập nhật kỳ hạn của sổ lệnh
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="policyDetailId"></param>
        /// <param name="username"></param>
        public void HistoryPolicyDetail(long orderId, int? policyDetailId, string username)
        {
            _logger.LogInformation($"{nameof(HistoryPolicyDetail)}: orderId = {orderId}, policyDetailId = {policyDetailId}, username = {username} ");

            string oldValue = (from o in _epicSchemaDbContext.GarnerOrders
                               join p in _epicSchemaDbContext.GarnerPolicies on o.PolicyId equals p.Id
                               join pd in _epicSchemaDbContext.GarnerPolicyDetails on o.PolicyDetailId equals pd.Id
                               where o.Id == orderId && o.Deleted == YesNo.NO && p.Deleted == YesNo.NO
                               select (pd.Name + " - " + p.Name + " (" + pd.Id + ")")).FirstOrDefault();

            string newValue = (from pd in _epicSchemaDbContext.GarnerPolicyDetails 
                               join p in _epicSchemaDbContext.GarnerPolicies on pd.PolicyId equals p.Id
                               where pd.Id == policyDetailId && pd.Deleted == YesNo.NO && p.Deleted == YesNo.NO
                               select (pd.Name + " - " + p.Name + " (" + pd.Id + ")")).FirstOrDefault();

            if(oldValue != newValue)
            {
                Add(new GarnerHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = GarnerFieldName.UPDATE_POLICY_DETAIL,
                    UpdateTable = GarnerHistoryUpdateTables.GAN_ORDER,
                    Action = GarnerHistoryAction.CAP_NHAT,
                    Summary = "cập nhật kỳ hạn của sổ lệnh",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        /// <summary>
        /// Lịch sử cập nhật hạn mức của đại lý phân phối
        /// </summary>
        /// <param name="input"></param>
        /// <param name="partnerId"></param>
        /// <param name="username"></param>
        public void HistoryProductTradingProvider(GarnerProductTradingProvider input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(HistoryProductTradingProvider)}: input = {input}, partnerId = {partnerId}, username = {username} ");
            var productTradingProviderFind = _epicSchemaDbContext.GarnerProductTradingProviders.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && input.TradingProviderId == p.TradingProviderId && p.Deleted == YesNo.NO);
            if (productTradingProviderFind != null)
            {
                // cài đặt hạn mức
                if (productTradingProviderFind.HasTotalInvestmentSub != input.HasTotalInvestmentSub)
                {
                    Add(new GarnerHistoryUpdate(productTradingProviderFind.Id, productTradingProviderFind.HasTotalInvestmentSub, input.HasTotalInvestmentSub, GarnerFieldName.UPDATE_HAS_TOTAL_INVESTMENT_SUB,
                        GarnerHistoryUpdateTables.GAN_PRODUCT_TRADING_PROVIDER, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_PRODUCT_TRADING_PROVIDER), username);
                }
                // số tiền
                if (productTradingProviderFind.TotalInvestmentSub != input.TotalInvestmentSub)
                {
                    Add(new GarnerHistoryUpdate(productTradingProviderFind.Id, productTradingProviderFind.TotalInvestmentSub?.ToString(), input.TotalInvestmentSub?.ToString(), GarnerFieldName.UPDATE_TOTAL_INVESTMENT_SUB,
                        GarnerHistoryUpdateTables.GAN_PRODUCT_TRADING_PROVIDER, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_PRODUCT_TRADING_PROVIDER), username);
                }
                // Số lượng
                if (productTradingProviderFind.Quantity != input.Quantity)
                {
                    Add(new GarnerHistoryUpdate(productTradingProviderFind.Id, productTradingProviderFind.Quantity?.ToString(), input.Quantity?.ToString(), GarnerFieldName.UPDATE_QUANTITY,
                        GarnerHistoryUpdateTables.GAN_PRODUCT_TRADING_PROVIDER, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_PRODUCT_TRADING_PROVIDER), username);
                }
                // Chi trả lợi nhuận
                if (productTradingProviderFind.IsProfitFromPartner != input.IsProfitFromPartner)
                {
                    Add(new GarnerHistoryUpdate(productTradingProviderFind.Id, productTradingProviderFind.IsProfitFromPartner, input.IsProfitFromPartner, GarnerFieldName.UPDATE_IS_PROFIT_FROM_PARTNER,
                        GarnerHistoryUpdateTables.GAN_PRODUCT_TRADING_PROVIDER, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_PRODUCT_TRADING_PROVIDER), username);
                }
                // Ngày phân phối
                if (productTradingProviderFind.DistributionDate != input.DistributionDate)
                {
                    Add(new GarnerHistoryUpdate(productTradingProviderFind.Id, productTradingProviderFind.DistributionDate.ToString(), input.DistributionDate.ToString(), GarnerFieldName.UPDATE_DISTRIBUTION_DATE,
                        GarnerHistoryUpdateTables.GAN_PRODUCT_TRADING_PROVIDER, ActionTypes.CAP_NHAT, GarnerHistoryUpdateSummary.SUMMARY_PRODUCT_TRADING_PROVIDER), username);
                }
            }

        }

        // thêm lịch sử khi cập nhận file product
        public void HistoryCollateralUpdate(CreateGarnerProductFileDto input, int partnerId, string username)
        {
            _logger.LogInformation($"{nameof(HistoryCollateralUpdate)}: input = {JsonSerializer.Serialize(input)}, partnerId = {partnerId}, username = {username}");
            var fileFind = _epicSchemaDbContext.GarnerProductFiles.FirstOrDefault(p => p.Id == input.Id && p.PartnerId == partnerId && p.Deleted == YesNo.NO);

            var type = GarnerDocumentTypes.DocumentTypes(input.DocumentType);

            if (fileFind != null)
            {
                // tiêu đề
                if (fileFind.Title != input.Title)
                {
                    Add(new GarnerHistoryUpdate(fileFind.Id, fileFind.Title, input.Title, GarnerFieldName.UPDATE_FILE_TITLE,
                        GarnerHistoryUpdateTables.GAN_PRODUCT_FILE, ActionTypes.CAP_NHAT, type), username);
                }
                // đường dẫn file
                if (fileFind.Url != input.Url)
                {
                    Add(new GarnerHistoryUpdate(fileFind.Id, fileFind.Url, input.Url, GarnerFieldName.UPDATE_FILE_URL,
                        GarnerHistoryUpdateTables.GAN_PRODUCT_FILE, ActionTypes.CAP_NHAT, type), username);
                }
                // tổng giá trị
                if (fileFind.TotalValue != input.TotalValue)
                {
                    Add(new GarnerHistoryUpdate(fileFind.Id, fileFind?.TotalValue.ToString(), input.TotalValue.ToString(), GarnerFieldName.UPDATE_FILE_TOTAL_VALUE,
                        GarnerHistoryUpdateTables.GAN_PRODUCT_FILE, ActionTypes.CAP_NHAT, type), username);
                }
                // miêu tả
                if (fileFind.Description != input.Description)
                {
                    Add(new GarnerHistoryUpdate(fileFind.Id, fileFind.Description, input.Description, GarnerFieldName.UPDATE_FILE_DESCRIPTION,
                        GarnerHistoryUpdateTables.GAN_PRODUCT_FILE, ActionTypes.CAP_NHAT, type), username);
                }
            }
        }

        /// <summary>
        /// lịch sử cập nhật số tiền đầu tư
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="totalValue"></param>
        /// <param name="username"></param>
        public void HistoryTotalValue (long orderId, decimal? totalValue, string username)
        {
            _logger.LogInformation($"{nameof(HistoryTotalValue)}: orderId = {orderId}, totalValue = {totalValue}, username = {username} ");
            var total = _epicSchemaDbContext.GarnerOrders.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO);
            var oldValue = total.TotalValue;
            if(oldValue != totalValue)
            {
                Add(new GarnerHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = NumberToText.ConvertNumberIS((double)oldValue),
                    NewValue = NumberToText.ConvertNumberIS((double)totalValue),
                    FieldName = GarnerFieldName.UPDATE_TOTAL_VALUE,
                    UpdateTable = GarnerHistoryUpdateTables.GAN_ORDER,
                    Action = GarnerHistoryAction.CAP_NHAT,
                    Summary = "cập nhật số tiền đầu tư",
                    CreatedDate = DateTime.Now,
                }, username);
            }    
        }

        /// <summary>
        /// Lấy thông tin lịch sử theo bảng 
        /// </summary>
        /// <param name="updateTable"></param>
        /// <returns></returns>
        public PagingResult<GarnerHistoryUpdate> FindAllByTable(FilterGarnerHistoryDto input)
        {
            _logger.LogInformation($"{nameof(FindAllByTable)}: input = {JsonSerializer.Serialize(input)}");
            var resultPaging = new PagingResult<GarnerHistoryUpdate>();

            var history = _dbSet.Where(g => g.UpdateTable == input.UploadTable && (input.RealTableId == null || input.RealTableId == g.RealTableId));
            resultPaging.TotalItems = history.Count();
            if (input.PageSize != -1)
            {
                history = history.Skip(input.Skip).Take(input.PageSize);
            }
            resultPaging.Items = history;
            return resultPaging;
        }

        /// <summary>
        /// Lấy thông tin lịch sử theo sản phẩm đầu tư
        /// </summary>
        /// <param name="productId"></param>
        /// <returns></returns>
        public List<GarnerHistoryUpdate> FindByProductId(int productId)
        {
            _logger.LogInformation($"{System.Reflection.MethodBase.GetCurrentMethod().Name}: productId = {productId}");
            var result = new List<GarnerHistoryUpdate>();
            //result.AddRange(FindAllByTable(GarnerHistoryUpdateTables.GAN_PRODUCT, productId));
            var historyProduct = FindAllByTable(new FilterGarnerHistoryDto()
            {
                UploadTable = GarnerHistoryUpdateTables.GAN_PRODUCT,
                RealTableId = productId,
                PageSize = -1,
            });

            result.AddRange(historyProduct.Items);

            var historyUpdateProductTradingProvider = from historyUpdate in _dbSet
                                                      join productTrading in _epicSchemaDbContext.GarnerProductTradingProviders
                                                      on historyUpdate.RealTableId equals productTrading.Id
                                                      where productTrading.ProductId == productId && historyUpdate.UpdateTable == GarnerHistoryUpdateTables.GAN_PRODUCT_TRADING_PROVIDER
                                                      && productTrading.Deleted == YesNo.NO
                                                      select historyUpdate;
            result.AddRange(historyUpdateProductTradingProvider);
            return result.OrderByDescending(e => e.CreatedDate).ToList();
        }

        /// <summary>
        /// Lịch sử chỉnh sửa Offline => Online  của hợp đồng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="source"></param>
        /// <param name="username"></param>
        public void HistoryOrderSource(long orderId, string username)
        {
            _logger.LogInformation($"{nameof(HistoryOrderSource)}: orderId = {orderId}, username = {username} ");

            var order = _epicSchemaDbContext.GarnerOrders.FirstOrDefault(o => o.Id == orderId && o.Deleted == YesNo.NO);

            string oldValue = null;

            if (order.Source == SourceOrder.OFFLINE)
            {
                oldValue = SourceOrderText.OFFLINE;
            }


            string newValue = SourceOrderText.ONLINE;


            if (oldValue != null && oldValue != newValue)
            {
                Add(new GarnerHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = GarnerFieldName.UPDATE_SOURCE,
                    UpdateTable = GarnerHistoryUpdateTables.GAN_ORDER,
                    Action = GarnerHistoryAction.CAP_NHAT,
                    Summary = "Chuyển Online",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        /// <summary>
        /// Lịch sửa cập nhật thông tin giấy tờ của khách hàng
        /// </summary>
        /// <param name="orderId"></param>
        /// <param name="contactAddressId"></param>
        /// <param name="username"></param>
        public void HistoryinvestorIdentification(long orderId ,int? investorIdenId ,string username)
        {
            _logger.LogInformation($"{nameof(HistoryinvestorIdentification)}: orderId = {orderId}, investorIdenId = {investorIdenId}, username = {username} ");

            var investor = (from o in _epicSchemaDbContext.GarnerOrders
                            join c in _epicSchemaDbContext.CifCodes on o.CifCode equals c.CifCode
                            where o.Id == orderId
                            select c).FirstOrDefault();
            string oldValue = null;
            string newValue = null;
            if (investor != null)
            {
                oldValue = (from order in _epicSchemaDbContext.GarnerOrders
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
                Add(new GarnerHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = GarnerFieldName.UPDATE_BUSINESS_BANK_ACC,
                    UpdateTable = GarnerHistoryUpdateTables.GAN_ORDER,
                    Action = GarnerHistoryAction.CAP_NHAT,
                    Summary = "Cập nhật giấy tờ khách hàng",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        public void HistoryContactAddress(long orderId, int? contactAddressId, string username)
        {
            _logger.LogInformation($"{nameof(HistoryContactAddress)}: orderId = {orderId}, investorIdenId = {contactAddressId}, username = {username} ");

            var investor = (from o in _epicSchemaDbContext.GarnerOrders
                            join c in _epicSchemaDbContext.CifCodes on o.CifCode equals c.CifCode
                            where o.Id == orderId
                            select c).FirstOrDefault();
            string oldValue = null;
            string newValue = null;
            if (investor != null)
            {
                oldValue = (from order in _epicSchemaDbContext.GarnerOrders
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
                Add(new GarnerHistoryUpdate
                {
                    RealTableId = orderId,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = GarnerFieldName.UPDATE_CONTACT_ADDRESS_ID,
                    UpdateTable = GarnerHistoryUpdateTables.GAN_ORDER,
                    Action = GarnerHistoryAction.CAP_NHAT,
                    Summary = "Cập nhật địa chỉ khách hàng",
                    CreatedDate = DateTime.Now,
                }, username);
            }
        }

        /// <summary>
        /// Lấy thông tin lịch sử theo tab Phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public PagingResult<GarnerHistoryUpdate> FindAllByMultiTable(FilterGarnerDistributionHistoryDto input)
        {
            _logger.LogInformation($"{nameof(FindAllByTable)}: input = {JsonSerializer.Serialize(input)}");
            var resultPaging = new PagingResult<GarnerHistoryUpdate>();

            List<GarnerHistoryUpdate> history = new();
            //Lịch sử distribution
            var distributionbankHistory = _dbSet.Where(e => e.UpdateTable == GarnerHistoryUpdateTables.GAN_DISTRIBUTION_TRADING_BANK_ACCOUNT && e.RealTableId == input.DistributionId);
            var distributionHistory = _dbSet.Where(e => e.UpdateTable == GarnerHistoryUpdateTables.GAN_DISTRIBUTION && e.RealTableId == input.DistributionId);
            history.AddRange(distributionbankHistory.ToList());
            history.AddRange(distributionHistory.ToList());
            //Lịch sử bảng giá
            var distributionPrice = from priceHistory in _dbSet
                                    join price in _epicSchemaDbContext.GarnerProductPrices on priceHistory.RealTableId equals price.Id
                                    where priceHistory.UpdateTable == GarnerHistoryUpdateTables.GAN_PRODUCT_PRICE && price.DistributionId == input.DistributionId select priceHistory;
            history.AddRange(distributionPrice.ToList());

            //chính sách
            var policyHistory = from h in _dbSet
                                join policy in _epicSchemaDbContext.GarnerPolicies on h.RealTableId equals policy.Id
                                where h.UpdateTable == GarnerHistoryUpdateTables.GAN_POLICY && policy.DistributionId == input.DistributionId
                                select h;
            history.AddRange(policyHistory.ToList());

            //Lịch sử kỳ hạn
            var policyDetailHistory = from h in _dbSet
                                      join policyDetail in _epicSchemaDbContext.GarnerPolicyDetails on h.RealTableId equals policyDetail.Id
                                      where h.UpdateTable == GarnerHistoryUpdateTables.GAN_POLICY_DETAIL && policyDetail.DistributionId == input.DistributionId
                                      select h;
            history.AddRange(policyDetailHistory.ToList());

            //Lích sử file chính sách
            var policyFileHistory = from h in _dbSet
                                    join policyFile in _epicSchemaDbContext.GarnerProductOverviewFiles on h.RealTableId equals policyFile.Id
                                    where h.UpdateTable == GarnerHistoryUpdateTables.GAN_PRODUCT_OVERVIEW_FILE && policyFile.DistributionId == input.DistributionId
                                    select h;
            history.AddRange(policyFileHistory.ToList());

            //Mấu hợp đồng
            var contractTemplateHistory = from h in _dbSet
                                          join contractTemplate in _epicSchemaDbContext.GarnerContractTemplates on h.RealTableId equals contractTemplate.Id
                                          join policy in _epicSchemaDbContext.GarnerPolicies on contractTemplate.PolicyId equals policy.Id
                                          where h.UpdateTable == GarnerHistoryUpdateTables.GAN_RECEIVE_CONTRACT_TEMP && policy.DistributionId == input.DistributionId
                                          select h;
            history.AddRange(contractTemplateHistory.ToList());

            //Hợp đồng gia nhận
            var receiveContractHistory = from h in _dbSet
                                         join receiveContract in _epicSchemaDbContext.GarnerReceiveContractTemps on h.RealTableId equals receiveContract.Id
                                         where h.UpdateTable == GarnerHistoryUpdateTables.GAN_RECEIVE_CONTRACT_TEMP && receiveContract.DistributionId == input.DistributionId
                                         select h;
            history.AddRange(receiveContractHistory.ToList());

            resultPaging.TotalItems = history.Count();

            if (input.PageSize != -1)
            {
                history = history.Skip(input.Skip).Take(input.PageSize).ToList();
            }
            resultPaging.Items = history.OrderByDescending(e => e.Id);
            return resultPaging;
        }
    }
}
