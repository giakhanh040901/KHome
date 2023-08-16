using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using DocumentFormat.OpenXml.Math;
using DocumentFormat.OpenXml.Office.CustomUI;
using DocumentFormat.OpenXml.Office2010.CustomUI;
using DocumentFormat.OpenXml.Office2010.Excel;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using EPIC.CoreRepositories;
using EPIC.CoreRepositoryExtensions;
using EPIC.CoreSharedServices.Implements;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.RocketChat;
using EPIC.Entities.Dto.Sale;
using EPIC.FileEntities.Settings;
using EPIC.GarnerDomain.Interfaces;
using EPIC.GarnerEntities.DataEntities;
using EPIC.GarnerEntities.Dto.GarnerApprove;
using EPIC.GarnerEntities.Dto.GarnerContractTemplate;
using EPIC.GarnerEntities.Dto.GarnerContractTemplateTemp;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCode;
using EPIC.GarnerEntities.Dto.GarnerDistributionConfigContractCodeDetail;
using EPIC.GarnerEntities.Dto.GarnerHistory;
using EPIC.GarnerEntities.Dto.GarnerPolicy;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetail;
using EPIC.GarnerEntities.Dto.GarnerPolicyDetailTemp;
using EPIC.GarnerEntities.Dto.GarnerProduct;
using EPIC.GarnerEntities.Dto.GarnerProductOverview;
using EPIC.GarnerEntities.Dto.GarnerProductPrice;
using EPIC.GarnerEntities.Dto.GarnerProductTradingProvider;
using EPIC.GarnerRepositories;
using EPIC.InvestEntities.DataEntities;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IISIntegration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MySqlX.XDevAPI.Common;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Transactions;
using static EPIC.GarnerRepositories.GarnerDistributionEFRepository;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EPIC.GarnerDomain.Implements
{
    public class GarnerDistributionServices : IGarnerDistributionServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<GarnerDistributionServices> _logger;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly ILogicInvestorTradingSharedServices _logicInvestorTradingSharedServices;
        private readonly GarnerProductEFRepository _garnerProductEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly GarnerProductTradingProviderEFRepository _garnerProductTradingProviderEFRepository;
        private readonly GarnerApproveEFRepository _garnerApproveEFRepository;
        private readonly GarnerProductTypeEFRepository _garnerProductTypeEFRepository;
        private readonly GarnerHistoryUpdateEFRepository _garnerHistoryUpdateEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly GarnerDistributionEFRepository _garnerDistributionEFRepository;
        private readonly GarnerProductOverviewFileEFRepository _garnerProductOverviewFileEFRepository;
        private readonly GarnerProductOverviewOrgEFRepository _garnerProductOverviewOrgEFRepository;
        private readonly GarnerOrderEFRepository _garnerOrderEFRepository;
        private readonly GarnerPolicyEFRepository _garnerPolicyEFRepository;
        private readonly GarnerPolicyTempEFRepository _garnerPolicyTempEFRepository;
        private readonly GarnerPolicyDetailEFRepository _garnerPolicyDetailEFRepository;
        private readonly GarnerPolicyDetailTempEFRepository _garnerPolicyDetailTempEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly InvestorSaleEFRepository _investorSaleEFRepository;
        private readonly GarnerInterestPaymentEFRepository _garnerInterestPaymentEFRepository;
        private readonly GarnerDistributionTradingBankAccountRepository _garnerDistributionTradingBankAccountRepository;
        private readonly GarnerContractTemplateEFRepository _garnerContractTemplateEFRepository;
        private readonly GarnerContractTemplateTempEFRepository _garnerContractTemplateTempEFRepository;
        private readonly IGarnerFormulaServices _garnerFormulaServices;
        private readonly GarnerConfigContractCodeEFRepository _garnerConfigContractCodeEFRepository;
        private readonly GarnerConfigContractCodeDetailEFRepository _garnerConfigContractCodeDetailEFRepository;
        private readonly GarnerProductPriceEFRepository _garnerProductPriceEFRepository;
        private readonly IGarnerDistributionSharedServices _garnerDistributionSharedServices;
        private readonly BusinessCustomerBankEFRepository _businessCustomerBankEFRepository;
        private readonly BankEFRepository _bankEFRepository;

        public GarnerDistributionServices(EpicSchemaDbContext dbContext,
            DatabaseOptions databaseOptions,
            IMapper mapper,
            ILogger<GarnerDistributionServices> logger,
            IHttpContextAccessor httpContextAccessor,
            IGarnerFormulaServices garnerFormulaServices,
            IGarnerDistributionSharedServices garnerDistributionSharedServices,
            ILogicInvestorTradingSharedServices logicInvestorTradingSharedServices)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _httpContext = httpContextAccessor;
            _logicInvestorTradingSharedServices = logicInvestorTradingSharedServices;
            _garnerProductEFRepository = new GarnerProductEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _garnerProductTradingProviderEFRepository = new GarnerProductTradingProviderEFRepository(dbContext, logger);
            _garnerApproveEFRepository = new GarnerApproveEFRepository(dbContext, logger);
            _garnerProductTypeEFRepository = new GarnerProductTypeEFRepository(dbContext, logger);
            _garnerHistoryUpdateEFRepository = new GarnerHistoryUpdateEFRepository(dbContext, logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(dbContext, logger);
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(dbContext, logger);
            _garnerDistributionEFRepository = new GarnerDistributionEFRepository(dbContext, logger);
            _garnerProductOverviewFileEFRepository = new GarnerProductOverviewFileEFRepository(dbContext, logger);
            _garnerProductOverviewOrgEFRepository = new GarnerProductOverviewOrgEFRepository(dbContext, logger);
            _garnerOrderEFRepository = new GarnerOrderEFRepository(dbContext, logger);
            _garnerPolicyEFRepository = new GarnerPolicyEFRepository(dbContext, logger);
            _garnerPolicyTempEFRepository = new GarnerPolicyTempEFRepository(dbContext, logger);
            _garnerPolicyDetailEFRepository = new GarnerPolicyDetailEFRepository(dbContext, logger);
            _garnerPolicyDetailTempEFRepository = new GarnerPolicyDetailTempEFRepository(dbContext, logger);
            _garnerDistributionTradingBankAccountRepository = new GarnerDistributionTradingBankAccountRepository(dbContext, logger);
            _saleEFRepository = new SaleEFRepository(dbContext, logger);
            _investorSaleEFRepository = new InvestorSaleEFRepository(dbContext, logger);
            _garnerInterestPaymentEFRepository = new GarnerInterestPaymentEFRepository(dbContext, logger);
            _garnerContractTemplateEFRepository = new GarnerContractTemplateEFRepository(dbContext, logger);
            _garnerContractTemplateTempEFRepository = new GarnerContractTemplateTempEFRepository(dbContext, logger);
            _garnerFormulaServices = garnerFormulaServices;
            _garnerConfigContractCodeEFRepository = new GarnerConfigContractCodeEFRepository(dbContext, logger);
            _garnerConfigContractCodeDetailEFRepository = new GarnerConfigContractCodeDetailEFRepository(dbContext, logger);
            _garnerProductPriceEFRepository = new GarnerProductPriceEFRepository(dbContext, logger);
            _garnerDistributionSharedServices = garnerDistributionSharedServices;
            _businessCustomerBankEFRepository = new BusinessCustomerBankEFRepository(dbContext, logger);
            _bankEFRepository = new BankEFRepository(dbContext, logger);
        }

        #region Phân phối sản phẩm
        public GarnerDistribution Add(CreateGarnerDistributionDto input)
        {
            _logger.LogInformation($"{nameof(Add)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var productFind = _garnerProductEFRepository.FindById(input.ProductId);
            if (productFind == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerProductNotFound);
            }
            var insert = _garnerDistributionEFRepository.Add(_mapper.Map<GarnerDistribution>(input), tradingProviderId, username);

            // Thêm nhiều tài khoản ngân hàng thu và chi
            _garnerDistributionTradingBankAccountRepository.UpdateTradingBankAccount(insert.Id, input.TradingBankAccountCollects, input.TradingBankAccountPays, username);
            _dbContext.SaveChanges();
            return insert;
        }

        public GarnerDistribution Update(UpdateGarnerDistributionDto input)
        {
            _logger.LogInformation($"{nameof(Update)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var productFind = _garnerProductEFRepository.FindById(input.ProductId);
            if (productFind == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductNotFound)), new FaultCode(((int)ErrorCode.GarnerProductNotFound).ToString()), "");
            }
            // get distribution
            var distribution = _garnerDistributionEFRepository.FindById(input.Id).ThrowIfNull(_dbContext, ErrorCode.GarnerDistributionNotFound);
            //Lấy ra ngân hàng trước khi update
            var distributionTradingBankCollects = _garnerDistributionTradingBankAccountRepository.Entity.Where(e => e.DistributionId == input.Id && e.Type == DistributionTradingBankAccountTypes.THU
                                                                                                                                    && e.Deleted == YesNo.NO).ToList();
            var distributionTradingBankPays = _garnerDistributionTradingBankAccountRepository.Entity.Where(e => e.DistributionId == input.Id && e.Type == DistributionTradingBankAccountTypes.CHI
                                                                                                                                    && e.Deleted == YesNo.NO).ToList();

            //Lịch sử update ngày bắt đầu
            if (distribution.OpenCellDate != input.OpenCellDate)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_DISTRIBUTION,
                    RealTableId = distribution.Id,
                    OldValue = distribution.OpenCellDate.ToString(),
                    NewValue = input.OpenCellDate.ToString(),
                    FieldName = GarnerFieldName.UPDATE_OPEN_CELL_DATE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_OPEN_CELL_DATE
                }, username);
            }

            //Lịch sử update kết thúc
            if (distribution.CloseCellDate != input.CloseCellDate)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_DISTRIBUTION,
                    RealTableId = distribution.Id,
                    OldValue = distribution.CloseCellDate.ToString(),
                    NewValue = input.CloseCellDate.ToString(),
                    FieldName = GarnerFieldName.UPDATE_CLOSE_CELL_DATE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CLOSE_CELL_DATE
                }, username);
            }

            //Tài khoản nhận tiền
            if (!input.TradingBankAccountCollects.SequenceEqual(distributionTradingBankCollects.Select(e => e.BusinessCustomerBankAccId)))
            {
                //Giá trị cũ của tài khoản nhận
                List<string> oldValues = new List<string>();
                foreach (var item in distributionTradingBankCollects)
                {
                    var businessCustomerBank = _businessCustomerBankEFRepository.FindById(item.BusinessCustomerBankAccId);
                    var bankName = _bankEFRepository.FindById(businessCustomerBank.BankId);
                    string bankAccountInfo = businessCustomerBank.BankAccNo + " - " + bankName.BankName + "(" + item.BusinessCustomerBankAccId + ")";
                    oldValues.Add(bankAccountInfo);
                }
                string oldValue = String.Join(", ", oldValues);

                //Giá trị mới của tài khoản nhận
                List<string> newValues = new List<string>();
                foreach (var item in input.TradingBankAccountCollects)
                {
                    var businessCustomerBank = _businessCustomerBankEFRepository.FindById(item);
                    var bankName = _bankEFRepository.FindById(businessCustomerBank.BankId);
                    string bankAccountInfo = businessCustomerBank.BankAccNo + " - " + bankName.BankName + "(" + item + ")";
                    newValues.Add(bankAccountInfo);
                }
                string newValue = String.Join(", ", newValues);

                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_DISTRIBUTION_TRADING_BANK_ACCOUNT,
                    RealTableId = distribution.Id,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = GarnerFieldName.UPDATE_BUSINESS_CUSTOMER_BANK_ACC_ID,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_DISTRIBUTION_BANK_COLLECT
                }, username);
            }

            //Tài khoản chi tiền
            if (!input.TradingBankAccountPays.SequenceEqual(distributionTradingBankPays.Select(e => e.BusinessCustomerBankAccId)))
            {
                if (distributionTradingBankPays.Count() > 0)
                {
                    var realTableId = distributionTradingBankPays.First().Id;
                }
                //Giá trị cũ của tài khoản nhận
                List<string> oldValues = new List<string>();
                foreach (var item in distributionTradingBankPays)
                {
                    var businessCustomerBank = _businessCustomerBankEFRepository.FindById(item.BusinessCustomerBankAccId);
                    var bankName = _bankEFRepository.FindById(businessCustomerBank.BankId);
                    string bankAccountInfo = businessCustomerBank.BankAccNo + " - " + bankName.BankName + "(" + item.BusinessCustomerBankAccId + ")";
                    oldValues.Add(bankAccountInfo);
                }
                string oldValue = String.Join(", ", oldValues);

                //Giá trị mới của tài khoản nhận
                List<string> newValues = new List<string>();
                foreach (var item in input.TradingBankAccountPays)
                {
                    var businessCustomerBank = _businessCustomerBankEFRepository.FindById(item);
                    var bankName = _bankEFRepository.FindById(businessCustomerBank.BankId);
                    string bankAccountInfo = businessCustomerBank.BankAccNo + " - " + bankName.BankName + "(" + item + ")";
                    newValues.Add(bankAccountInfo);
                }
                string newValue = String.Join(", ", newValues);

                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_DISTRIBUTION_TRADING_BANK_ACCOUNT,
                    RealTableId = distribution.Id,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = GarnerFieldName.UPDATE_BUSINESS_CUSTOMER_BANK_ACC_ID,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_DISTRIBUTION_BANK_PAY
                }, username);
            }
            var update = _garnerDistributionEFRepository.Update(_mapper.Map<GarnerDistribution>(input), tradingProviderId, username);
            // Thêm nhiều tài khoản ngân hàng thu và chi
            _garnerDistributionTradingBankAccountRepository.UpdateTradingBankAccount(update.Id, input.TradingBankAccountCollects, input.TradingBankAccountPays, username);
            _dbContext.SaveChanges();
            return update;
        }

        /// <summary>
        /// Xem thông tin chi tiết phân phối sản phẩm
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public GarnerDistributionDto FindById(int id)
        {
            _logger.LogInformation($"{nameof(FindById)}: Id = {id}");

            var result = new GarnerDistributionDto();

            //Lấy thông tin phân phối sản phẩm
            var distributionFind = _garnerDistributionEFRepository.FindById(id);
            if (distributionFind == null)
            {
                throw new FaultException(new FaultReason($"{_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerDistributionNotFound)}"), new FaultCode(((int)ErrorCode.GarnerDistributionNotFound).ToString()), "");
            }

            result = _mapper.Map<GarnerDistributionDto>(distributionFind);

            // Lấy thông tin phân phối sản phẩm cho đại lý
            var productTradingProvider = _garnerDistributionSharedServices.LimitCalculationTrading(distributionFind.ProductId, distributionFind.TradingProviderId);
            if (productTradingProvider != null)
            {
                result.HasTotalInvestmentSub = productTradingProvider.HasTotalInvestmentSub;
                result.TotalInvestmentSub = productTradingProvider.TotalInvestmentSub;
                result.Quantity = productTradingProvider.Quantity;
            }

            //Lấy thông tin sản phẩm tích lũy
            var productFind = _garnerProductEFRepository.FindById(distributionFind.ProductId);
            if (productFind != null)
            {
                result.GarnerProduct = _mapper.Map<GarnerProductDto>(productFind);

                // Nếu là sản phẩm tích lũy Cổ Phần
                if (productFind.ProductType == GarnerProductTypes.CO_PHAN)
                {
                    // Lấy loại hình dự án
                    result.GarnerProduct.InvProductTypes = _garnerProductTypeEFRepository.FindAllByProductId(productFind.Id).Select(g => g.Type).ToList();

                    // Lấy thông tin Tổ chức phát hành
                    result.GarnerProduct.CpsIssuer = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.CpsIssuerId ?? 0));

                    // Lấy thông tin Đại lý lưu ký
                    result.GarnerProduct.CpsDepositProvider = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.CpsDepositProviderId ?? 0));
                }

                // Nếu là sản phẩm tích lũy Bất Động sản
                else if (productFind.ProductType == GarnerProductTypes.BAT_DONG_SAN)
                {
                    //Lấy loại hình dự án
                    result.GarnerProduct.InvProductTypes = _garnerProductTypeEFRepository.FindAllByProductId(id).Select(g => g.Type).ToList();

                    // Lấy thông tin Chủ đầu tư
                    result.GarnerProduct.InvOwner = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.InvOwnerId ?? 0));

                    // Lấy thông tin Tổng thầu
                    result.GarnerProduct.InvGeneralContractor = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.InvGeneralContractorId ?? 0));
                }
                result.IsInvested = _garnerOrderEFRepository.SumValue(id, null);
            }

            var listTradingBankAccountFind = _garnerDistributionTradingBankAccountRepository.FindAllListByDistribution(id);
            result.TradingBankAccountCollects = listTradingBankAccountFind.Where(b => b.Type == DistributionTradingBankAccountTypes.THU).Select(r => r.BusinessCustomerBankAccId).ToList();
            result.TradingBankAccountPays = listTradingBankAccountFind.Where(b => b.Type == DistributionTradingBankAccountTypes.CHI).Select(r => r.BusinessCustomerBankAccId).ToList();
            return result;
        }

        /// <summary>
        /// Lấy danh sách ngân hàng thu chi của đại lý theo Phân phối sản phẩm
        /// </summary>
        /// <param name="distributionId"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public List<BusinessCustomerBankDto> FindBankByDistributionId(int distributionId, int type)
        {
            _logger.LogInformation($"{nameof(FindBankByDistributionId)}: distributionId = {distributionId}, type = {type}");

            var tradingBankAccounts = _garnerDistributionTradingBankAccountRepository.FindAllListByDistribution(distributionId, type);

            var result = new List<BusinessCustomerBankDto>();
            foreach (var bankItem in tradingBankAccounts)
            {
                var businessCustomerBanks = _businessCustomerEFRepository.FindBankById(bankItem.BusinessCustomerBankAccId);
                if (businessCustomerBanks != null)
                {
                    result.Add(businessCustomerBanks);
                }
            }
            return result;
        }

        /// <summary>
        /// Chọn tài khoản chi tự động
        /// </summary>
        /// <param name="id"></param>
        /// <param name="tIdWithoutOtp"></param>
        /// <returns></returns>
        public GarnerDistributionTradingBankAccount ChooseAutoAccount(int id)
        {
            _logger.LogInformation($"{nameof(ChooseAutoAccount)}: id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var accountFind = (from account in _dbContext.GarnerDistributionTradingBankAccounts
                               join p in _dbContext.TradingMSBPrefixAccounts on account.BusinessCustomerBankAccId equals p.TradingBankAccountId
                               where (account.Id == id && account.Type == DistributionTradingBankAccountTypes.CHI && account.Deleted == YesNo.NO)
                               && (p.TradingProviderId == tradingProviderId)
                               select account).FirstOrDefault();

            if (accountFind == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerDistributionTradingBankAccNotFound);
            }

            var prefixMsb = (from account in _dbContext.GarnerDistributionTradingBankAccounts
                             join p in _dbContext.TradingMSBPrefixAccounts on account.BusinessCustomerBankAccId equals p.TradingBankAccountId
                             where account.Id == id && p.Deleted == YesNo.NO
                             select p).FirstOrDefault().ThrowIfNull(_dbContext, ErrorCode.CoreTradingMSBPrefixAccountNotFound);

            if (string.IsNullOrEmpty(prefixMsb.TIdWithoutOtp))
            {
                _defErrorEFRepository.ThrowException(ErrorCode.CoreTradingMSBPrefixAccountNotFound);
            }

            accountFind.IsAuto = YesNo.YES;

            var listAccount = _garnerDistributionTradingBankAccountRepository.Entity.Where(o => o.Id != id
                && o.Deleted == YesNo.NO && o.Type == DistributionTradingBankAccountTypes.CHI);
            foreach (var account in listAccount)
            {
                account.IsAuto = YesNo.NO;
            }


            _dbContext.SaveChanges();
            return accountFind;
        }

        public PagingResult<GarnerDistributionDto> FindAll(FilterGarnerDistributionDto input)
        {
            _logger.LogInformation($"{nameof(FindAll)}: input = {JsonSerializer.Serialize(input)}");

            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            int? tradingProviderId = null;
            if (usertype != UserTypes.EPIC && usertype != UserTypes.ROOT_EPIC)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }

            var resultPaging = new PagingResult<GarnerDistributionDto>();
            var result = new List<GarnerDistributionDto>();
            var distributionFind = _garnerDistributionEFRepository.FindAll(input, tradingProviderId);
            foreach (var item in distributionFind.Items)
            {
                var resultItem = new GarnerDistributionDto();
                resultItem = _mapper.Map<GarnerDistributionDto>(item);

                // Lấy thông tin phân phối sản phẩm cho đại lý
                var productTradingProvider = _garnerDistributionSharedServices.LimitCalculationTrading(item.ProductId, item.TradingProviderId);
                if (productTradingProvider != null)
                {
                    resultItem.HasTotalInvestmentSub = productTradingProvider.HasTotalInvestmentSub;
                    resultItem.TotalInvestmentSub = productTradingProvider.TotalInvestmentSub;
                    resultItem.Quantity = productTradingProvider.Quantity;
                }

                // Lấy thông tin sản phẩm tích lũy
                var productFind = _garnerProductEFRepository.FindById(item.ProductId);
                if (productFind != null)
                {
                    resultItem.GarnerProduct = _mapper.Map<GarnerProductDto>(productFind);
                    // Nếu là sản phẩm tích lũy Cổ Phần
                    if (productFind.ProductType == GarnerProductTypes.CO_PHAN)
                    {
                        // Lấy thông tin Tổ chức phát hành
                        resultItem.GarnerProduct.CpsIssuer = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.CpsIssuerId ?? 0));

                        // Lấy thông tin Đại lý lưu ký
                        resultItem.GarnerProduct.CpsDepositProvider = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.CpsDepositProviderId ?? 0));
                    }

                    // Nếu là sản phẩm tích lũy Bất Động sản
                    else if (productFind.ProductType == GarnerProductTypes.BAT_DONG_SAN)
                    {
                        //Lấy loại hình dự án
                        resultItem.GarnerProduct.InvProductTypes = _garnerProductTypeEFRepository.FindAllByProductId(productFind.Id).Select(g => g.Type).ToList();

                        // Lấy thông tin Chủ đầu tư
                        resultItem.GarnerProduct.InvOwner = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.InvOwnerId ?? 0));

                        // Lấy thông tin Tổng thầu
                        resultItem.GarnerProduct.InvGeneralContractor = _mapper.Map<BusinessCustomerDto>(_businessCustomerEFRepository.FindById(productFind.InvGeneralContractorId ?? 0));
                    }
                    resultItem.IsInvested = _garnerOrderEFRepository.SumValue(item.Id, null);
                }
                result.Add(resultItem);
            }
            resultPaging.TotalItems = distributionFind.TotalItems;
            resultPaging.Items = result;

            return resultPaging;
        }

        /// <summary> 
        /// Lấy thông tin phân phối sản phẩm để đặt lệnh CMS
        /// </summary>
        /// <returns></returns>
        public List<GarnerDistributionDto> FindAllDistribution(GarnerDistributionFilterDto input)
        {

            int? tradingProviderId = null;
            int? partnerId = null;
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                _dbContext.CheckTradingRelationshipPartner(partnerId, input.TradingProviderIds);
            }
            _logger.LogInformation($"{nameof(FindAllDistribution)}: tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            var result = new List<GarnerDistributionDto>();
            var distributionFind = _garnerDistributionEFRepository.FindAllDistribution(input, tradingProviderId);
            foreach (var item in distributionFind)
            {
                var resultItem = new GarnerDistributionDto();
                resultItem = _mapper.Map<GarnerDistributionDto>(item);

                // Lấy thông tin phân phối sản phẩm cho đại lý
                var productTradingProvider = _garnerDistributionSharedServices.LimitCalculationTrading(item.ProductId, item.TradingProviderId);
                if (productTradingProvider != null)
                {
                    resultItem.HasTotalInvestmentSub = productTradingProvider.HasTotalInvestmentSub;
                    resultItem.TotalInvestmentSub = productTradingProvider.TotalInvestmentSub;
                    resultItem.Quantity = productTradingProvider.Quantity;
                }

                // Lấy thông tin chính sách, và kỳ hạn để đặt lệnh trong trạng thái hoạt động
                resultItem.Policies = GetAllPolicy(item.Id, input).Where(p => p.Status == Status.ACTIVE && (p.PolicyDetails.Count == 0 || p.PolicyDetails.Any(d => d.Status == Status.ACTIVE))).ToList();

                //Thông tin của product
                var product = _garnerProductEFRepository.FindById(item.ProductId);

                resultItem.GarnerProduct = _mapper.Map<GarnerProductDto>(product);
                result.Add(resultItem);
            }

            return result;
        }
        #endregion

        #region Trình duyệt Phân phối sản phẩm
        /// <summary>
        /// Yêu cầu trình duyệt
        /// </summary>
        /// <param name="input"></param>
        public void Request(CreateGarnerRequestDto input)
        {
            _logger.LogInformation($"{nameof(Request)}: input = {JsonSerializer.Serialize(input)}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var actionType = ActionTypes.THEM_MOI;

            //Tìm kiếm thông tin Phân phối sản phẩm
            var distributionFind = _garnerDistributionEFRepository.FindById(input.Id);
            if (distributionFind == null)
            {
                throw new FaultException(new FaultReason($"{_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerDistributionNotFound)}"), new FaultCode(((int)ErrorCode.GarnerDistributionNotFound).ToString()), "");
            }

            // Status = 1 : trình duyệt với, Status = 4 trình duyệt lại sản phẩm
            if (distributionFind.Status == DistributionStatus.KHOI_TAO || distributionFind.Status == DistributionStatus.HUY_DUYET)
            {
                //Nếu đã tồn tại bản ghi trước đó
                var findRequest = _garnerApproveEFRepository.FindByIdOfDataType(input.Id, GarnerApproveDataTypes.GAN_DISTRIBUTION);
                if (findRequest != null)
                {
                    actionType = ActionTypes.CAP_NHAT;
                }

                var request = _garnerApproveEFRepository.Request(new GarnerApprove
                {
                    UserRequestId = userId,
                    UserApproveId = input.UserApproveId,
                    RequestNote = input.RequestNote,
                    ActionType = actionType,
                    DataType = GarnerApproveDataTypes.GAN_DISTRIBUTION,
                    ReferId = input.Id,
                    Summary = input.Summary,
                    CreatedBy = username,
                    TradingProviderId = tradingProviderId,
                    PartnerId = null
                });

                distributionFind.Status = DistributionStatus.CHO_DUYET;
                request.DataStatus = DistributionStatus.CHO_DUYET;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Duyệt Phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void Approve(GarnerApproveDto input)
        {
            _logger.LogInformation($"{nameof(Approve)}: input = {JsonSerializer.Serialize(input)}");

            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            //Tìm kiếm thông tin dự án
            var distributionFind = _garnerDistributionEFRepository.FindById(input.Id);
            if (distributionFind == null)
            {
                throw new FaultException(new FaultReason($"{_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerDistributionNotFound)}"), new FaultCode(((int)ErrorCode.GarnerDistributionNotFound).ToString()), "");
            }

            if (distributionFind.Status == DistributionStatus.CHO_DUYET)
            {
                var findRequest = _garnerApproveEFRepository.FindByIdOfDataType(input.Id, GarnerApproveDataTypes.GAN_DISTRIBUTION);
                if (findRequest == null)
                {
                    throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerApproveNotFound)), new FaultCode(((int)ErrorCode.GarnerDistributionNotFound).ToString()), "");
                }
                else
                {
                    _garnerApproveEFRepository.Approve(new GarnerApprove
                    {
                        Id = findRequest.Id,
                        ApproveNote = input.ApproveNote,
                        UserApproveId = userId,
                    });
                }
                distributionFind.Status = DistributionStatus.HOAT_DONG;
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Huy duyet san pham
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void Cancel(GarnerCancelDto input)
        {
            _logger.LogInformation($"{nameof(Cancel)}: input = {JsonSerializer.Serialize(input)}");

            //Tìm kiếm thông tin Phân phối sản phẩm
            var distributionFind = _garnerDistributionEFRepository.FindById(input.Id);
            if (distributionFind == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerDistributionNotFound)), new FaultCode(((int)ErrorCode.GarnerDistributionNotFound).ToString()), "");
            }

            var findRequest = _garnerApproveEFRepository.FindByIdOfDataType(input.Id, GarnerApproveDataTypes.GAN_DISTRIBUTION);
            if (findRequest == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerDistributionNotFound)), new FaultCode(((int)ErrorCode.GarnerDistributionNotFound).ToString()), "");
            }
            else
            {
                _garnerApproveEFRepository.Cancel(new GarnerApprove
                {
                    Id = findRequest.Id,
                    CancelNote = input.CancelNote
                });
            }
            //Cập nhật trạng thái cho Phân phối sản phẩm
            distributionFind.Status = DistributionStatus.HUY_DUYET;

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Kiểm tra Phân phối sản phẩm
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void Check(GarnerCheckDto input)
        {
            _logger.LogInformation($"{nameof(Check)}: input = {JsonSerializer.Serialize(input)}");

            var userId = CommonUtils.GetCurrentUserId(_httpContext);

            //Tìm kiếm thông tin phân phối sản phẩm
            var distributionFind = _garnerDistributionEFRepository.FindById(input.Id);
            if (distributionFind == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerDistributionNotFound)), new FaultCode(((int)ErrorCode.GarnerDistributionNotFound).ToString()), "");
            }

            var findRequest = _garnerApproveEFRepository.FindByIdOfDataType(input.Id, GarnerApproveDataTypes.GAN_DISTRIBUTION);
            if (findRequest == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerDistributionNotFound)), new FaultCode(((int)ErrorCode.GarnerDistributionNotFound).ToString()), "");
            }
            else
            {
                _garnerApproveEFRepository.Cancel(new GarnerApprove
                {
                    Id = findRequest.Id,
                    UserCheckId = userId
                });
            }
            distributionFind.IsCheck = YesNo.YES;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// EPIC, ROOT_EPIC Xét phân phối sản phẩm làm mặc định 
        /// </summary>
        public void SetDefault(int distributionId)

        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC)
            {
                var distributionFind = _garnerDistributionEFRepository.FindById(distributionId).ThrowIfNull<GarnerDistribution>(_dbContext, ErrorCode.GarnerDistributionNotFound);

                var distributionDefaultFind = _garnerDistributionEFRepository.Entity.Where(d => d.IsDefault == YesNo.YES && d.Deleted == YesNo.NO);
                foreach (var item in distributionDefaultFind)
                {
                    item.IsDefault = YesNo.NO;
                }

                distributionFind.IsDefault = YesNo.YES;

                _dbContext.SaveChanges();
            }
        }
        #endregion

        #region Chính sách phân phôi sản phẩm
        /// <summary>
        /// Thêm chính sách bán theo kỳ hạn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public GarnerPolicyMoreInfoDto AddPolicy(CreatePolicyDto input)
        {
            _logger.LogInformation($"{nameof(AddPolicy)}: input = {JsonSerializer.Serialize(input)}");

            //Lấy thông tin đại lý
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var policyInsert = _mapper.Map<GarnerPolicy>(input);
            policyInsert.DistributionId = input.DistributionId;
            var policy = _garnerPolicyEFRepository.Add(policyInsert, tradingProviderId, username);

            var result = _mapper.Map<GarnerPolicyMoreInfoDto>(policy);

            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                RealTableId = policy.Id,
                Action = ActionTypes.THEM_MOI,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_ADD_POLICY
            }, username);

            if (input.PolicyTempId != null)
            {
                //Lấy thông tin kỳ hạn mẫu
                var policyDetailTemps = _garnerPolicyDetailTempEFRepository.FindAll(input.PolicyTempId ?? 0, tradingProviderId);
                //Lưu kỳ hạn 
                foreach (var temp in policyDetailTemps)
                {
                    var policyDetailInsert = _mapper.Map<GarnerPolicyDetail>(temp);
                    policyDetailInsert.DistributionId = input.DistributionId;
                    policyDetailInsert.PolicyId = policy.Id;
                    var policyDetail = _garnerPolicyDetailEFRepository.Add(policyDetailInsert, tradingProviderId, username);

                    _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                    {
                        UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY_DETAIL,
                        RealTableId = policyDetail.Id,
                        Action = ActionTypes.THEM_MOI,
                        Summary = GarnerHistoryUpdateSummary.SUMMARY_ADD_POLICY_DETAIL
                    }, username);
                }
                result.PolicyDetailTemps = _mapper.Map<List<GarnerPolicyDetailTempDto>>(policyDetailTemps);

                //Lấy thông tin mẫu hợp đồng mẫu
                //var contractTemplateTemps = _garnerContractTemplateTempEFRepository.FindAll(input.PolicyTempId ?? 0, tradingProviderId);
                //foreach (var item in contractTemplateTemps)
                //{
                //    var contractTemplate = _mapper.Map<GarnerContractTemplate>(item);
                //    contractTemplate.Id = policy.Id;
                //    _garnerContractTemplateEFRepository.Add(contractTemplate, tradingProviderId, username);
                //}
                //result.ContractTemplateTemps = _mapper.Map<List<GarnerContractTemplateTempDto>>(contractTemplateTemps);
            }
            _dbContext.SaveChanges();
            return result;
        }

        /// <summary>
        /// Update chính sách bán theo kỳ hạn
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="NotImplementedException"></exception>
        public void UpdatePolicy(UpdatePolicyDto input)
        {
            _logger.LogInformation($"{nameof(UpdatePolicy)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var getPolicy = _garnerPolicyEFRepository.Entity.FirstOrDefault(p => p.Id == input.Id && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO);
            if (getPolicy == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerProductNotFound)), new FaultCode(((int)ErrorCode.GarnerProductNotFound).ToString()), "");
            }
            var updatePolicy = _mapper.Map<GarnerPolicy>(input);
            updatePolicy.Id = input.Id;

            #region Lịch sử thay đổi

            //Mã chính sách 
            if (getPolicy.Code != input.Code)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.Code,
                    NewValue = input.Code,
                    FieldName = GarnerFieldName.UPDATE_CODE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CODE
                }, username);
            }

            //Tên chính sách
            if (getPolicy.Name != input.Name)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.Name,
                    NewValue = input.Name,
                    FieldName = GarnerFieldName.UPDATE_NAME,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_NAME
                }, username);
            }

            //Số tiền tích lũy tối thiểu
            if (getPolicy.MinMoney != input.MinMoney)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.MinMoney.ToString(),
                    NewValue = input.MinMoney.ToString(),
                    FieldName = GarnerFieldName.UPDATE_MIN_MONEY,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_MIN_MONEY
                }, username);
            }

            //Số tiền tích lũy tối đa
            if (getPolicy.MaxMoney != input.MaxMoney)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.MaxMoney.ToString(),
                    NewValue = input.MaxMoney.ToString(),
                    FieldName = GarnerFieldName.UPDATE_MAX_MONEY,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_MAX_MONEY
                }, username);
            }

            //Số ngày đầu tư tối thiểu
            if (getPolicy.MinInvestDay != input.MinInvestDay)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.MinInvestDay.ToString(),
                    NewValue = input.MinInvestDay.ToString(),
                    FieldName = GarnerFieldName.UPDATE_MIN_INVEST_DAY,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_MIN_INVEST_DAY
                }, username);
            }

            //Thuế lợi nhuận
            if (getPolicy.IncomeTax != input.IncomeTax)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.IncomeTax.ToString(),
                    NewValue = input.IncomeTax.ToString(),
                    FieldName = GarnerFieldName.UPDATE_INCOME_TAX,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_INCOME_TAX
                }, username);
            }

            //Loại nhà đầu tư
            if (getPolicy.InvestorType != input.InvestorType)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.InvestorType,
                    NewValue = input.InvestorType,
                    FieldName = GarnerFieldName.UPDATE_INVESTOR_TYPE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_INVESTOR_TYPE
                }, username);
            }

            //Phân loại chính sách sản phẩm
            if (getPolicy.Classify != input.Classify)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.Classify.ToString(),
                    NewValue = input.Classify.ToString(),
                    FieldName = GarnerFieldName.UPDATE_CLASSIFY,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CLASSIFY
                }, username);
            }

            //Phân loại chính sách sản phẩm
            if (getPolicy.Classify != input.Classify)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.Classify.ToString(),
                    NewValue = input.Classify.ToString(),
                    FieldName = GarnerFieldName.UPDATE_CLASSIFY,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CLASSIFY
                }, username);
            }

            //Loại hình kỳ hạn
            if (getPolicy.GarnerType != input.GarnerType)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.GarnerType.ToString(),
                    NewValue = input.GarnerType.ToString(),
                    FieldName = GarnerFieldName.UPDATE_GARNER_TYPE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_GARNER_TYPE
                }, username);
            }

            //Loại hình kỳ hạn
            if (getPolicy.GarnerType != input.GarnerType)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.GarnerType.ToString(),
                    NewValue = input.GarnerType.ToString(),
                    FieldName = GarnerFieldName.UPDATE_GARNER_TYPE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_GARNER_TYPE
                }, username);
            }

            //Kiểu trả lợi tức
            if (getPolicy.InterestType != input.InterestType)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.InterestType.ToString(),
                    NewValue = input.InterestType.ToString(),
                    FieldName = GarnerFieldName.UPDATE_INTEREST_TYPE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_INTEREST_TYPE
                }, username);
            }

            //loại hình lợi tức
            if (getPolicy.CalculateType != input.CalculateType)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.CalculateType.ToString(),
                    NewValue = input.CalculateType.ToString(),
                    FieldName = GarnerFieldName.UPDATE_INTEREST_TYPE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_INTEREST_TYPE
                }, username);
            }

            //Thứ tự rút tiền
            if (getPolicy.OrderOfWithdrawal != input.OrderOfWithdrawal)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.OrderOfWithdrawal.ToString(),
                    NewValue = input.OrderOfWithdrawal.ToString(),
                    FieldName = GarnerFieldName.UPDATE_ORDER_OF_WITHDRAWAL,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_ORDER_OF_WITHDRAWAL
                }, username);
            }

            //Số tiền rút tối thiểu
            if (getPolicy.MinWithdraw != input.MinWithdraw)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.MinWithdraw.ToString(),
                    NewValue = input.MinWithdraw.ToString(),
                    FieldName = GarnerFieldName.UPDATE_MIN_WITHDRAWAL,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_MIN_WITHDRAWAL
                }, username);
            }

            //Số tiền rút tối đa
            if (getPolicy.MaxWithdraw != input.MaxWithdraw)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.MaxWithdraw.ToString(),
                    NewValue = input.MaxWithdraw.ToString(),
                    FieldName = GarnerFieldName.UPDATE_MAX_WITHDRAWAL,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_MAX_WITHDRAWAL
                }, username);
            }

            //Phí rút tiền
            if (getPolicy.WithdrawFee != input.WithdrawFee)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.WithdrawFee.ToString(),
                    NewValue = input.WithdrawFee.ToString(),
                    FieldName = GarnerFieldName.UPDATE_WITHDRAWAL_FEE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_WITHDRAWAL_FEE
                }, username);
            }

            //Kiểu tính phí rút vốn
            if (getPolicy.WithdrawFeeType != input.WithdrawFeeType)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.WithdrawFeeType.ToString(),
                    NewValue = input.WithdrawFeeType.ToString(),
                    FieldName = GarnerFieldName.UPDATE_WITHDRAWAL_FEE_TYPE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_WITHDRAWAL_FEE_TYPE
                }, username);
            }

            //Chuyển đổi tài sản
            if (getPolicy.IsTransferAssets != input.IsTransferAssets)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.IsTransferAssets,
                    NewValue = input.IsTransferAssets,
                    FieldName = GarnerFieldName.UPDATE_IS_TRANSFER_ASSETS,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_IS_TRANSFER_ASSETS
                }, username);
            }

            //Phí chuyển đổi tài sản
            if (getPolicy.TransferAssetsFee != input.TransferAssetsFee)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.TransferAssetsFee.ToString(),
                    NewValue = input.TransferAssetsFee.ToString(),
                    FieldName = GarnerFieldName.UPDATE_TRANSFER_ASSETS_FEE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_TRANSFER_ASSETS_FEE
                }, username);
            }

            //Thứ tự hiển thị
            if (getPolicy.SortOrder != input.SortOrder)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.SortOrder.ToString(),
                    NewValue = input.SortOrder.ToString(),
                    FieldName = GarnerFieldName.UPDATE_SORT_ORDER,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_SORT_ORDER
                }, username);
            }

            //Ngày bắt đầu
            if (getPolicy.StartDate != input.StartDate)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.StartDate.ToString(),
                    NewValue = input.StartDate.ToString(),
                    FieldName = GarnerFieldName.UPDATE_START_DATE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_START_DATE
                }, username);
            }

            //Ngày kết thúc
            if (getPolicy.EndDate != input.EndDate)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.EndDate.ToString(),
                    NewValue = input.EndDate.ToString(),
                    FieldName = GarnerFieldName.UPDATE_END_DATE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_END_DATE
                }, username);
            }

            //Mô tả
            if (getPolicy.Description != input.Description)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                    RealTableId = input.Id,
                    OldValue = getPolicy.Description,
                    NewValue = input.Description,
                    FieldName = GarnerFieldName.UPDATE_DESCRIPTION,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_DESCRIPTION
                }, username);
            }
            #endregion
            //Update
            _garnerPolicyEFRepository.Update(updatePolicy, tradingProviderId, username);

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xoá chính sách
        /// </summary>
        /// <param name="policyId"></param>
        public void DeletePolicy(int policyId)
        {
            _logger.LogInformation($"{nameof(DeletePolicy)}: PolicyId: {policyId}");

            //Lấy thông tin 
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var getPolicy = _garnerPolicyEFRepository.Entity.FirstOrDefault(x => x.Id == policyId && x.TradingProviderId == tradingProviderId).ThrowIfNull<GarnerPolicy>(_dbContext, ErrorCode.GarnerPolicyNotFound);

            var getOrder = _garnerOrderEFRepository.Entity.Any(e => e.PolicyId == policyId && e.Deleted == YesNo.NO);
            if (getOrder)
            {
                _garnerDistributionEFRepository.ThrowException(ErrorCode.GarnerOrderNotFound);
            }

            getPolicy.Deleted = YesNo.YES;
            getPolicy.ModifiedBy = username;
            getPolicy.ModifiedDate = DateTime.Now;

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                RealTableId = policyId,
                Action = ActionTypes.XOA,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_CODE
            }, username);
            _dbContext.SaveChanges();
        }


        /// <summary>
        /// Thêm kỳ hạn
        /// </summary>
        /// <param name="entity"></param>
        public GarnerPolicyDetailDto AddPolicyDetail(CreatePolicyDetailDto input)
        {
            _logger.LogInformation($"{nameof(AddPolicyDetail)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var policy = _garnerPolicyEFRepository.Entity.FirstOrDefault(e => e.Id == input.PolicyId && e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO);
            if (policy != null)
            {
                //if (policy.InterestType == EPIC.Utils.InterestTypes.NGAY_CO_DINH)
                //{
                //    if (input.RepeatFixedDate == null || input.InterestPeriodType == null)
                //    {
                //        throw new FaultException(new FaultReason($" Kỳ hạn {input.Name} không được bỏ trống số ngày chi trả cố định"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                //    }
                //}
                /*if (policy.InterestType == EPIC.Utils.InterestTypes.NGAY_CO_DINH)
                {
                    if (input.RepeatFixedDate == null *//*|| input.InterestPeriodType == null*//*)
                    {
                        throw new FaultException(new FaultReason($" Kỳ hạn {input.Name} không được bỏ trống số ngày chi trả cố định"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }
                }
                else if (policy.InterestType == EPIC.Utils.InterestTypes.DINH_KY)
                {
                    if (input.InterestPeriodQuantity == null || input.InterestPeriodType == null)
                    {
                        throw new FaultException(new FaultReason($"Kỳ hạn {input.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }
                };*/
            }
            var policyDetailInsert = _mapper.Map<GarnerPolicyDetail>(input);
            policyDetailInsert.PolicyId = policy.Id;
            policyDetailInsert.DistributionId = policy.DistributionId;
            policyDetailInsert = _garnerPolicyDetailEFRepository.Add(policyDetailInsert, tradingProviderId, username);

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY_DETAIL,
                RealTableId = policyDetailInsert.Id,
                Action = ActionTypes.THEM_MOI,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_ADD_POLICY_DETAIL
            }, username);

            _dbContext.SaveChanges();
            return _mapper.Map<GarnerPolicyDetailDto>(policyDetailInsert);
        }

        /// <summary>
        /// Update kỳ hạn
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void UpdatePolicyDetail(UpdatePolicyDetailDto input)
        {
            _logger.LogInformation($"{nameof(UpdatePolicyDetail)}: input = {JsonSerializer.Serialize(input)}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var getPolicyDetail = _garnerPolicyDetailEFRepository.Entity.FirstOrDefault(e => e.Id == input.Id && e.PolicyId == input.PolicyId && e.TradingProviderId == input.TradingProviderId && e.Deleted == YesNo.NO);
            if (getPolicyDetail == null)
            {
                _garnerPolicyDetailEFRepository.ThrowException(ErrorCode.GarnerPolicyDetailNotFound);
            }

            var updatePolicydetail = _mapper.Map<GarnerPolicyDetail>(input);
            updatePolicydetail.Id = input.Id;

            #region Lịch sử thay đổi
            //Số thứ tự
            if (getPolicyDetail.SortOrder != input.SortOrder)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY_DETAIL,
                    RealTableId = input.Id,
                    OldValue = getPolicyDetail.SortOrder.ToString(),
                    NewValue = input.SortOrder.ToString(),
                    FieldName = GarnerFieldName.UPDATE_SORT_ORDER,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_POLICY_DETAIL_SORT_ORDER
                }, username);
            }

            //Số kỳ đầu tư
            if (getPolicyDetail.PeriodQuantity != input.PeriodQuantity)
            {
                string oldQuantity = getPolicyDetail.PeriodQuantity + "";
                string oldValue = getPolicyDetail.PeriodType == PeriodType.NGAY ? oldQuantity + PeriodDisplay.NGAY
                                : getPolicyDetail.PeriodType == PeriodType.THANG ? oldQuantity + PeriodDisplay.THANG : oldQuantity + PeriodDisplay.NAM;

                string newQuantity = input.PeriodQuantity + "";
                string newValue = input.PeriodType == PeriodType.NGAY ? newQuantity + PeriodDisplay.NGAY
                                : input.PeriodType == PeriodType.THANG ? newQuantity + PeriodDisplay.THANG : newQuantity + PeriodDisplay.NAM;

                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY_DETAIL,
                    RealTableId = input.Id,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = GarnerFieldName.UPDATE_PERIOD_QUANTITY,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_POLICY_DETAIL_PERIOD_QUANTITY
                }, username);
            }

            //Số kỳ đầu tư
            if (getPolicyDetail.PeriodType != input.PeriodType)
            {
                string oldQuantity = getPolicyDetail.PeriodQuantity + "";
                string oldValue = getPolicyDetail.PeriodType == PeriodType.NGAY ? oldQuantity + PeriodDisplay.NGAY
                                : getPolicyDetail.PeriodType == PeriodType.THANG ? oldQuantity + PeriodDisplay.THANG : oldQuantity + PeriodDisplay.NAM;

                string newQuantity = input.PeriodQuantity + "";
                string newValue = input.PeriodType == PeriodType.NGAY ? newQuantity + PeriodDisplay.NGAY
                                : input.PeriodType == PeriodType.THANG ? newQuantity + PeriodDisplay.THANG : newQuantity + PeriodDisplay.NAM;

                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY_DETAIL,
                    RealTableId = input.Id,
                    OldValue = oldValue,
                    NewValue = newValue,
                    FieldName = GarnerFieldName.UPDATE_PERIOD_TYPE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_POLICY_DETAIL_PERIOD_QUANTITY
                }, username);
            }

            //Tên viết tắt
            if (getPolicyDetail.ShortName != input.ShortName)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY_DETAIL,
                    RealTableId = input.Id,
                    OldValue = getPolicyDetail.ShortName,
                    NewValue = input.ShortName,
                    FieldName = GarnerFieldName.UPDATE_SHORT_NAME,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_SHORT_NAME
                }, username);
            }

            //Tên kỳ hạn
            if (getPolicyDetail.Name != input.Name)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY_DETAIL,
                    RealTableId = input.Id,
                    OldValue = getPolicyDetail.Name,
                    NewValue = input.Name,
                    FieldName = GarnerFieldName.UPDATE_NAME,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_POLICY_DETAIL_NAME
                }, username);
            }

            //Lợi nhuận
            if (getPolicyDetail.Profit != input.Profit)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY_DETAIL,
                    RealTableId = input.Id,
                    OldValue = getPolicyDetail.Profit.ToString(),
                    NewValue = input.Profit.ToString(),
                    FieldName = GarnerFieldName.UPDATE_NAME,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_POLICY_DETAIL_NAME
                }, username);
            }

            //Số ngày đầu tư
            if (getPolicyDetail.InterestDays != input.InterestDays)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY_DETAIL,
                    RealTableId = input.Id,
                    OldValue = getPolicyDetail.InterestDays.ToString(),
                    NewValue = input.InterestDays.ToString(),
                    FieldName = GarnerFieldName.UPDATE_INTEREST_DAYS,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_INTEREST_DAYS
                }, username);
            }

            //Kiểu trả lợi tức
            if (getPolicyDetail.InterestType != input.InterestType)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY_DETAIL,
                    RealTableId = input.Id,
                    OldValue = getPolicyDetail.InterestType.ToString(),
                    NewValue = input.InterestType.ToString(),
                    FieldName = GarnerFieldName.UPDATE_INTEREST_TYPE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_INTEREST_TYPE
                }, username);
            }
            #endregion

            var update = _garnerPolicyDetailEFRepository.UpdatePolicyDetail(updatePolicydetail, username, tradingProviderId);

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xóa kỳ hạn
        /// </summary>
        /// <param name="policyDetailId"></param>
        public void DeletePolicyDetail(int policyDetailId)
        {
            _logger.LogInformation($"{nameof(DeletePolicyDetail)}: PolicyDetailId = {policyDetailId}");

            //Lấy thông tin 
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var getPolicyDetail = _garnerPolicyDetailEFRepository.Entity.FirstOrDefault(x => x.Id == policyDetailId && x.TradingProviderId == tradingProviderId)
                .ThrowIfNull<GarnerPolicyDetail>(_dbContext, ErrorCode.GarnerPolicyDetailNotFound);

            var getOrder = _garnerOrderEFRepository.Entity.Any(e => e.PolicyDetailId == policyDetailId && e.Deleted == YesNo.NO);
            if (getOrder)
            {
                _garnerDistributionEFRepository.ThrowException(ErrorCode.GarnerPolicyDetailIsUsingInOrder);
            }

            getPolicyDetail.Deleted = YesNo.YES;
            getPolicyDetail.ModifiedBy = username;
            getPolicyDetail.ModifiedDate = DateTime.Now;

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY_DETAIL,
                RealTableId = policyDetailId,
                Action = ActionTypes.XOA,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_DELETE_POLICY_DETAIL
            }, username);

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Thay đổi trạng thái chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <exception cref="FaultException"></exception>
        public int UpdateStatusPolicy(int policyId)
        {
            _logger.LogInformation($"{nameof(UpdateStatusPolicy)}: policyId = {policyId}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var getPolicy = _garnerPolicyEFRepository.FindById(policyId, tradingProviderId);
            if (getPolicy == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerPolicyNotFound)), new FaultCode(((int)ErrorCode.GarnerPolicyNotFound).ToString()), "");
            }

            string oldValue;
            string newValue;

            if (getPolicy.Status == Status.ACTIVE)
            {
                oldValue = StatusText.ACTIVE;
                getPolicy.Status = Status.INACTIVE;
                newValue = StatusText.DEACTIVE;
            }
            else
            {
                oldValue = StatusText.DEACTIVE;
                getPolicy.Status = Status.ACTIVE;
                newValue = StatusText.ACTIVE;
            }

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                RealTableId = policyId,
                OldValue = oldValue,
                NewValue = newValue,
                FieldName = GarnerFieldName.UPDATE_STATUS,
                Action = ActionTypes.CAP_NHAT,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_STATUS_POLICY
            }, username);

            getPolicy.ModifiedBy = username;
            getPolicy.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return policyId;
        }

        /// <summary>
        /// Thay đổi trạng thái kỳ hạn
        /// </summary>
        /// <param name="policyDetailId"></param>
        /// <exception cref="NotImplementedException"></exception>
        public int UpdateStatusPolicyDetail(int policyDetailId)
        {
            _logger.LogInformation($"{nameof(UpdateStatusPolicyDetail)}: policyDetailId = {policyDetailId}");

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var getPolicyDetail = _garnerPolicyDetailEFRepository.FindById(policyDetailId, tradingProviderId);
            if (getPolicyDetail == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerPolicyDetailNotFound)), new FaultCode(((int)ErrorCode.GarnerPolicyDetailNotFound).ToString()), "");
            }

            string oldValue;
            string newValue;

            if (getPolicyDetail.Status == Status.ACTIVE)
            {
                oldValue = StatusText.ACTIVE;
                getPolicyDetail.Status = Status.INACTIVE;
                newValue = StatusText.DEACTIVE;
            }
            else
            {
                oldValue = StatusText.DEACTIVE;
                getPolicyDetail.Status = Status.ACTIVE;
                newValue = StatusText.ACTIVE;
            }

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_POLICY,
                RealTableId = policyDetailId,
                OldValue = oldValue,
                NewValue = newValue,
                FieldName = GarnerFieldName.UPDATE_STATUS,
                Action = ActionTypes.CAP_NHAT,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_STATUS_POLICY
            }, username);

            getPolicyDetail.ModifiedBy = username;
            getPolicyDetail.ModifiedDate = DateTime.Now;
            _dbContext.SaveChanges();
            return policyDetailId;
        }

        /// <summary>
        /// Lấy danh sách chính sách và kỳ hạn theo id chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public GarnerPolicyMoreInfoDto FindPolicyAndPolicyDetailByPolicyId(int policyId)
        {
            _logger.LogInformation($"{nameof(FindPolicyAndPolicyDetailByPolicyId)}: policyId = {policyId}");

            //Lấy thông tin đại lý
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            //Lấy thông tin chính sách
            var policy = _garnerPolicyEFRepository.FindById(policyId, tradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {policyId}"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            var result = _mapper.Map<GarnerPolicyMoreInfoDto>(policy);
            result.PolicyDetails = new List<GarnerPolicyDetailDto>();

            //Lấy thông tin kỳ hạn theo chính sách
            var policyDetailList = _garnerPolicyDetailEFRepository.GetAllPolicyDetailByPolicyId(policy.Id, tradingProviderId);
            foreach (var detailItem in policyDetailList)
            {
                var detail = _mapper.Map<GarnerPolicyDetailDto>(detailItem);
                result.PolicyDetails.Add(detail);
            }

            //Lấy thông tin mẫu hợp đồng
            var contractTemplates = _garnerContractTemplateEFRepository.FindAll(policyId);
            result.ContractTemplates = _mapper.Map<List<GarnerContractTemplateDto>>(contractTemplates);

            var distributionFind = _garnerDistributionEFRepository.FindById(policy.DistributionId, tradingProviderId);
            var distribution = _mapper.Map<GarnerDistributionDto>(distributionFind);
            result.Distribution = distribution;
            return result;
        }
        /// <summary>
        /// Lấy danh sách và kỳ hạn theo id chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public List<GarnerPolicyDetailDto> FindPolicyDetailByPolicyId(int policyId)
        {
            _logger.LogInformation($"{nameof(FindPolicyDetailByPolicyId)}: policyId = {policyId}");

            //Lấy thông tin đại lý
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            //Lấy thông tin chính sách
            var policy = _garnerPolicyEFRepository.FindById(policyId, tradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {policyId}"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            var result = new List<GarnerPolicyDetailDto>();

            //Lấy thông tin kỳ hạn theo chính sách
            var policyDetailList = _garnerPolicyDetailEFRepository.GetAllPolicyDetailByPolicyId(policy.Id, tradingProviderId);
            foreach (var detailItem in policyDetailList)
            {
                var detail = _mapper.Map<GarnerPolicyDetailDto>(detailItem);
                result.Add(detail);
            }
            return result;
        }

        /// <summary>
        /// Lấy ra danh sách chính sách và kỳ hạn theo Distribution
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<GarnerPolicyMoreInfoDto> GetAllPolicy(int distributionId, GarnerDistributionFilterDto input)
        {
            _logger.LogInformation($"{nameof(GetAllPolicy)}: distributionId = {distributionId}");
            //Lấy thông tin đại lý
            //var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            int? tradingProviderId = null;
            int? partnerId = null;
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                _dbContext.CheckTradingRelationshipPartner(partnerId, input.TradingProviderIds);
            }
            List<GarnerPolicyMoreInfoDto> result = new();

            //Lấy danh sách chính sách theo DistributionId 
            var getAllPolicy = _garnerPolicyEFRepository.GetAllPolicyByDistributionId(distributionId, input, tradingProviderId);
            foreach (var policyItem in getAllPolicy)
            {
                var policy = _mapper.Map<GarnerPolicyMoreInfoDto>(policyItem);

                //Lấy ra danh sách kỳ hạn theo chính sách
                policy.PolicyDetails = new List<GarnerPolicyDetailDto>();
                var getAllPolicyDetail = _garnerPolicyDetailEFRepository.GetAllPolicyDetailByPolicyId(policy.Id, policy.TradingProviderId);
                foreach (var policyDetailItem in getAllPolicyDetail)
                {
                    var policyDetail = _mapper.Map<GarnerPolicyDetailDto>(policyDetailItem);
                    policy.PolicyDetails.Add(policyDetail);
                }
                result.Add(policy);
            }
            return result.OrderByDescending(e => e.Id).ToList();
        }

        public List<GarnerPolicyDetailDto> GetByPolicyId(int policyId)
        {
            _logger.LogInformation($"{nameof(GetByPolicyId)}: policyId = {policyId}");

            //Lấy thông tin đại lý
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            //Lấy danh sách chính sách theo DistributionId 
            var getPolicyDetails = _garnerPolicyDetailEFRepository.GetAllPolicyDetailByPolicyId(policyId, tradingProviderId);

            return _mapper.Map<List<GarnerPolicyDetailDto>>(getPolicyDetails);
        }
        #endregion

        #region Tổng quan sản phẩm tích lũy
        /// <summary>
        /// Update tổng quan sản phẩm tích lũy
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public void UpdateProductOverview(UpdateGarnerProductOverviewDto input)
        {
            _logger.LogInformation($"{nameof(UpdateProductOverview)}: input = {JsonSerializer.Serialize(input)}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            //Tìm kiếm thông tin phân phối sản phẩm
            var distributionFind = _garnerDistributionEFRepository.FindById(input.Id);
            if (distributionFind == null)
            {
                throw new FaultException(new FaultReason(_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerDistributionNotFound)), new FaultCode(((int)ErrorCode.GarnerDistributionNotFound).ToString()), "");
            }

            //Cập nhật thông tin của tổng quan
            _garnerDistributionEFRepository.UpdateProductOverview(_mapper.Map<GarnerDistribution>(input), tradingProviderId, username);

            //Cập nhật thông tin File
            _garnerProductOverviewFileEFRepository.UpdateProductOverviewFile(input.Id, input.ProductOverviewFiles, tradingProviderId, username);

            //Cập nhật thông tin tổ chức
            _garnerProductOverviewOrgEFRepository.UpdateProductOverviewOrg(input.Id, input.ProductOverviewOrgs, tradingProviderId, username);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Lấy thông tin tổng quan của dự án
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public GarnerProductOverviewDto FindProductOverview(int id)
        {
            _logger.LogInformation($"{nameof(FindProductOverview)}: Id = {id}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var result = new GarnerProductOverviewDto();
            var distributionFind = _garnerDistributionEFRepository.FindById(id);
            if (distributionFind == null)
            {
                throw new FaultException(new FaultReason($"{_defErrorEFRepository.GetDefError((int)ErrorCode.GarnerDistributionNotFound)}"), new FaultCode(((int)ErrorCode.GarnerDistributionNotFound).ToString()), "");
            }
            result = _mapper.Map<GarnerProductOverviewDto>(distributionFind);
            result.ProductOverviewFiles = _mapper.Map<List<GarnerProductOverviewFileDto>>(_garnerProductOverviewFileEFRepository.FindAllListByDistribution(id, tradingProviderId));
            result.ProductOverviewOrgs = _mapper.Map<List<GarnerProductOverviewOrgDto>>(_garnerProductOverviewOrgEFRepository.FindAllListByDistribution(id, tradingProviderId));
            return result;
        }
        #endregion

        #region Services cho App

        /// <summary>
        /// Lấy danh sách sản phẩm tích lũy cho App
        /// </summary>
        /// <param name="keyword"></param>
        /// <returns></returns>
        public List<AppGarnerDistributionDto> AppDistributionGetAll(string keyword, bool isSaleView)
        {
            int? investorId = null;
            try
            {
                investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            }
            catch
            {
                investorId = null;
            }

            _logger.LogInformation($"{nameof(AppDistributionGetAll)}: keyword = {keyword}, investorId = {investorId}, isSaleView = {isSaleView}");

            var listTradingProviderIds = _logicInvestorTradingSharedServices.FindListTradingProviderForApp(investorId, isSaleView);
            // Lấy danh sách bảng hàng 
            var result = AppFilterDistribution(keyword, listTradingProviderIds);
            return result;
        }

        /// <summary>
        /// Lọc danh sách bảng hàng
        /// </summary>
        /// <returns></returns>
        public List<AppGarnerDistributionDto> AppFilterDistribution(string keyword, List<int> listTradingProviderIds = null)
        {
            _logger.LogInformation($"{nameof(AppFilterDistribution)}: keyword = {keyword}, listTradingProviderIds = {JsonSerializer.Serialize(listTradingProviderIds)}");

            var policyDefault = new AppGarnerDistributionDto(); // Chính sách nổi bật
            var policyDefaultFind = new GarnerPolicy(); // Tìm chính sách mặc định được đại lý xét

            // Lấy danh sách bảng hàng 
            var distributionFind = _garnerDistributionEFRepository.AppDistributionGetAll(keyword);

            //Nếu có danh sách đại lý mà investor đang thuộc thì lọc bảng hàng
            if (listTradingProviderIds != null && listTradingProviderIds.Count > 0)
            {
                //Lọc bảng hàng mà investor đã thuộc đại lý trong đấy
                distributionFind = distributionFind.Where(o => listTradingProviderIds.Contains(o.TradingProviderId));
            }

            // Lấy tạm chính sách đầu tiên
            policyDefault = distributionFind.FirstOrDefault();

            // Lấy đại lý trong phân phối mặc định mà EPIC xét trong phân phối chung
            var distributionDefault = _garnerDistributionEFRepository.Entity.FirstOrDefault(d => d.IsDefault == YesNo.YES && d.IsCheck == YesNo.YES && d.IsClose == YesNo.NO
                                                                    && d.Deleted == YesNo.NO && d.IsShowApp == YesNo.YES
                                                                    && d.Status == DistributionStatus.HOAT_DONG);
            // Tìm chính sách mặc định mà do đại lý cài cho sản phẩm phân phối
            if (distributionDefault != null)
            {
                policyDefaultFind = _garnerPolicyEFRepository.FindPolicyDefault(distributionDefault.TradingProviderId);
            }

            // Xét chính sách nổi bật
            // Nếu đại lý mặc định có trong danh sách đại lý lọc

            if (distributionDefault != null && distributionFind.Select(d => d.TradingProviderId).Contains(distributionDefault.TradingProviderId))
            {
                //Nếu chính sách mặc định của đại lý mặc định có trong danh sách bảng hàng
                if (policyDefaultFind != null && distributionFind.Select(d => d.PolicyId).Contains(policyDefaultFind.Id))
                {
                    policyDefault = distributionFind.FirstOrDefault(d => d.PolicyId == policyDefaultFind.Id);
                }
                // Nếu không thì lấy chính sách đầu tiên trong bảng hàng của đại lý mặc định
                else
                {
                    policyDefault = distributionFind.FirstOrDefault(d => d.TradingProviderId == distributionDefault.TradingProviderId);
                }
            }

            var result = new List<AppGarnerDistributionDto>();
            foreach (var item in distributionFind)
            {
                var resultItem = new AppGarnerDistributionDto();
                var policyFind = _garnerPolicyEFRepository.FindById(item.PolicyId);
                resultItem = _mapper.Map<AppGarnerDistributionDto>(policyFind);

                // Hiển thị Chu kỳ nhận lợi thức
                if (policyFind.GarnerType == GarnerPolicyTypes.KHONG_CHON_KY_HAN)
                {
                    resultItem.InterestTypeName = InterestTypes.InterestTypeNames(policyFind.InterestType, policyFind.RepeatFixedDate);
                }
                resultItem.PolicyId = policyFind.Id;
                resultItem.ProductType = item.ProductType;
                resultItem.ProductTypeName = DictionaryNames.ProductTypeName(item.ProductType);
                resultItem.DistributionId = item.DistributionId;
                resultItem.Profit = item.Profit;
                resultItem.ProductName = item.ProductName;
                resultItem.PolicyName = policyFind.Name;
                resultItem.TradingProviderName = item.TradingProviderName;
                resultItem.Icon = item.Icon;
                resultItem.Image = item.Image;
                resultItem.IsDefault = YesNo.NO;
                var policyDetailFind = _garnerPolicyDetailEFRepository.GetAllPolicyDetailByPolicyId(item.PolicyId);
                //resultItem.PolicyDetail = _mapper.Map<List<AppGarnerPolicyDetailDto>>(policyDetailFind);

                //Lấy chính sách mặc định nếu là khách hàng là sale hoặc có tư vấn viên mặc định
                if (policyDefault != null && policyDefault.PolicyId == item.PolicyId)
                {
                    resultItem.IsDefault = YesNo.YES;
                }
                result.Add(resultItem);
            }
            return result;
        }

        /// <summary>
        /// Xem trước lợi nhuận tích lũy cho App
        /// </summary>
        /// <param name="policyId"></param>
        /// <param name="totalValue"></param>
        public List<AppGarnerPolicyDetailDto> AppListPolicyDetail(int policyId, decimal totalValue)
        {
            _logger.LogInformation($"{nameof(AppListPolicyDetail)}: policyId = {policyId}, totalValue = {totalValue}");

            var result = _garnerFormulaServices.GetCashFlow(policyId, totalValue, DateTime.Now);
            return result;
        }

        /// <summary>
        /// App tổng quan sản phẩm tích lũy
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public AppDistributionOverviewDto DistributionOverview(int policyId)
        {
            var policyFind = _garnerPolicyEFRepository.FindById(policyId);
            if (policyFind == null)
            {
                _logger.LogError($"Không tìm thấy thông tin chính sách: policyId = {policyId}");
                return null;
            }

            var distributionFind = _garnerDistributionEFRepository.FindById(policyFind.DistributionId);
            if (distributionFind == null)
            {
                _logger.LogError($"Không tìm thấy thông tin phân phối sản phẩm: distributionId = {policyFind.DistributionId}");
                return null;
            }

            var productFind = _garnerProductEFRepository.FindById(distributionFind.ProductId);
            if (productFind == null)
            {
                _logger.LogError($"Không tìm thấy thông tin sản phẩm tích lũy: productId = {distributionFind.ProductId}");
                return null;
            }

            var rating = from rate in _dbContext.GarnerRatings
                         join order in _dbContext.GarnerOrders on rate.OrderId equals order.Id
                         where order.PolicyId == policyId && order.Deleted == YesNo.NO
                         select rate;

            var totalRating = rating.Count();
            decimal avarageRating = 0;
            if (totalRating > 0)
            {
                avarageRating = Decimal.Round(rating.Select(o => (decimal)o.Rate).Average(), 1);
            }

            var result = new AppDistributionOverviewDto();

            var maxProfit = _garnerPolicyDetailEFRepository.GetAllPolicyDetailByPolicyId(policyId)?.Max(r => r.Profit);
            result.Profit = maxProfit ?? 0;
            result.MinMoney = policyFind.MinMoney;
            result.Description = policyFind.Description;

            result.ProductCode = productFind.Code;
            result.ProductName = productFind.Name;
            result.ProductType = productFind.ProductType;
            // Cộng thêm 1000 tham gia fake
            result.TotalParticipants = 1000 + _garnerOrderEFRepository.EntityNoTracking
                .Where(o => o.Status == OrderStatus.DANG_DAU_TU)
                .Select(o => o.CifCode)
                .Distinct()
                .Count();
            result.RatingRate = avarageRating;
            result.TotalReviewers = totalRating;

            result.Description = policyFind.Description;
            result.ContentType = distributionFind.ContentType;
            result.OverviewContent = distributionFind.OverviewContent;
            result.OverviewImageUrl = distributionFind.OverviewImageUrl;

            // Lấy thông tin danh sách tổ chức
            var listOrganizationFind = _garnerProductOverviewOrgEFRepository.FindAllListByDistribution(policyFind.DistributionId);
            result.Organization = _mapper.Map<List<AppProductOverviewOrgDto>>(listOrganizationFind);

            // Lấy thông tin danh sách file tài liệu tổng quan
            var listOverviewFile = _garnerProductOverviewFileEFRepository.FindAllListByDistribution(policyFind.DistributionId);

            // Thông tin phân phối
            result.DistributionInfo = _mapper.Map<List<AppProductOverviewFileDto>>(listOverviewFile.Where(f => f.DocumentType == DocumentTypes.THONG_TIN_SAN_PHAM));

            // Hồ sơ pháp lý
            result.LegalRecords = _mapper.Map<List<AppProductOverviewFileDto>>(listOverviewFile.Where(f => f.DocumentType == DocumentTypes.HO_SO_PHAP_LY));
            return result;
        }
        #endregion

        public GarnerPolicyMoreInfoDto PolicyIsShowApp(int policyId)
        {
            _logger.LogInformation($"{nameof(PolicyIsShowApp)}: policyId = {policyId}");

            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyFind = _garnerPolicyEFRepository.FindById(policyId, tradingProviderId);
            var isShowApp = YesNo.YES;
            if (policyFind.IsShowApp == YesNo.NO)
            {
                isShowApp = YesNo.YES;
            }
            else
            {
                isShowApp = YesNo.NO;
            }
            var showApp = _garnerPolicyEFRepository.PolicyIsShowApp(policyId, isShowApp, tradingProviderId);
            var item = _mapper.Map<GarnerPolicyMoreInfoDto>(showApp);

            _dbContext.SaveChanges();
            return item;
        }

        /// <summary>
        /// thay đổi IsClose của distribution
        /// </summary>
        /// <param name="distributionId"></param>
        public void DistributionClose(int distributionId)
        {
            _logger.LogInformation($"{nameof(DistributionClose)}: distributionId = {distributionId}");
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var distributionFind = _garnerDistributionEFRepository.DistributionClose(distributionId, tradingProviderId);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Lấy danh sách distribution không phân trang
        /// </summary>
        /// <returns></returns>
        public List<GarnerDistributionDto> GetAllDistribution(GarnerDistributionFilterDto input)
        {

            int? tradingProviderId = null;
            int? partnerId = null;
            var usertype = CommonUtils.GetCurrentUserType(_httpContext);
            if (usertype == UserTypes.TRADING_PROVIDER || usertype == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (usertype == UserTypes.PARTNER || usertype == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                _dbContext.CheckTradingRelationshipPartner(partnerId, input.TradingProviderIds);
            }
            _logger.LogInformation($"{nameof(GetAllDistribution)}: input = {JsonSerializer.Serialize(input)}, tradingProviderId = {tradingProviderId}, partnerId = {partnerId}");

            var result = (from distribution in _dbContext.GarnerDistributions
                          let isInvested = _garnerOrderEFRepository.SumValue(distribution.Id, null)
                          let product = _garnerProductEFRepository.FindById(distribution.ProductId, null)
                          where distribution.IsClose == YesNo.NO && distribution.Status == DistributionStatus.HOAT_DONG
                           && (distribution.OpenCellDate != null && distribution.OpenCellDate.Value.Date <= DateTime.Now.Date)
                           && (distribution.CloseCellDate != null && distribution.CloseCellDate.Value.Date >= DateTime.Now.Date)
                           && distribution.Deleted == YesNo.NO
                           && ((tradingProviderId != null && distribution.TradingProviderId == tradingProviderId) || (input.TradingProviderIds != null && input.TradingProviderIds.Contains(distribution.TradingProviderId)))
                          select new GarnerDistributionDto
                          {
                              Id = distribution.Id,
                              ProductId = distribution.ProductId,
                              TradingProviderId = distribution.TradingProviderId,
                              OpenCellDate = distribution.OpenCellDate,
                              CloseCellDate = distribution.CloseCellDate,
                              Status = distribution.Status,
                              IsClose = distribution.IsClose,
                              IsShowApp = distribution.IsShowApp,
                              IsCheck = distribution.IsCheck,
                              IsDefault = distribution.IsDefault,
                              ContentType = distribution.ContentType,
                              OverviewContent = distribution.OverviewContent,
                              OverviewImageUrl = distribution.OverviewImageUrl,
                              Image = distribution.Image,
                              CreatedBy = distribution.CreatedBy,
                              IsInvested = isInvested,
                              GarnerProduct = _mapper.Map<GarnerProductDto>(product),
                              Policies = (from policy in _dbContext.GarnerPolicies
                                          where policy.DistributionId == distribution.Id && policy.Status == Status.ACTIVE && policy.Deleted == YesNo.NO
                                          select new GarnerPolicyMoreInfoDto
                                          {
                                              Id = policy.Id,
                                              TradingProviderId = policy.TradingProviderId,
                                              DistributionId = policy.DistributionId,
                                              Code = policy.Code,
                                              Name = policy.Name,
                                              MinMoney = policy.MinMoney,
                                              MaxMoney = policy.MaxMoney,
                                              MinInvestDay = policy.MinInvestDay,
                                              IncomeTax = policy.IncomeTax,
                                              InvestorType = policy.InvestorType,
                                              Classify = policy.Classify,
                                              CalculateType = policy.CalculateType,
                                              GarnerType = policy.GarnerType,
                                              InterestType = policy.InterestType,
                                              InterestPeriodQuantity = policy.InterestPeriodQuantity,
                                              InterestPeriodType = policy.InterestPeriodType,
                                              RepeatFixedDate = policy.RepeatFixedDate,
                                              MinWithdraw = policy.MinWithdraw,
                                              MaxWithdraw = policy.MaxWithdraw,
                                              WithdrawFee = policy.WithdrawFee,
                                              WithdrawFeeType = policy.WithdrawFeeType,
                                              OrderOfWithdrawal = policy.OrderOfWithdrawal,
                                              IsTransferAssets = policy.IsTransferAssets,
                                              TransferAssetsFee = policy.TransferAssetsFee,
                                              IsShowApp = policy.IsShowApp,
                                              Status = policy.Status,
                                              IsDefault = policy.IsDefault,
                                              IsDefaultEpic = policy.IsDefaultEpic,
                                              StartDate = policy.StartDate,
                                              EndDate = policy.EndDate,
                                              Description = policy.Description,
                                              SortOrder = policy.SortOrder,
                                              PolicyDetails = (from gpd in _dbContext.GarnerPolicyDetails
                                                               join gp in _dbContext.GarnerPolicies on gpd.PolicyId equals gp.Id
                                                               where gp.Id == policy.Id && gp.Deleted == YesNo.NO && gpd.Deleted == YesNo.NO
                                                               && gpd.Status == Status.ACTIVE
                                                               orderby gpd.Id descending
                                                               select new GarnerPolicyDetailDto
                                                               {
                                                                   Id = gpd.Id,
                                                                   TradingProviderId = gpd.TradingProviderId,
                                                                   DistributionId = gpd.DistributionId,
                                                                   PolicyId = gpd.PolicyId,
                                                                   SortOrder = gpd.SortOrder,
                                                                   Name = gpd.Name,
                                                                   ShortName = gpd.ShortName,
                                                                   IsShowApp = gpd.IsShowApp,
                                                                   Status = gpd.Status,
                                                                   Profit = gpd.Profit,
                                                                   InterestDays = gpd.InterestDays,
                                                                   PeriodQuantity = gpd.PeriodQuantity,
                                                                   PeriodType = gpd.PeriodType,
                                                                   InterestType = gpd.InterestType,
                                                                   InterestPeriodQuantity = gpd.InterestPeriodQuantity,
                                                                   InterestPeriodType = gpd.InterestPeriodType,
                                                                   RepeatFixedDate = gpd.RepeatFixedDate
                                                               }).ToList()
                                          }).OrderByDescending(p => p.Id).ToList()
                          }).ToList();

            foreach (var item in result)
            {
                var productTradingProvider = _garnerDistributionSharedServices.LimitCalculationTrading(item.ProductId, item.TradingProviderId);
                if (productTradingProvider != null)
                {
                    item.HasTotalInvestmentSub = productTradingProvider.HasTotalInvestmentSub;
                    item.TotalInvestmentSub = productTradingProvider.TotalInvestmentSub;
                    item.Quantity = productTradingProvider.Quantity;
                }
            }
            return result;
        }

        /// <summary>
        /// Thêm config mã hợp đồng (Config contract code)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public void AddConfigContractCode(CreateConfigContractCodeDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(AddConfigContractCode)}: input = {JsonSerializer.Serialize(input)}");

            var transaction = _dbContext.Database.BeginTransaction();
            //Add cấu hình contractCode
            var insertConfig = _mapper.Map<GarnerConfigContractCode>(input);
            insertConfig.Status = Utils.Status.ACTIVE;

            var resultConfig = _garnerConfigContractCodeEFRepository.Add(insertConfig, tradingProviderId, username);
            //Add Config detail
            foreach (var item in input.ConfigContractCodeDetails)
            {
                var insertConfigDetail = _mapper.Map<GarnerConfigContractCodeDetail>(item);
                insertConfigDetail.ConfigContractCodeId = resultConfig.Id;
                var detailAdd = _garnerConfigContractCodeDetailEFRepository.Add(insertConfigDetail);
            }
            _dbContext.SaveChanges();
            transaction.Commit();
        }

        /// <summary>
        /// Cập nhật config mã hợp đồng (Config contract code)
        /// </summary>
        /// <param name="input"></param>
        public void UpdateConfigContractCode(UpdateConfigContractCodeDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _logger.LogInformation($"{nameof(UpdateConfigContractCode)}: input = {JsonSerializer.Serialize(input)}; tradingProviderId = {tradingProviderId}");

            var configContractCode = _garnerConfigContractCodeEFRepository.FindById(input.Id, tradingProviderId);
            if (configContractCode == null)
            {
                _defErrorEFRepository.ThrowException(ErrorCode.GarnerConfigContractCodeNotFound);
            }
            configContractCode.Name = input.Name;
            configContractCode.Description = input.Description;
            UpdateConfigContractCodeDetail(input.ConfigContractCodeDetails, input.Id);
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Cập nhật config contract code details
        /// </summary>
        /// <param name="input"></param>
        /// <param name="configContractCodeId"></param>
        public void UpdateConfigContractCodeDetail(List<CreateConfigContractCodeDetailDto> input, int configContractCodeId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateConfigContractCodeDetail)}: input = {JsonSerializer.Serialize(input)}");

            var configContractCode = _garnerConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId)
                .ThrowIfNull(_dbContext, ErrorCode.GarnerConfigContractCodeNotFound);

            var getConfigDetailQuery = _garnerConfigContractCodeDetailEFRepository.Entity.Where(c => c.ConfigContractCodeId == configContractCodeId);
            // Xóa detail
            var ids = input.Where(i => i.Id != 0).Select(i => i.Id);
            var removeDetail = getConfigDetailQuery.Where(d => !ids.Contains(d.Id)).ToList();
            _garnerConfigContractCodeDetailEFRepository.Entity.RemoveRange(removeDetail);

            foreach (var item in input)
            {
                if (item.Id == 0)
                {
                    if (item.Key != ConfigContractCode.FIX_TEXT)
                    {
                        item.Value = null;
                    }
                    var insertConfigDetail = _mapper.Map<GarnerConfigContractCodeDetail>(item);
                    _garnerConfigContractCodeDetailEFRepository.Add(insertConfigDetail);
                }
                else
                {
                    var configdetailUpdate = _garnerConfigContractCodeDetailEFRepository.Entity.FirstOrDefault(e => e.Id == item.Id);
                    if (configdetailUpdate != null)
                    {
                        configdetailUpdate.Key = item.Key;
                        configdetailUpdate.SortOrder = item.SortOrder;
                        if (item.Key == ConfigContractCode.FIX_TEXT)
                        {
                            configdetailUpdate.Value = item.Value;
                        }
                    }
                }
            }
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xem danh sách config contract code
        /// </summary>
        /// <returns></returns>
        public PagingResult<GarnerConfigContractCodeDto> GetAllConfigContractCode(FilterConfigContractCodeDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            _logger.LogInformation($"{nameof(GetAllConfigContractCode)}");

            var configContractCodes = _garnerConfigContractCodeEFRepository.GetAll(input, tradingProviderId);
            var result = new PagingResult<GarnerConfigContractCodeDto>();
            result.Items = _mapper.Map<List<GarnerConfigContractCodeDto>>(configContractCodes.Items);
            result.TotalItems = configContractCodes.TotalItems;
            foreach (var item in result.Items)
            {
                item.ConfigContractCodeDetails = _mapper.Map<List<GarnerConfigContractCodeDetailDto>>(_garnerConfigContractCodeDetailEFRepository.GetAllByConfigId(item.Id));
            }
            return result;
        }

        public List<GarnerConfigContractCodeDto> GetAllConfigContractCodeStatusActive()
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            _logger.LogInformation($"{nameof(GetAllConfigContractCode)}");

            var configContractCodes = _garnerConfigContractCodeEFRepository.GetAllStatusActive(tradingProviderId);
            var result = _mapper.Map<List<GarnerConfigContractCodeDto>>(configContractCodes);
            foreach (var item in result)
            {
                item.ConfigContractCodeDetails = _mapper.Map<List<GarnerConfigContractCodeDetailDto>>(_garnerConfigContractCodeDetailEFRepository.GetAllByConfigId(item.Id));
            }
            return result;
        }

        /// <summary>
        /// Lấy config theo Id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        /// <param name="customerType"></param>
        /// <returns></returns>
        public GarnerConfigContractCodeDto GetConfigContractCodeById(int configContractCodeId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            _logger.LogInformation($"{nameof(GetConfigContractCodeById)}: configContractCodeId = {configContractCodeId}");

            var configContractCode = _garnerConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId);
            var result = _mapper.Map<GarnerConfigContractCodeDto>(configContractCode);
            result.ConfigContractCodeDetails = _mapper.Map<List<GarnerConfigContractCodeDetailDto>>(_garnerConfigContractCodeDetailEFRepository.GetAllByConfigId(configContractCode.Id));
            return result;
        }

        /// <summary>
        /// Đổi trạng thái config theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        public void UpdateConfigContractCodeStatus(int configContractCodeId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(UpdateConfigContractCodeStatus)}: configContractCodeId = {configContractCodeId}");

            var configContractCode = _garnerConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId);
            configContractCode.Status = (configContractCode.Status == Utils.Status.ACTIVE) ? Utils.Status.INACTIVE : Utils.Status.ACTIVE;
            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Xóa theo id
        /// </summary>
        /// <param name="configContractCodeId"></param>
        public void DeleteConfigContractCode(int configContractCodeId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _logger.LogInformation($"{nameof(DeleteConfigContractCode)}: configContractCodeId = {configContractCodeId}");

            var configContractCode = _garnerConfigContractCodeEFRepository.FindById(configContractCodeId, tradingProviderId);
            configContractCode.Deleted = YesNo.YES;
            _dbContext.SaveChanges();
        }

        public void IsShowApp(int distributionId)
        {
            _logger.LogInformation($"{nameof(IsShowApp)}: distributionId = {distributionId}");
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var distribution = _garnerDistributionEFRepository.FindById(distributionId, tradingProviderId);
            if (distribution.IsShowApp == YesNo.NO)
            {
                distribution.IsShowApp = YesNo.YES;
            }
            else
            {
                distribution.IsShowApp = YesNo.NO;
            }
            var showApp = _garnerDistributionEFRepository.Update(distribution, tradingProviderId, CommonUtils.GetCurrentUsername(_httpContext));
            _dbContext.SaveChanges();
        }

        public void ImportProductPrice(IFormFile file, int ditributionId)
        {
            if (file == null)
            {
                throw new FaultException(new FaultReason($"File được upload không có nội dung."), new FaultCode(((int)ErrorCode.FileUploadNoContent).ToString()), "");
            }
            if (file.Length > GarnerFileUpload.LimitLengthUpload)
            {
                throw new FaultException(new FaultReason($"Kích thước file không được vượt quá {GarnerFileUpload.LimitLengthUpload / (1024 * 1024)} MB."), new FaultCode(((int)ErrorCode.FileOverUploadLimit).ToString()), "");
            }

            //MemoryStream memoryStream = new();
            //file.CopyTo(memoryStream);
            SpreadsheetDocument doc = SpreadsheetDocument.Open(file.OpenReadStream(), false);
            WorkbookPart workbookPart = doc.WorkbookPart;

            List<WorksheetPart> worksheetParts = workbookPart.WorksheetParts.ToList();
            WorksheetPart worksheetPart = worksheetParts[0];
            SheetData sheetData = worksheetPart.Worksheet.Elements<SheetData>().First();
            List<Row> rows = sheetData.Elements<Row>().ToList();

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var ditributionFind = _garnerDistributionEFRepository.FindById(ditributionId, tradingProviderId);

            var rowCount = 0;
            try
            {
                for (var rowIndex = 0; rowIndex < rows.Count; rowIndex++)
                {
                    var row = rows[rowIndex];
                    if (string.IsNullOrWhiteSpace(rows[1].InnerText))
                    {
                        throw new FaultException(new FaultReason($"Dữ liệu dòng đầu tiên trống"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }
                    if (string.IsNullOrWhiteSpace(row.InnerText))
                    {
                        break;
                    };
                    rowCount++;

                }
                if (rowCount == 0)
                {
                    throw new FaultException(new FaultReason($"File excel không có dữ liệu"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw new FaultException(new FaultReason($"File excel không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            Cell cellDateFirst = rows[1].Elements<Cell>().ToList()[0];
            Cell cellDateLast = rows[rowCount - 1].Elements<Cell>().ToList()[0];

            if (!double.TryParse(cellDateFirst.InnerText, out double dateInnerTextFirst))
            {
                throw new FaultException(new FaultReason($"Thông tin ngày tại dòng đầu tiên không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            if (!double.TryParse(cellDateLast.InnerText, out double dateInnerTextLast))
            {
                throw new FaultException(new FaultReason($"Thông tin ngày tại hàng cuối cùng không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            DateTime dateFirst = DateTime.FromOADate(dateInnerTextFirst);
            DateTime dateLast = DateTime.FromOADate(dateInnerTextLast);
            if (dateFirst.ToShortDateString() != ditributionFind.OpenCellDate.Value.ToShortDateString())
            {
                throw new FaultException(new FaultReason($"Dữ liệu ngày ở hàng đầu tiên không trùng với ngày mở bán"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            if (dateLast.ToShortDateString() != ditributionFind.CloseCellDate.Value.ToShortDateString())
            {
                throw new FaultException(new FaultReason($"Dữ liệu ngày ở hàng cuối cùng không trùng với ngày đáo hạn"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }

            using (TransactionScope scope = new TransactionScope())
            {
                for (int i = 1; i < rowCount; i++)
                {
                    var cells = rows[i].Elements<Cell>().ToList();

                    Cell cellDate = cells[0];

                    Cell cellPrice = cells[1];

                    if (!double.TryParse(cellDate?.InnerText, out double dateInnerText))
                    {
                        throw new FaultException(new FaultReason($"Dòng thứ {i} có dữ liệu không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }

                    if (!double.TryParse(cellPrice.InnerText, out double price))
                    {
                        throw new FaultException(new FaultReason($"Dòng thứ {i} có dữ liệu không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }

                    DateTime date = DateTime.FromOADate(dateInnerText);
                    DateTime datePrevious = DateTime.FromOADate(dateInnerText);

                    if (i > 1)
                    {

                        var cellsPrevious = rows[i - 1]?.Elements<Cell>().ToList();
                        Cell cellDatePrevious = cellsPrevious[0];

                        if (!double.TryParse(cellDatePrevious?.InnerText, out double dateInnerText2))
                        {
                            throw new FaultException(new FaultReason($"Hàng thứ {i} có dữ liệu không hợp lệ"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                        }

                        DateTime datePrevios = DateTime.FromOADate(dateInnerText2);

                        if ((date.Date - datePrevios.Date).Days != 1)
                        {
                            throw new FaultException(new FaultReason($"Dữ liệu ngày hàng thứ {i} và {i + 1} không liên tiếp với nhau "), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                        }
                    }

                    var insertProductPrice = _garnerProductPriceEFRepository.Add(new GarnerProductPrice
                    {
                        TradingProviderId = tradingProviderId,
                        DistributionId = ditributionId,
                        PriceDate = date,
                        Price = (decimal)price,
                        CreatedBy = username
                    });

                    _dbContext.SaveChanges();

                    //Lịch sử thay đổi
                    _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                    {
                        UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT_PRICE,
                        RealTableId = insertProductPrice.Id,
                        Action = ActionTypes.THEM_MOI,
                        Summary = GarnerHistoryUpdateSummary.SUMMARY_ADD_PRODUC_RPICE
                    }, username);

                    _dbContext.SaveChanges();
                }
                scope.Complete();
            }
        }

        public void DeleteProductPrice(int ditributionId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var ditributionFind = _garnerDistributionEFRepository.FindById(ditributionId, tradingProviderId).ThrowIfNull(_dbContext, ErrorCode.GarnerDistributionNotFound);
            _garnerProductPriceEFRepository.Delete(ditributionId, tradingProviderId);
            var productPrices = _garnerProductPriceEFRepository.Entity.Where(p => p.DistributionId == ditributionId && p.Deleted == YesNo.NO && p.TradingProviderId == tradingProviderId);

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT_PRICE,
                RealTableId = ditributionId,
                Action = ActionTypes.THEM_MOI,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_DELETE_PRODUC_RPICE
            }, username);

            _dbContext.SaveChanges();
        }

        public PagingResult<GarnerProductPrice> FindAll(FilterGarnerProductPriceDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _garnerProductPriceEFRepository.FindAll(input, tradingProviderId);
        }

        public void UpdateProductPrice(UpdateProductPriceDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var productPriceFind = _garnerProductPriceEFRepository.FindProductPrice(input.Id, input.PriceDate).ThrowIfNull(_dbContext, ErrorCode.GarnerProductPriceNotFoundDate);
            _garnerProductPriceEFRepository.Update(input, username, tradingProviderId);
            _dbContext.SaveChanges();
        }

        #region File chính sách
        /// <summary>
        /// Thêm File chính sách
        /// </summary>
        /// <param name="input"></param>
        public void AddDistributionPolicyFile(CreateGarnerProductOverviewFileDto input)
        {
            _logger.LogInformation($"{nameof(AddDistributionPolicyFile)}: input = {JsonSerializer.Serialize(input)}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            //Tìm kiếm thông tin phân phối sản phẩm
            var distributionFind = _garnerDistributionEFRepository.FindById(input.DistributionId).ThrowIfNull(_dbContext, ErrorCode.GarnerDistributionNotFound);

            //Cập nhật thông tin File
            var result = _garnerProductOverviewFileEFRepository.AddDistributionPolicyFile(input.DistributionId, input, tradingProviderId, username);
            _dbContext.SaveChanges();

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT_OVERVIEW_FILE,
                RealTableId = result.Id,
                Action = ActionTypes.THEM_MOI,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_ADD_POLICY_FILE
            }, username);

            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Update File chính sách
        /// </summary>
        /// <param name="input"></param>
        public void UpdateDistributionPolicyFile(CreateGarnerProductOverviewFileDto input)
        {
            _logger.LogInformation($"{nameof(UpdateDistributionPolicyFile)}: input = {JsonSerializer.Serialize(input)}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            //Tìm kiếm thông tin phân phối sản phẩm
            var distributionFind = _garnerDistributionEFRepository.FindById(input.DistributionId).ThrowIfNull(_dbContext, ErrorCode.GarnerDistributionNotFound);


            //Oldvalue
            var distributionPolicyFile = _garnerProductOverviewFileEFRepository.Entity.FirstOrDefault(e => e.Id == input.Id && e.DistributionId == input.DistributionId && e.TradingProviderId == tradingProviderId && e.Deleted == YesNo.NO);
            #region Lịch sử thay đổi
            //Thay đổi file
            if (distributionPolicyFile.Url != input.Url)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT_OVERVIEW_FILE,
                    RealTableId = distributionPolicyFile.Id,
                    OldValue = distributionPolicyFile.Url,
                    NewValue = input.Url,
                    FieldName = GarnerFieldName.UPDATE_URL,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_POLICY_FILE_URL
                }, username);
            }

            //Thay đổi tên file
            if (distributionPolicyFile.Title != input.Title)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT_OVERVIEW_FILE,
                    RealTableId = distributionPolicyFile.Id,
                    OldValue = distributionPolicyFile.Title,
                    NewValue = input.Title,
                    FieldName = GarnerFieldName.UPDATE_TITLE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_POLICY_FILE_TITLE
                }, username);
            }

            //Thay đổi ngày có hiệu lực
            if (distributionPolicyFile.EffectiveDate != input.EffectiveDate)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT_OVERVIEW_FILE,
                    RealTableId = distributionPolicyFile.Id,
                    OldValue = distributionPolicyFile.EffectiveDate.ToString(),
                    NewValue = input.EffectiveDate.ToString(),
                    FieldName = GarnerFieldName.UPDATE_EFFECTIVE_DATE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_EFFECTIVE_DATE
                }, username);
            }

            //Thay đổi ngày hết hiệu lực
            if (distributionPolicyFile.ExpirationDate != input.ExpirationDate)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT_OVERVIEW_FILE,
                    RealTableId = distributionPolicyFile.Id,
                    OldValue = distributionPolicyFile.ExpirationDate.ToString(),
                    NewValue = input.ExpirationDate.ToString(),
                    FieldName = GarnerFieldName.UPDATE_EXPIRATION_DATE,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_EXPIRATION_DATE
                }, username);
            }

            //Thay đổi mô tả
            if (distributionPolicyFile.Description != input.Description)
            {
                _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
                {
                    UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT_OVERVIEW_FILE,
                    RealTableId = distributionPolicyFile.Id,
                    OldValue = distributionPolicyFile.Description,
                    NewValue = input.Description,
                    FieldName = GarnerFieldName.UPDATE_DESCRIPTION,
                    Action = ActionTypes.CAP_NHAT,
                    Summary = GarnerHistoryUpdateSummary.SUMMARY_UPDATE_DESCRIPTION
                }, username);
            }
            #endregion

            //Cập nhật thông tin File
            distributionPolicyFile.Title = input.Title;
            distributionPolicyFile.Url = input.Url;
            distributionPolicyFile.DocumentType = DocumentTypes.FILE_CHINH_SACH;
            distributionPolicyFile.Description = input.Description;
            distributionPolicyFile.ExpirationDate = input.ExpirationDate;
            distributionPolicyFile.EffectiveDate = input.EffectiveDate;
            distributionPolicyFile.ModifiedDate = DateTime.Now;
            distributionPolicyFile.ModifiedBy = username;


            _dbContext.SaveChanges();
        }

        /// <summary>
        /// Update File chính sách
        /// </summary>
        /// <param name="input"></param>
        public void DeleteDistributionPolicyFile(int id)
        {
            _logger.LogInformation($"{nameof(DeleteDistributionPolicyFile)}: id = {id}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            //Tìm kiếm thông tin phân phối sản phẩm

            var policyFile = _garnerProductOverviewFileEFRepository.Entity.FirstOrDefault(e => e.Id == id && e.DocumentType == DocumentTypes.FILE_CHINH_SACH && e.Deleted == YesNo.NO).ThrowIfNull(_dbContext, ErrorCode.GarnerDistributionPolicyFileNotFound);

            policyFile.Deleted = YesNo.YES;
            policyFile.ModifiedBy = username;
            policyFile.ModifiedDate = DateTime.Now;

            //Lịch sử thay đổi
            _garnerHistoryUpdateEFRepository.Add(new GarnerHistoryUpdate()
            {
                UpdateTable = GarnerHistoryUpdateTables.GAN_PRODUCT_OVERVIEW_FILE,
                RealTableId = id,
                Action = ActionTypes.XOA,
                Summary = GarnerHistoryUpdateSummary.SUMMARY_DELETE_POLICY_FILE
            }, username);

            _dbContext.SaveChanges();
        }

        public PagingResult<GarnerProductOverviewFile> FindAllDistributionFile(FilterGarnerDistributionFileDto input)
        {
            _logger.LogInformation($"{nameof(FindAllDistributionFile)}: distributionId = {input.DistributionId}");

            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var distributionFind = _garnerDistributionEFRepository.FindById(input.DistributionId).ThrowIfNull(_dbContext, ErrorCode.GarnerDistributionNotFound);
            var result = new PagingResult<GarnerProductOverviewFile>();

            result.Items = _garnerProductOverviewFileEFRepository.FindAllPolicyFileByDistribution(input.DistributionId, tradingProviderId);

            result.TotalItems = result.Items.Count();
            if (input.PageSize != -1)
            {
                result.Items = result.Items.Skip(input.Skip).Take(input.PageSize);
            }
            return result;
        }

        #endregion

        public PagingResult<GarnerHistoryUpdate> FindAllDistributionHistory(FilterGarnerDistributionHistoryDto input)
        {
            _logger.LogInformation($"{nameof(FindAllDistributionHistory)}: input = {JsonSerializer.Serialize(input)}");

            var result = _garnerHistoryUpdateEFRepository.FindAllByMultiTable(input);

            return result;
        }

        public IEnumerable<GarnerDistributionByTradingDto> FindAllDistributionBanHoByTrading()
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var (tradingProviderId, tradingBanHo) = userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER
                                                ? (CommonUtils.GetCurrentTradingProviderId(_httpContext), _saleEFRepository.FindTradingProviderBanHo(CommonUtils.GetCurrentTradingProviderId(_httpContext)))
                                                : (0, new List<int>());

            var result = (from distributions in _dbContext.GarnerDistributions
                          join products in _dbContext.GarnerProducts on distributions.ProductId equals products.Id
                          join productTradings in _dbContext.GarnerProductTradingProviders on products.Id equals productTradings.ProductId
                          where (userType == UserTypes.ROOT_EPIC || (tradingBanHo.Contains(distributions.TradingProviderId)
                                || distributions.TradingProviderId == tradingProviderId))
                                && distributions.Status == DistributionStatus.HOAT_DONG
                                && distributions.IsShowApp == YesNo.YES && distributions.IsClose == YesNo.NO
                                && distributions.Deleted == YesNo.NO && products.Deleted == YesNo.NO && productTradings.Deleted == YesNo.NO
                          select new GarnerDistributionByTradingDto
                          {
                              Id = distributions.Id,
                              Code = products.Code,
                              Name = products.Name,
                              IsSalePartnership = tradingBanHo.Contains(distributions.TradingProviderId)
                          }).Distinct().AsEnumerable();
            return result;
        }
    }
}
