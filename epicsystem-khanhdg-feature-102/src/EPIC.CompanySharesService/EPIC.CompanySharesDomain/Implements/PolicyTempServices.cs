using EPIC.DataAccess.Models;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ProductBondPolicyTemp;
using EPIC.Entities;
using EPIC.Utils;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using EPIC.CompanySharesDomain.Interfaces;
using EPIC.CompanySharesRepositories;
using EPIC.CompanySharesEntities.Dto.PolicyTemp;
using EPIC.CompanySharesEntities.DataEntities;

namespace EPIC.CompanySharesDomain.Implements
{
    public class PolicyTempServices : IPolicyTempServices
    {
        private readonly ILogger<PolicyTempServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly PolicyTempRepository _policyTempRepository;
        private readonly IHttpContextAccessor _httpContext;

        public PolicyTempServices(
            ILogger<PolicyTempServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _policyTempRepository = new PolicyTempRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }

        public void Add(CreatePolicyTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            using (TransactionScope scope = new TransactionScope())
            {
                var policyTemp = _policyTempRepository.Add(new PolicyTemp
                {
                    Code = input.Code,
                    Name = input.Name,
                    Type = input.Type,
                    InvestorType = input.InvestorType,
                    IncomeTax = input.IncomeTax,
                    MinMoney = input.MinMoney,
                    IsTransfer = input.IsTransfer,
                    TransferTax = input.TransferTax,
                    Classify = input.Classify,
                    CreatedBy = input.Code
                });

                var listPolicyDetailTemp = input?.PolicyDetailTemp;
                if (listPolicyDetailTemp != null)
                {
                    foreach (var policyDetailTemp in listPolicyDetailTemp)
                    {
                        var CPSPolicyDetailTemp = new PolicyDetailTemp()
                        {
                            PolicyTempId = policyTemp.Id,
                            Name = policyDetailTemp.Name,
                            InterestPeriodQuantity = policyDetailTemp.InterestPeriodQuantity,
                            InterestPeriodType = policyDetailTemp.InterestPeriodType,
                            PeriodQuantity = policyDetailTemp.PeriodQuantity,
                            PeriodType = policyDetailTemp.PeriodType,
                            ShortName = policyDetailTemp.ShortName,
                            Profit = policyDetailTemp.Profit,
                            InterestDays = policyDetailTemp.InterestDays,
                            InterestType = policyDetailTemp.InterestType,
                            STT = policyDetailTemp.Stt,
                            CreatedBy = username
                        };
                        if (CPSPolicyDetailTemp.InterestType == InterestTypes.DINH_KY)
                        {
                            if (CPSPolicyDetailTemp.InterestPeriodQuantity == null || CPSPolicyDetailTemp.InterestPeriodType == null)
                            {
                                throw new FaultException(new FaultReason($" Kỳ hạn {policyDetailTemp.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                            }
                        }
                        _policyTempRepository.AddPolicyDetailTemp(CPSPolicyDetailTemp);
                    }
                }
                scope.Complete();
            }

            _policyTempRepository.CloseConnection();
        }

        public void AddPolicyDetailTemp(PolicyDetailTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _policyTempRepository.AddPolicyDetailTemp(new PolicyDetailTemp
            {
                PolicyTempId = input.CPSPolicyTempId,
                Name = input.Name,
                InterestPeriodQuantity = input.InterestPeriodQuantity,
                InterestPeriodType = input.InterestPeriodType,
                PeriodQuantity = input.PeriodQuantity,
                PeriodType = input.PeriodType,
                ShortName = input.ShortName,
                Profit = input.Profit,
                InterestDays = input.InterestDays,
                InterestType = input.InterestType,
                STT = input.Stt,
                CreatedBy = username
            });
        }

        public int ChangeStatusPolicyDetailTemp(int id)
        {
            var policyDetailTemp = FindPolicyDetailTempById(id);
            var status = BondPolicyDetailTemplate.ACTIVE;
            if (policyDetailTemp.Status == BondPolicyDetailTemplate.ACTIVE)
            {
                status = BondPolicyDetailTemplate.DEACTIVE;
            }
            else
            {
                status = BondPolicyTemplate.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _policyTempRepository.UpdateStatusPolicyDetailTemp(id, status, username);
        }

        public int ChangeStatusPolicyTemp(int id)
        {
            var policyTemp = FindPolicyTempById(id);
            var status = BondPolicyTemplate.ACTIVE;
            if (policyTemp.Status == BondPolicyTemplate.ACTIVE)
            {
                status = BondPolicyTemplate.DEACTIVE;
            }
            else
            {
                status = BondPolicyTemplate.ACTIVE;
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
            return _policyTempRepository.Delete(id);
        }

        public PagingResult<ViewPolicyTempDto> FindAll(int pageSize, int pageNumber, bool isNoPaging, string keyword, string status, decimal? classify)
        {
            var query = _policyTempRepository.FindAllPolicyTemp(pageSize, isNoPaging ? -1 : pageNumber, keyword, status, classify);
            var result = new PagingResult<ViewPolicyTempDto>
            {
                TotalItems = query.TotalItems,
            };

            var items = new List<ViewPolicyTempDto>() { };

            if (query.Items != null)
            {
                var groupByPolicyTempList = query.Items.GroupBy(item => item.Id)
                            .Select(gr => new ViewPolicyTempDto()
                            {
                                Id = gr.Key,
                            });

                foreach (var tmpPolicyTemp in groupByPolicyTempList)
                {
                    var firstPolicyTemp = query.Items.FirstOrDefault(p => p.Id == tmpPolicyTemp.Id);
                    var policyTemp = new ViewPolicyTempDto()
                    {
                        Id = firstPolicyTemp.Id,
                        Code = firstPolicyTemp.Code,
                        Name = firstPolicyTemp.Name,
                        Type = firstPolicyTemp.Type,
                        IncomeTax = firstPolicyTemp.IncomeTax,
                        InvestorType = firstPolicyTemp.InvestorType,
                        MinMoney = firstPolicyTemp.MinMoney,
                        Status = firstPolicyTemp.Status,
                        IsTransfer = firstPolicyTemp.IsTransfer,
                        TransferTax = firstPolicyTemp.TransferTax,
                        Classify = firstPolicyTemp.Classify,
                        CreatedBy = firstPolicyTemp.CreatedBy,
                        CreatedDate = firstPolicyTemp.CreatedDate,
                        ModifiedBy = firstPolicyTemp.ModifiedBy,
                        ModifiedDate = firstPolicyTemp.ModifiedDate,
                        PolicyDetailTemp = new List<ViewPolicyDetailTempDto>() { },
                    };

                    var policyDetailTempList = query.Items.Where(p => p.Id == firstPolicyTemp.Id).OrderBy(x => x.DeStt).ToList();
                    foreach (var tmpPolicyDetailTemp in policyDetailTempList)
                    {
                        if (tmpPolicyDetailTemp.Id > 0)
                        {
                            var policyDetailTemp = new ViewPolicyDetailTempDto()
                            {
                                Id = tmpPolicyDetailTemp.Id,
                                Name = tmpPolicyDetailTemp.DeName,
                                InterestDays = tmpPolicyDetailTemp.DeInterestDays,
                                InterestPeriodQuantity = tmpPolicyDetailTemp.DeInterestPeriodQuantity,
                                InterestPeriodType = tmpPolicyDetailTemp.DeInterestPeriodType,
                                PeriodQuantity = tmpPolicyDetailTemp.DePeriodQuantity,
                                PeriodType = tmpPolicyDetailTemp.DePeriodType,
                                Status = tmpPolicyDetailTemp.DeStatus,
                                CreatedBy = tmpPolicyDetailTemp.DeCreatedBy,
                                ModifiedBy = tmpPolicyDetailTemp.DeModifiedBy,
                                ModifiedDate = tmpPolicyDetailTemp.DeModifiedDate,
                                ShortName = tmpPolicyDetailTemp.DeShortName,
                                Profit = tmpPolicyDetailTemp.DeProfit,
                                Stt = tmpPolicyDetailTemp.DeStt,
                                InterestType = tmpPolicyDetailTemp.DeInterestType,
                                CreatedDate = tmpPolicyDetailTemp.DeCreatedDate
                            };
                            policyTemp.PolicyDetailTemp.Add(policyDetailTemp);
                        }
                    }
                    items.Add(policyTemp);
                }
            }
            result.Items = items;

            return result;
        }

        public ViewPolicyTempDto FindById(int id)
        {
            var policyTempList = _policyTempRepository.FindPolicyById(id);
            var policyTemp = policyTempList.FirstOrDefault();
            var result = new ViewPolicyTempDto()
            {
                Id = policyTemp.Id,
                Code = policyTemp.Code,
                Name = policyTemp.Name,
                Type = policyTemp.Type,
                IncomeTax = policyTemp.IncomeTax,
                InvestorType = policyTemp.InvestorType,
                MinMoney = policyTemp.MinMoney,
                Status = policyTemp.Status,
                IsTransfer = policyTemp.IsTransfer,
                TransferTax = policyTemp.TransferTax,
                Classify = policyTemp.Classify,
                CreatedBy = policyTemp.CreatedBy,
                CreatedDate = policyTemp.CreatedDate,
                ModifiedBy = policyTemp.ModifiedBy,
                ModifiedDate = policyTemp.ModifiedDate,
                PolicyDetailTemp = new List<ViewPolicyDetailTempDto>() { },
            };
            foreach (var CPSpolicyTemp in policyTempList)
            {
                if (CPSpolicyTemp.PolicyDetailTempId > 0)
                {
                    var policyDetailTemp = new ViewPolicyDetailTempDto()
                    {
                        Id = CPSpolicyTemp.PolicyDetailTempId,
                        Name = CPSpolicyTemp.DeName,
                        InterestDays = CPSpolicyTemp.DeInterestDays,
                        InterestPeriodQuantity = CPSpolicyTemp.DeInterestPeriodQuantity,
                        InterestPeriodType = CPSpolicyTemp.DeInterestPeriodType,
                        PeriodQuantity = CPSpolicyTemp.DePeriodQuantity,
                        PeriodType = CPSpolicyTemp.DePeriodType,
                        Status = CPSpolicyTemp.DeStatus,
                        CreatedBy = CPSpolicyTemp.DeCreatedBy,
                        ModifiedBy = CPSpolicyTemp.DeModifiedBy,
                        ModifiedDate = CPSpolicyTemp.DeModifiedDate,
                        ShortName = CPSpolicyTemp.DeShortName,
                        Profit = CPSpolicyTemp.DeProfit,
                        Stt = CPSpolicyTemp.DeStt,
                        InterestType = CPSpolicyTemp.DeInterestType,
                        CreatedDate = CPSpolicyTemp.DeCreatedDate
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
            return _policyTempRepository.FindById(id);
        }

        public int UpdatePolicyDetailTemp(int id, UpdatePolicyDetailTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            if (input.InterestType == InterestTypes.DINH_KY)
            {
                if (input.InterestPeriodQuantity == null || input.InterestPeriodType == null)
                {
                    throw new FaultException(new FaultReason($" Kỳ hạn {input.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }

            var policyDetailTemp = FindPolicyDetailTempById(id);
            policyDetailTemp.Name = input.Name;
            policyDetailTemp.InterestPeriodQuantity = input.InterestPeriodQuantity;
            policyDetailTemp.InterestPeriodType = input.InterestPeriodType;
            policyDetailTemp.PeriodQuantity = input.PeriodQuantity;
            policyDetailTemp.PeriodType = input.PeriodType;
            policyDetailTemp.ShortName = input.ShortName;
            policyDetailTemp.Profit = input.Profit;
            policyDetailTemp.InterestDays = input.InterestDays;
            policyDetailTemp.InterestType = input.InterestType;
            policyDetailTemp.STT = input.Stt;
            policyDetailTemp.ModifiedBy = username;
            return _policyTempRepository.UpdatePolicyDetailTemp(policyDetailTemp);
        }

        public int UpdatePolicyTemp(int id, UpdatePolicyTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var policyTemp = FindPolicyTempById(id);
            policyTemp.Code = input.Code;
            policyTemp.Name = input.Name;
            policyTemp.Type = input.Type;
            policyTemp.InvestorType = input.InvestorType;
            policyTemp.IncomeTax = input.IncomeTax;
            policyTemp.MinMoney = input.MinMoney;
            policyTemp.IsTransfer = input.IsTransfer;
            policyTemp.TransferTax = input.TransferTax;
            policyTemp.Classify = input.Classify;
            policyTemp.ModifiedBy = username;
            return _policyTempRepository.Update(policyTemp);
        }
    }
}
