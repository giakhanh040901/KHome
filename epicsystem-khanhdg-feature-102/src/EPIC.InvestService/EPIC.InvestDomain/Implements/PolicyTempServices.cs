using AutoMapper;
using DocumentFormat.OpenXml.Wordprocessing;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.PolicyTemp;
using EPIC.InvestRepositories;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Invest;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Transactions;

namespace EPIC.InvestDomain.Implements
{
    public class PolicyTempServices : IPolicyTempServices
    {
        private readonly ILogger<PolicyTempServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly PolicyTempRepository _policyTempRepository;
        private readonly InvestPolicyTempEFRepository _policyTempEFRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;


        public PolicyTempServices(
            EpicSchemaDbContext dbContext,
            ILogger<PolicyTempServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext, IMapper mapper)
        {
            _logger = logger;
            _dbContext = dbContext;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _policyTempRepository = new PolicyTempRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _policyTempEFRepository = new InvestPolicyTempEFRepository(_dbContext, _logger);
            _mapper = mapper;
        }

        public ViewPolicyTempDto Add(CreatePolicyTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            ViewPolicyTempDto result = null;
            using (TransactionScope scope = new TransactionScope())
            {
                var policyTemp = _policyTempRepository.AddPolicyTemp(new PolicyTemp
                {
                    Code = input.Code,
                    Name = input.Name,
                    Type = input.Type,
                    IncomeTax = input.IncomeTax,
                    MinMoney = input.MinMoney,
                    MaxMoney = input.MaxMoney,
                    IsTransfer = input.IsTransfer,
                    TransferTax = input.TransferTax,
                    Classify = input.Classify,
                    MinWithdraw = input.MinWithdraw,
                    CalculateType = input.CalculateType,
                    ExitFee = input.ExitFee,
                    ExitFeeType = input.ExitFeeType,
                    TradingProviderId = tradingProviderId,
                    Description = input.Description,
                    PolicyDisplayOrder = input.PolicyDisplayOrder,
                    RenewalsType = input.RenewalsType,
                    RemindRenewals = input.RemindRenewals,
                    ExpirationRenewals = input.ExpirationRenewals,
                    MaxWithDraw = input.MaxWithDraw,
                    MinTakeContract = input.MinTakeContract,
                    MinInvestDay = input.MinInvestDay,
                    CalculateWithdrawType = input.CalculateWithdrawType,
                    ProfitRateDefault = input.CalculateWithdrawType == InvestCalculateWithdrawTypes.KY_HAN_THAP_HON_GAN_NHAT ? 0 : input.ProfitRateDefault,
                    CreatedBy = username
                });
                result = _mapper.Map<ViewPolicyTempDto>(policyTemp);
                var listPolicyDetailTemp = input?.PolicyDetailTemp;
                if (listPolicyDetailTemp != null)
                {
                    foreach (var policyDetailTemp in listPolicyDetailTemp)
                    {
                        var investPolicyDetailTemp = new PolicyDetailTemp
                        {
                            PolicyTempId = policyTemp.Id,
                            Name = policyDetailTemp.Name,
                            PeriodQuantity = policyDetailTemp.PeriodQuantity,
                            PeriodType = policyDetailTemp.PeriodType,
                            ShortName = policyDetailTemp.ShortName,
                            Profit = policyDetailTemp.Profit,
                            InterestDays = policyDetailTemp.InterestDays,
                            STT = policyDetailTemp.Stt,
                            InterestType = policyDetailTemp.InterestType,
                            InterestPeriodType = policyDetailTemp.InterestPeriodType,
                            InterestPeriodQuantity = policyDetailTemp.InterestPeriodQuantity,
                            FixedPaymentDate = policyDetailTemp.FixedPaymentDate,
                            CreatedBy = username
                        };

                        if (policyTemp.Type == InvPolicyType.FIXED_PAYMENT_DATE)
                        {
                            if (investPolicyDetailTemp.FixedPaymentDate == null || investPolicyDetailTemp.InterestPeriodType == null)
                            {
                                throw new FaultException(new FaultReason($" Kỳ hạn {policyDetailTemp.Name} không được bỏ trống số ngày chi trả cố định"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                            }
                        }
                        else
                        {
                            if (investPolicyDetailTemp.InterestType == InterestTypes.DINH_KY)
                            {
                                if (investPolicyDetailTemp.InterestPeriodQuantity == null || investPolicyDetailTemp.InterestPeriodType == null)
                                {
                                    throw new FaultException(new FaultReason($" Kỳ hạn {policyDetailTemp.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                                }
                            }
                        }
                        _policyTempRepository.AddPolicyDetailTemp(investPolicyDetailTemp);
                    }
                }
                scope.Complete();
            }
            _policyTempRepository.CloseConnection();
            return result;
        }

        public void AddBondPolicyDetailTemp(PolicyDetailTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _policyTempRepository.AddPolicyDetailTemp(new PolicyDetailTemp
            {
                PolicyTempId = input.PolicyTempId,
                Name = input.Name,
                PeriodQuantity = input.PeriodQuantity,
                PeriodType = input.PeriodType,
                ShortName = input.ShortName,
                Profit = input.Profit,
                InterestDays = input.InterestDays,
                STT = input.Stt,
                InterestType = input.InterestType,
                InterestPeriodType = input.InterestPeriodType,
                InterestPeriodQuantity = input.InterestPeriodQuantity,
                FixedPaymentDate = input.FixedPaymentDate,
                CreatedBy = username
            });
        }

        public int ChangeStatusPolicyDetailTemp(int id)
        {
            var policyDetailTemp = FindPolicyDetailTempById(id);
            var status = INVPolicyDetailTemplateStatus.ACTIVE;
            if (policyDetailTemp.Status == INVPolicyDetailTemplateStatus.ACTIVE)
            {
                status = INVPolicyDetailTemplateStatus.DEACTIVE;
            }
            else
            {
                status = INVPolicyDetailTemplateStatus.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _policyTempRepository.UpdateStatusPolicyDetailTemp(id, status, username);
        }

        public int ChangeStatusPolicyTemp(int id)
        {
            var policyTemp = FindPolicyTempById(id);
            var status = INVPolicyTemplateStatus.ACTIVE;
            if (policyTemp.Status == INVPolicyTemplateStatus.ACTIVE)
            {
                status = INVPolicyTemplateStatus.DEACTIVE;
            }
            else
            {
                status = INVPolicyTemplateStatus.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _policyTempRepository.UpdateStatusPolicyTemp(id, status, username);
        }

        public int DeletePolicyDetailTemp(int id)
        {
            return _policyTempRepository.DeletePolicyDetailTemp(id);
        }

        public int DeletePolicyTemp(int id)
        {
            return _policyTempRepository.DeletePolicyTemp(id);
        }

        public PagingResult<ViewPolicyTempDto> FindAll(FilterPolicyTempDto input)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                var partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
                if (!_dbContext.TradingProviderPartners.Any(t => t.TradingProviderId == input.TradingProviderId && t.PartnerId == partnerId && t.Deleted == YesNo.NO))
                {
                    return new PagingResult<ViewPolicyTempDto>();
                }
            }

            input.TradingProviderId = tradingProviderId;
            var query = _policyTempEFRepository.FindAll(input);
            var result = new PagingResult<ViewPolicyTempDto>();
            result.TotalItems = query.TotalItems;

            var listItem = new List<ViewPolicyTempDto>();

            foreach (var item in query.Items)
            {
                var resultItem = _mapper.Map<ViewPolicyTempDto>(item);
                resultItem.PolicyDetailTemp = _mapper.Map<List<ViewPolicyDetailTempDto>>(item.PolicyDetailTemps);
                listItem.Add(resultItem);
            }
            result.Items = listItem;
            return result;
        }

        public ViewPolicyTempDto FindById(int id)
        {
            var bondPolicyTempList = _policyTempRepository.FindById(id);
            var policyTemp = bondPolicyTempList.FirstOrDefault();
            var result = new ViewPolicyTempDto()
            {
                Id = policyTemp.Id,
                Code = policyTemp.Code,
                Name = policyTemp.Name,
                Type = policyTemp.Type,
                IncomeTax = policyTemp.IncomeTax,
                MinMoney = policyTemp.MinMoney,
                MaxMoney = policyTemp.MaxMoney,
                Status = policyTemp.Status,
                IsTransfer = policyTemp.IsTransfer,
                TransferTax = policyTemp.TransferTax,
                Classify = policyTemp.Classify,
                MinWithdraw = policyTemp.MinWithdraw,
                CalculateType = policyTemp.CalculateType,
                ExitFee = policyTemp.ExitFee,
                ExitFeeType = policyTemp.ExitFeeType,
                PolicyDisplayOrder = policyTemp.PolicyDisplayOrder,
                Description = policyTemp.Description,
                RenewalsType = policyTemp.RenewalsType,
                RemindRenewals = policyTemp.RemindRenewals,
                ExpirationRenewals = policyTemp.ExpirationRenewals,
                MaxWithDraw = policyTemp.MaxWithDraw,
                MinTakeContract = policyTemp.MinTakeContract,
                MinInvestDay = policyTemp.MinInvestDay,
                CreatedBy = policyTemp.CreatedBy,
                CreatedDate = policyTemp.CreatedDate,
                ModifiedBy = policyTemp.ModifiedBy,
                ModifiedDate = policyTemp.ModifiedDate,
                ProfitRateDefault = policyTemp.ProfitRateDefault,
                CalculateWithdrawType = policyTemp.CalculateWithdrawType,
                PolicyDetailTemp = new List<ViewPolicyDetailTempDto>() { },
            };
            foreach (var bondPolicyTemp in bondPolicyTempList)
            {
                if (bondPolicyTemp.DeId > 0)
                {
                    var policyDetailTemp = new ViewPolicyDetailTempDto()
                    {
                        Id = bondPolicyTemp.DeId,
                        Name = bondPolicyTemp.DeName,
                        InterestDays = bondPolicyTemp.DeInterestDays,  
                        PeriodQuantity = bondPolicyTemp.DePeriodQuantity,
                        PeriodType = bondPolicyTemp.DePeriodType,
                        Status = bondPolicyTemp.DeStatus,
                        CreatedBy = bondPolicyTemp.DeCreatedBy,
                        ModifiedBy = bondPolicyTemp.DeModifiedBy,
                        ModifiedDate = bondPolicyTemp.DeModifiedDate,
                        ShortName = bondPolicyTemp.DeShortName,
                        Profit = bondPolicyTemp.DeProfit,
                        Stt = bondPolicyTemp.DeStt,
                        InterestType = bondPolicyTemp.DeInterestType,
                        InterestPeriodType = bondPolicyTemp.DeInterestPeriodType,
                        InterestPeriodQuantity = bondPolicyTemp.DeInterestPeriodQuantity,
                        FixedPaymentDate = bondPolicyTemp.DeFixedPaymentDate,
                        CreatedDate = bondPolicyTemp.DeCreatedDate
                    };
                    result.PolicyDetailTemp.Add(policyDetailTemp);
                }
            }

            return result;
        }

        public PolicyDetailTemp FindPolicyDetailTempById(int id)
        {
            return _policyTempRepository.FindPolicyDetailTempById(id);
        }

        public PolicyTemp FindPolicyTempById(int id)
        {
            return _policyTempRepository.FindPolicyTempById(id);
        }

        public int UpdateProductBondPolicyDetailTemp(int id, UpdatePolicyDetailTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var productBondPolicyDetailTemp = FindPolicyDetailTempById(id);
            var productPolicyTemp = _policyTempRepository.FindPolicyTempById(productBondPolicyDetailTemp.PolicyTempId);
            if (productPolicyTemp != null)
            {
                if (productPolicyTemp.Type == InvPolicyType.FIXED_PAYMENT_DATE)
                {
                    if (input.FixedPaymentDate == null || input.InterestPeriodType == null)
                    {
                        throw new FaultException(new FaultReason($" Kỳ hạn {input.Name} không được bỏ trống số ngày chi trả cố định"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }
                }
                else
                {
                    if (input.InterestType == InterestTypes.DINH_KY)
                    {
                        if (input.InterestPeriodQuantity == null || input.InterestPeriodType == null)
                        {
                            throw new FaultException(new FaultReason($" Kỳ hạn {input.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                        }
                    }
                }
            }

            productBondPolicyDetailTemp.Name = input.Name; 
            productBondPolicyDetailTemp.PeriodQuantity = input.PeriodQuantity;
            productBondPolicyDetailTemp.PeriodType = input.PeriodType;
            productBondPolicyDetailTemp.ShortName = input.ShortName;
            productBondPolicyDetailTemp.Profit = input.Profit;
            productBondPolicyDetailTemp.InterestDays = input.InterestDays;
            productBondPolicyDetailTemp.InterestType = input.InterestType;
            productBondPolicyDetailTemp.InterestPeriodType = input.InterestPeriodType;
            productBondPolicyDetailTemp.InterestPeriodQuantity = input.InterestPeriodQuantity;
            productBondPolicyDetailTemp.STT = input.Stt;
            productBondPolicyDetailTemp.FixedPaymentDate = input.FixedPaymentDate;
            productBondPolicyDetailTemp.ModifiedBy = username;

            
            return _policyTempRepository.UpdatePolicyDetailTemp(productBondPolicyDetailTemp);
        }

        public int UpdatePolicyTemp(int id, UpdatePolicyTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var policyTemp = FindPolicyTempById(id);
            policyTemp.Code = input.Code;
            policyTemp.Name = input.Name;
            policyTemp.Type = input.Type;
            policyTemp.IncomeTax = input.IncomeTax;
            policyTemp.MinMoney = input.MinMoney;
            policyTemp.MaxMoney = input.MaxMoney;
            policyTemp.IsTransfer = input.IsTransfer;
            policyTemp.TransferTax = input.TransferTax;
            policyTemp.Classify = input.Classify;
            policyTemp.MinWithdraw = input.MinWithDraw;
            policyTemp.CalculateType = input.CalculateType;
            policyTemp.ExitFee = input.ExitFee;
            policyTemp.ExitFeeType = input.ExitFeeType;
            policyTemp.ModifiedBy = username;
            policyTemp.Description = input.Description;
            policyTemp.PolicyDisplayOrder = input.PolicyDisplayOrder;
            policyTemp.RenewalsType = input.RenewalsType;
            policyTemp.RemindRenewals = input.RemindRenewals;
            policyTemp.ExpirationRenewals = input.ExpirationRenewals;
            policyTemp.MaxWithDraw = input.MaxWithDraw;
            policyTemp.MinTakeContract = input.MinTakeContract;
            policyTemp.MinInvestDay = input.MinInvestDay;
            policyTemp.CalculateWithdrawType = input.CalculateWithdrawType;
            policyTemp.ProfitRateDefault = input.CalculateWithdrawType == InvestCalculateWithdrawTypes.KY_HAN_THAP_HON_GAN_NHAT ? 0 : input.ProfitRateDefault;
            return _policyTempRepository.UpdatePolicyTemp(policyTemp);
        }

        public PagingResult<ViewPolicyTemp> FindAllPolicyTempNoPermission(string status)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }

            var result = _policyTempRepository.FindAllPolicyTempNoPermission(tradingProviderId, status);
            foreach(var policyTemp in result.Items)
            {
                var listPolicyDetail = _policyTempRepository.FindBondPolicyDetailTempByPolicyTempId(policyTemp.Id);
                if (listPolicyDetail != null)
                {
                    policyTemp.PolicyDetailTemps = _mapper.Map<List<PolicyDetailTempDto>>(listPolicyDetail);
                }
            }

            return result;
        }
    }
}
