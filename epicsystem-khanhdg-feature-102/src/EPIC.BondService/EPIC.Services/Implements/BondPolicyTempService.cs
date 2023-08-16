using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ProductBondPolicyTemp;
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

namespace EPIC.BondDomain.Implements
{
    public class BondPolicyTempService : IBondPolicyTempService
    {
        private readonly ILogger<BondPrimaryService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly BondPolicyTempRepository _productBondPolicyTempRepository;
        private readonly IHttpContextAccessor _httpContext;

        public BondPolicyTempService(
            ILogger<BondPrimaryService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _productBondPolicyTempRepository = new BondPolicyTempRepository(_connectionString, _logger);
            _httpContext = httpContext;
        }

        public void Add(CreateProductBondPolicyTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            using (TransactionScope scope = new TransactionScope())
            {
                var policyTemp = _productBondPolicyTempRepository.Add(new BondPolicyTemp
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

                var listPolicyDetailTemp = input?.ProductBondPolicyDetailTemp;
                if(listPolicyDetailTemp != null)
                {
                    foreach (var policyDetailTemp in listPolicyDetailTemp)
                    {
                        var bondPolicyDetailTemp = new BondPolicyDetailTemp()
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
                        if (bondPolicyDetailTemp.InterestType == InterestTypes.DINH_KY)
                        {
                            if (bondPolicyDetailTemp.InterestPeriodQuantity == null || bondPolicyDetailTemp.InterestPeriodType == null)
                            {
                                throw new FaultException(new FaultReason($" Kỳ hạn {policyDetailTemp.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                            }
                        }
                        _productBondPolicyTempRepository.AddPolicyDetailTemp(bondPolicyDetailTemp);
                    }
                }   
                scope.Complete();
            }

            _productBondPolicyTempRepository.CloseConnection();
        }

        public void AddBondPolicyDetailTemp(ProductBondPolicyDetailTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            _productBondPolicyTempRepository.AddPolicyDetailTemp(new BondPolicyDetailTemp
            {
                PolicyTempId = input.BondPolicyTempId,
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

        public int ChangeStatusProductBondPolicyDetailTemp(int id)
        {
            var productBondPolicyDetailTemp = FindProductBondPolicyDetailTempById(id);
            var status = BondPolicyDetailTemplate.ACTIVE;
            if (productBondPolicyDetailTemp.Status == BondPolicyDetailTemplate.ACTIVE)
            {
                status = BondPolicyDetailTemplate.DEACTIVE;
            }
            else
            {
                status = BondPolicyTemplate.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _productBondPolicyTempRepository.UpdateStatusProductBondPolicyDetailTemp(id, status, username);
        }

        public int ChangeStatusProductBondPolicyTemp(int id)
        {
            var productBondPolicyTemp = FindProductBondPolicyTempById(id);
            var status = BondPolicyTemplate.ACTIVE;
            if (productBondPolicyTemp.Status == BondPolicyTemplate.ACTIVE)
            {
                status = BondPolicyTemplate.DEACTIVE;
            }
            else
            {
                status = BondPolicyTemplate.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _productBondPolicyTempRepository.UpdateStatusProductBondPolicyTemp(id, status, username);
        }

        public int DeleteProductBondPolicyDetailTemp(int id)
        {
            return _productBondPolicyTempRepository.DeleteProductBondPolicyDetailTemp(id);
        }

        public int DeleteProductBondPolicyTemp(int id)
        {
            return _productBondPolicyTempRepository.Delete(id);
        }

        public PagingResult<ViewProductBondPolicyTempDto> FindAll(int pageSize, int pageNumber, bool isNoPaging, string keyword, string status, decimal? classify)
        {
            var query = _productBondPolicyTempRepository.FindAllProductBondPolicyTemp(pageSize, isNoPaging ? -1 : pageNumber, keyword, status, classify);
            var result = new PagingResult<ViewProductBondPolicyTempDto>
            {
                TotalItems = query.TotalItems,
            };

            var items = new List<ViewProductBondPolicyTempDto>() { };

            if (query.Items != null)
            {
                var groupByPolicyTempList = query.Items.GroupBy(item => item.Id)
                            .Select(gr => new ViewProductBondPolicyTempDto()
                            {
                                BondPolicyTempId = gr.Key,
                            });

                foreach (var tmpPolicyTemp in groupByPolicyTempList)
                {
                    var firstPolicyTemp = query.Items.FirstOrDefault(p => p.Id == tmpPolicyTemp.BondPolicyTempId);
                    var policyTemp = new ViewProductBondPolicyTempDto()
                    {
                        BondPolicyTempId = firstPolicyTemp.Id,
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
                        ProductBondPolicyDetailTemp = new List<ViewProductBondPolicyDetailTempDto>() { },
                    };

                    var policyDetailTempList = query.Items.Where(p => p.Id == firstPolicyTemp.Id).OrderBy(x => x.DeStt).ToList();
                    foreach(var tmpPolicyDetailTemp in policyDetailTempList)
                    {   if(tmpPolicyDetailTemp.PolicyDetailTempId > 0)
                        {
                            var policyDetailTemp = new ViewProductBondPolicyDetailTempDto()
                            {
                                BondPolicyDetailTempId = tmpPolicyDetailTemp.PolicyDetailTempId,
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
                            policyTemp.ProductBondPolicyDetailTemp.Add(policyDetailTemp);
                        }      
                    } 
                    items.Add(policyTemp);
                }
            }
            result.Items = items;

            return result;
        }

        public ViewProductBondPolicyTempDto FindById(int id)
        {
            var bondPolicyTempList = _productBondPolicyTempRepository.FindProductBondPolicyById(id);
            var policyTemp = bondPolicyTempList.FirstOrDefault();
            var result = new ViewProductBondPolicyTempDto()
            {
                BondPolicyTempId = policyTemp.Id,
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
                ProductBondPolicyDetailTemp = new List<ViewProductBondPolicyDetailTempDto>() { },
            };
            foreach(var bondPolicyTemp in bondPolicyTempList)
            {
                if (bondPolicyTemp.PolicyDetailTempId > 0)
                {
                    var policyDetailTemp = new ViewProductBondPolicyDetailTempDto()
                    {
                        BondPolicyDetailTempId = bondPolicyTemp.PolicyDetailTempId,
                        Name = bondPolicyTemp.DeName,
                        InterestDays = bondPolicyTemp.DeInterestDays,
                        InterestPeriodQuantity = bondPolicyTemp.DeInterestPeriodQuantity,
                        InterestPeriodType = bondPolicyTemp.DeInterestPeriodType,
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
                        CreatedDate = bondPolicyTemp.DeCreatedDate
                    };
                    result.ProductBondPolicyDetailTemp.Add(policyDetailTemp);
                }
            }

            return result;
        }

        public BondPolicyDetailTemp FindProductBondPolicyDetailTempById(int id)
        {
            return _productBondPolicyTempRepository.FindBondPolicyDetailTempById(id);
        }

        public BondPolicyTemp FindProductBondPolicyTempById(int id)
        {
            return _productBondPolicyTempRepository.FindById(id);
        }

        public int UpdateProductBondPolicyDetailTemp(int id, UpdateProductBondPolicyDetailTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            if (input.InterestType == InterestTypes.DINH_KY)
            {
                if (input.InterestPeriodQuantity == null || input.InterestPeriodType == null)
                {
                    throw new FaultException(new FaultReason($" Kỳ hạn {input.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                }
            }

            var productBondPolicyDetailTemp = FindProductBondPolicyDetailTempById(id);
            productBondPolicyDetailTemp.Name = input.Name;
            productBondPolicyDetailTemp.InterestPeriodQuantity = input.InterestPeriodQuantity;
            productBondPolicyDetailTemp.InterestPeriodType = input.InterestPeriodType;
            productBondPolicyDetailTemp.PeriodQuantity = input.PeriodQuantity;
            productBondPolicyDetailTemp.PeriodType = input.PeriodType;
            productBondPolicyDetailTemp.ShortName = input.ShortName;
            productBondPolicyDetailTemp.Profit = input.Profit;
            productBondPolicyDetailTemp.InterestDays = input.InterestDays;
            productBondPolicyDetailTemp.InterestType = input.InterestType;
            productBondPolicyDetailTemp.STT = input.Stt;
            productBondPolicyDetailTemp.ModifiedBy = username;
            return _productBondPolicyTempRepository.UpdateProductBondPolicyDetailTemp(productBondPolicyDetailTemp);
        }

        public int UpdateProductBondPolicyTemp(int id, UpdateProductBondPolicyTempDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var productBondPolicyTemp = FindProductBondPolicyTempById(id);
            productBondPolicyTemp.Code = input.Code;
            productBondPolicyTemp.Name = input.Name;
            productBondPolicyTemp.Type = input.Type;
            productBondPolicyTemp.InvestorType = input.InvestorType;
            productBondPolicyTemp.IncomeTax = input.IncomeTax;
            productBondPolicyTemp.MinMoney = input.MinMoney;
            productBondPolicyTemp.IsTransfer = input.IsTransfer;
            productBondPolicyTemp.TransferTax = input.TransferTax;
            productBondPolicyTemp.Classify = input.Classify;
            productBondPolicyTemp.ModifiedBy = username;
            return _productBondPolicyTempRepository.Update(productBondPolicyTemp);
        }
    }
}
