using AutoMapper;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesEntities.DataEntities;
using EPIC.CompanySharesEntities.Dto.CpsInfo;
using EPIC.CompanySharesRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.CompanyShares;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using CalendarRepository = EPIC.CompanySharesRepositories.CalendarRepository;

namespace EPIC.CompanySharesDomain.Implements
{
    public class CpsInfoServices : ICpsInfoServices
    {
        private readonly ILogger<CpsInfoServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly IHttpContextAccessor _httpContext;
        private readonly CpsInfoRepository _cpsInfoRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly CalendarRepository _calendarRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly ApproveRepository _approveRepository;
        private readonly IMapper _mapper;
        private readonly ICpsSharedServices _cpsSharedServices;
        private readonly OrderRepository _orderRepository;
        //private readonly BondIssuerRepository _orderRepository;

        public CpsInfoServices(
           ILogger<CpsInfoServices> logger,
           IConfiguration configuration,
           DatabaseOptions databaseOptions,
           IHttpContextAccessor httpContext,
           IMapper mapper,
           ICpsSharedServices cpsSharedServices
           )
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _cpsInfoRepository = new CpsInfoRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _calendarRepository = new CalendarRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _mapper = mapper;
            _cpsSharedServices = cpsSharedServices;
            _orderRepository = new OrderRepository(_connectionString, _logger);
        }

        public CpsInfo Add(CreateCpsInfoDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);

            var companySharesInfo = new CpsInfo
            {
                IssuerId = input.IssuerId,
                CpsCode = input.CpsCode,
                CpsName = input.CpsName,
                Description = input.Description,
                Content = input.Content,
                IssueDate = input.IssueDate,
                DueDate = input.DueDate,
                ParValue = input.ParValue,
                Period = input.Period,
                PeriodUnit = input.PeriodUnit,
                InterestRate = input.InterestRate,
                InterestPeriod = input.InterestPeriod,
                InterestPeriodUnit = input.InterestPeriodUnit,
                InterestRateType = input.InterestRateType,
                IsPaymentGuarantee = input.IsPaymentGuarantee,
                IsAllowSbd = input.IsAllowSbd,
                AllowSbdDay = input.AllowSbdDay,
                IsCollateral = input.IsCollateral,
                MaxInvestor = input.MaxInvestor,
                NumberClosePer = input.NumberClosePer,
                CountType = input.CountType,
                IsListing = input.IsListing,
                Quantity = input.Quantity,
                IsCheck = input.IsCheck,
                PolicyPaymentContent = input.PolicyPaymentContent,
                Icon = input.Icon,
                PartnerId = partnerId,
                CreatedBy = username,
                TotalInvestment = input.TotalInvestment,
                HasTotalInvestmentSub = input.HasTotalInvestmentSub
            };
            return _cpsInfoRepository.Add(companySharesInfo);
        }

        public int Delete(int id)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            return _cpsInfoRepository.Delete(id, partnerId);
        }

        public PagingResult<CpsInfoDto> FindAll(int pageSize, int pageNumber, string keyword, string status, string isCheck, DateTime? issueDate, DateTime? dueDate)
        {
            int partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var companySharesInfoList = _cpsInfoRepository.FindAllCompanySharesInfo(pageSize, pageNumber, keyword, status, partnerId, isCheck, issueDate, dueDate);
            var result = new PagingResult<CpsInfoDto>();
            var items = new List<CpsInfoDto>();
            result.TotalItems = companySharesInfoList.TotalItems;
            foreach (var companyShares in companySharesInfoList.Items)
            {
                var cpsInfoById = _cpsInfoRepository.FindById(companyShares.Id);
                var cpsInfo = _mapper.Map<CpsInfoDto>(companyShares);

                long soLuongDaBan = 0;
                long sumQuantityOrder = 0;

                var companySharesSecondary = _cpsInfoRepository.FindById(cpsInfo.Id);
                if (companySharesSecondary != null)
                {
                    //sumQuantityOrder = _companySharesInfoRepository.SumQuantity(bondSecondary.TradingProviderId, bondSecondary.Id);
                }
                soLuongDaBan += sumQuantityOrder;

                cpsInfo.SoLuongConLai = cpsInfo.Quantity - soLuongDaBan;

                //var issuer = _issuerRepository.FindById(cpsInfoById.IssuerId.Value);
                //if (issuer != null)
                //{
                //    cpsInfo.BondIssuer = new ViewIssuerDto()
                //    {
                //        IssuerId = issuer.IssuerId,
                //        BusinessCustomerId = issuer.BusinessCustomerId
                //    };

                //    var businessCustomerIssuer = _businessCustomerRepository.FindBusinessCustomerById(issuer.BusinessCustomerId);
                //    if (businessCustomerIssuer != null)
                //    {
                //        cpsInfo.BondIssuer.BusinessCustomer = new BusinessCustomerDto()
                //        {
                //            BusinessCustomerId = businessCustomerIssuer.BusinessCustomerId,
                //            TaxCode = businessCustomerIssuer.TaxCode,
                //            ShortName = businessCustomerIssuer.ShortName,
                //            Code = businessCustomerIssuer.Code,
                //            Name = businessCustomerIssuer.Name,
                //        };
                //        var listBank = _businessCustomerRepository.FindAllBusinessCusBank(businessCustomerIssuer.BusinessCustomerId ?? 0, -1, 0, null);
                //        cpsInfo.BondIssuer.BusinessCustomer.BusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
                //    };
                //};


                items.Add(cpsInfo);
            }
            result.Items = items;
            return result;
        }

        public CpsInfoDto FindById(int id)
        {
            var cpsInfo = _cpsInfoRepository.FindById(id);
            //var issuer = _issuerRepository.FindById(cpsInfo.IssuerId.Value);

            var result = _mapper.Map<CpsInfoDto>(cpsInfo);

            decimal soLuongDaBan = 0;
            long sumQuantityOrder = 0;

            //CPS Secondary

            var CpsSecondary = _orderRepository.FindById(cpsInfo.Id);

            if (CpsSecondary != null)
            {
                sumQuantityOrder = _orderRepository.SumQuantity(CpsSecondary.TradingProviderId, CpsSecondary.Id);
            }
            soLuongDaBan += sumQuantityOrder;

            //result.SoLuongConLai = cpsInfo.Quantity - soLuongDaBan;

            //if (issuer != null)
            //{
            //    result.BondIssuer = new ViewIssuerDto()
            //    {
            //        IssuerId = issuer.IssuerId,
            //        BusinessCustomerId = issuer.BusinessCustomerId
            //    };

            //    var businessCustomerIssuer = _businessCustomerRepository.FindBusinessCustomerById(issuer.BusinessCustomerId);
            //    if (businessCustomerIssuer != null)
            //    {
            //        result.BondIssuer.BusinessCustomer = new BusinessCustomerDto()
            //        {
            //            BusinessCustomerId = businessCustomerIssuer.BusinessCustomerId,
            //            TaxCode = businessCustomerIssuer.TaxCode,
            //            ShortName = businessCustomerIssuer.ShortName,
            //            Code = businessCustomerIssuer.Code,
            //            Name = businessCustomerIssuer.Name,
            //        };
            //    };
            //};

            return result;
        }
        /// <summary>
        /// Find Coupon CompanyShare
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public DividendCpsDto FindDividendById(int id)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var cpsInfo = _cpsInfoRepository.FindById(id);

            // CalculateCouponByQuantity

            var result = _cpsSharedServices.CalculateDividendByQuantity(cpsInfo, cpsInfo.Quantity ?? 0 , partnerId);
            return result;
        }

        public int Update(int id, UpdateCpsInfoDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _cpsInfoRepository.Update(new CpsInfo
            {
                Id = id,
                IssuerId = input.IssuerId,
                CpsCode = input.CpsCode,
                CpsName = input.CpsName,
                Description = input.Description,
                Content = input.Content,
                IssueDate = input.IssueDate,
                DueDate = input.DueDate,
                Period = input.Period,
                PeriodUnit = input.PeriodUnit,
                InterestRate = input.InterestRate,
                InterestPeriod = input.InterestPeriod,
                InterestPeriodUnit = input.InterestPeriodUnit,
                InterestRateType = input.InterestRateType,
                IsPaymentGuarantee = input.IsPaymentGuarantee,
                IsAllowSbd = input.IsAllowSbd,
                AllowSbdDay = input.AllowSbdDay,
                IsCollateral = input.IsCollateral,
                MaxInvestor = input.MaxInvestor,
                NumberClosePer = input.NumberClosePer,
                CountType = input.CountType,
                IsListing = input.IsListing,
                Quantity = input.Quantity,
                IsCheck = input.IsCheck,
                PolicyPaymentContent = input.PolicyPaymentContent,
                Icon = input.Icon,
                ModifiedBy = username,
                PartnerId = partnerId,
                TotalInvestment = input.TotalInvestment,
                HasTotalInvestmentSub = input.HasTotalInvestmentSub
            });
        }

        public void Request(RequestStatusDto input)
        {
            var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var actionType = ActionTypes.THEM_MOI;
                var checkIsUpdate = _approveRepository.GetOneByActual(input.Id, CpsDataTypes.COMPANY_SHARES_INFO);
                if (checkIsUpdate != null)
                {
                    actionType = ActionTypes.CAP_NHAT;
                }
                _approveRepository.CreateApproveRequest(new CreateApproveRequestDto
                {
                    UserRequestId = userId,
                    UserApproveId = input.UserApproveId,
                    RequestNote = input.RequestNote,
                    ActionType = actionType,
                    DataType = CpsDataTypes.COMPANY_SHARES_INFO,
                    ReferId = input.Id,
                    Summary = input.Summary
                }, null, partnerId);
                _cpsInfoRepository.CompanySharesInfoRequest(input.Id);
                transaction.Complete();
            }
            _cpsInfoRepository.CloseConnection();
        }

        /// <summary>
        /// Duyet
        /// </summary>
        /// <param name="input"></param>
        public void Approve(ApproveStatusDto input)
        {
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CpsDataTypes.COMPANY_SHARES_INFO);

                if (approve != null)
                {
                    _approveRepository.ApproveRequestStatus(new ApproveRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        ApproveNote = input.ApproveNote
                    });
                }
                _cpsInfoRepository.CompanySharesInfoApprove(input.Id);
                transaction.Complete();
            }
            _cpsInfoRepository.CloseConnection();
        }

        /// <summary>
        /// Xac minh
        /// </summary>
        /// <param name="input"></param>
        public void Check(CheckStatusDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CpsDataTypes.COMPANY_SHARES_INFO);
                if (approve != null)
                {
                    _approveRepository.CheckRequest(new CheckRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        UserCheckId = userid,
                    });
                }
                _cpsInfoRepository.CompanySharesInfoCheck(input.Id);
                transaction.Complete();
            }
            _cpsInfoRepository.CloseConnection();
        }

        /// <summary>
        /// Huy
        /// </summary>
        public void Cancel(CancelStatusDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CpsDataTypes.COMPANY_SHARES_INFO);
                if (approve != null)
                {
                    _approveRepository.CancelRequest(new CancelRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        CancelNote = input.CancelNote,
                    });
                }
                _cpsInfoRepository.CompanySharesInfoCancel(input.Id);
                transaction.Complete();
            }
            _cpsInfoRepository.CloseConnection();
        }

        /// <summary>
        /// Đóng
        /// </summary>
        public void CloseOpen(int id)
        {
            _cpsInfoRepository.CompanySharesInfoClose(id);
        }
    }

}
