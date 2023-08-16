using AutoMapper;
using DocumentFormat.OpenXml.Bibliography;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.CoreRepositoryExtensions;
using EPIC.CoreSharedServices.Interfaces;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.BusinessCustomer;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Entities.Dto.Sale;
using EPIC.GarnerEntities.DataEntities;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.Distribution;
using EPIC.InvestEntities.Dto.DistributionNews;
using EPIC.InvestEntities.Dto.DistributionVideo;
using EPIC.InvestEntities.Dto.GeneralContractor;
using EPIC.InvestEntities.Dto.InvConfigContractCode;
using EPIC.InvestEntities.Dto.InvConfigContractCodeDetail;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestEntities.Dto.InvestProject;
using EPIC.InvestEntities.Dto.Owner;
using EPIC.InvestEntities.Dto.Policy;
using EPIC.InvestEntities.Dto.Project;
using EPIC.InvestEntities.Dto.ProjectImage;
using EPIC.InvestEntities.Dto.ProjectInformationShare;
using EPIC.InvestEntities.Dto.ProjectOverViewFile;
using EPIC.InvestEntities.Dto.ProjectOverviewOrg;
using EPIC.InvestEntities.Dto.ProjectType;
using EPIC.InvestRepositories;
using EPIC.RealEstateEntities.Dto.RstProjectInformationShare;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Core;
using EPIC.Utils.ConstantVariables.Garner;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.RealEstate;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.Linq;
using Hangfire;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text.Json;
using System.Transactions;

namespace EPIC.InvestDomain.Implements
{
    public class DistributionServices : IDistributionServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<DistributionServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly ILogicInvestorTradingSharedServices _logicInvestorTradingSharedServices;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly DistributionNewsRepository _distributionNewsRepository;
        private readonly DistributionVideoRepository _distributionVideoRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly InvestApproveRepository _investApproveRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly OwnerRepository _ownerRepository;
        private readonly GeneralContractorRepository _generalContractorRepository;
        private readonly PolicyTempRepository _policyTempRepository;
        private readonly ApproveRepository _approveRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly DistriPolicyFileRepository _distriPolicyFileRepository;
        private readonly ProjectJuridicalFileRepository _juridicalFileRepository;
        private readonly DistributionFileRepository _distributionFileRepository;
        private readonly InvestOrderRepository _orderRepository;
        private readonly InvestorSaleRepository _investorSaleRepository;
        private readonly ProjectOverviewFileRepository _projectOverviewFileRepository;
        private readonly ProjectOverviewOrgRepository _projectOverviewOrgRepository;
        private readonly WithdrawalRepository _withdrawalRepository;
        private readonly SaleRepository _saleRepository;
        private readonly PartnerRepository _partnerRepository;
        private readonly ProjectImageRepository _projectImageRepository;
        private readonly InvConfigContractCodeEFRepository _invConfigContractCodeEFRepository;
        private readonly InvConfigContractCodeDetailEFRepository _invConfigContractCodeDetailEFRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IInvestSharedServices _investSharedServices;
        private readonly BusinessCustomerEFRepository _businessCustomerEFRepository;
        private readonly SaleEFRepository _saleEFRepository;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly InvestHistoryUpdateEFRepository _investHistoryUpdateEFRepository;
        private readonly DistributionEFRepository _distribibutionEFRepository;
        private readonly IBackgroundJobClient _backgroundJobs;
        private readonly InvestBackgroundJobService _investBackgroundJobService;

        public DistributionServices(
            EpicSchemaDbContext dbContext,
            ILogger<DistributionServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper,
            IInvestSharedServices investSharedServices,
            ILogicInvestorTradingSharedServices logicInvestorTradingSharedServices,
            InvestBackgroundJobService investBackgroundJobService,
            IBackgroundJobClient backgroundJobs)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _logicInvestorTradingSharedServices = logicInvestorTradingSharedServices;
            _defErrorEFRepository = new DefErrorEFRepository(dbContext);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _distribibutionEFRepository = new DistributionEFRepository(_dbContext, _logger);
            _distributionNewsRepository = new DistributionNewsRepository(_connectionString, _logger);
            _distributionVideoRepository = new DistributionVideoRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _generalContractorRepository = new GeneralContractorRepository(_connectionString, _logger);
            _ownerRepository = new OwnerRepository(_connectionString, _logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, _logger);
            _investApproveRepository = new InvestApproveRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _policyTempRepository = new PolicyTempRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _distriPolicyFileRepository = new DistriPolicyFileRepository(_connectionString, _logger);
            _juridicalFileRepository = new ProjectJuridicalFileRepository(_connectionString, _logger);
            _distributionFileRepository = new DistributionFileRepository(_connectionString, _logger);
            _projectImageRepository = new ProjectImageRepository(_connectionString, _logger);
            _orderRepository = new InvestOrderRepository(_connectionString, _logger);
            _investorSaleRepository = new InvestorSaleRepository(_connectionString, _logger);
            _projectOverviewFileRepository = new ProjectOverviewFileRepository(_connectionString, _logger);
            _projectOverviewOrgRepository = new ProjectOverviewOrgRepository(_connectionString, _logger);
            _withdrawalRepository = new WithdrawalRepository(_connectionString, _logger);
            _saleRepository = new SaleRepository(_connectionString, _logger);
            _partnerRepository = new PartnerRepository(_connectionString, _logger);
            _invConfigContractCodeEFRepository = new InvConfigContractCodeEFRepository(dbContext, logger);
            _invConfigContractCodeDetailEFRepository = new InvConfigContractCodeDetailEFRepository(dbContext, logger);
            _httpContext = httpContext;
            _mapper = mapper;
            _investSharedServices = investSharedServices;
            _businessCustomerEFRepository = new BusinessCustomerEFRepository(_dbContext, _logger);
            _saleEFRepository = new SaleEFRepository(_dbContext, _logger);
            _investOrderEFRepository = new InvestOrderEFRepository(_dbContext, _logger);
            _investHistoryUpdateEFRepository = new InvestHistoryUpdateEFRepository(_dbContext, _logger);
            _investBackgroundJobService = investBackgroundJobService;
            _backgroundJobs = backgroundJobs;
        }

        private void AddTradingBankAccount(List<int> TradingBankAcc, List<int> TradingBankAccPays, int distributionId, string username, int tradingProviderId)
        {
            // Thêm nhiều tài khoản ngân hàng thụ hưởng
            if (TradingBankAcc != null)
            {
                var updateTradingBankAccountFind = _distributionRepository.GetAllTradingBankByDistribution(distributionId, DistributionTradingBankAccountTypes.THU);
                //Xóa đi những ngân hàng ko được truyền vào
                var updateTradingBankAccountRemove = updateTradingBankAccountFind.Where(p => !TradingBankAcc.Contains(p.TradingBankAccId)).ToList();
                foreach (var bankAccountItem in updateTradingBankAccountRemove)
                {
                    _distributionRepository.DeletedDistributionTradingBankAcc(bankAccountItem.Id);
                }
                foreach (var item in TradingBankAcc)
                {
                    // Nếu là thêm mới thì thêm vào
                    // Nếu loại ngân hàng chưa có trong list thì thêm vào, nếu đã có thì giữ nguyên
                    _distributionRepository.AddDistributionTradingBankAcc(new DistributionTradingBankAccount
                    {
                        DistributionId = distributionId,
                        TradingBankAccId = item,
                        CreatedBy = username,
                        Type = DistributionTradingBankAccountTypes.THU
                    }, tradingProviderId);
                }
            };

            // Thêm nhiều tài khoản ngân hàng chi
            if (TradingBankAccPays != null)
            {
                var updateTradingBankAccountFind = _distributionRepository.GetAllTradingBankByDistribution(distributionId, DistributionTradingBankAccountTypes.CHI);
                //Xóa đi những ngân hàng ko được truyền vào
                var updateTradingBankAccountRemove = updateTradingBankAccountFind.Where(p => !TradingBankAccPays.Contains(p.TradingBankAccId)).ToList();
                foreach (var bankAccountItem in updateTradingBankAccountRemove)
                {
                    _distributionRepository.DeletedDistributionTradingBankAcc(bankAccountItem.Id);
                }
                foreach (var item in TradingBankAccPays)
                {
                    // Nếu là thêm mới thì thêm vào
                    // Nếu loại ngân hàng chưa có trong list thì thêm vào, nếu đã có thì giữ nguyên
                    _distributionRepository.AddDistributionTradingBankAcc(new DistributionTradingBankAccount
                    {
                        DistributionId = distributionId,
                        TradingBankAccId = item,
                        CreatedBy = username,
                        Type = DistributionTradingBankAccountTypes.CHI
                    }, tradingProviderId);
                }
            };
        }

        public PagingResult<ViewDistributionDto> FindAll(FilterInvestDistributionDto input)
        {
            //string isClose = null;
            if (input.IsActive == true)
            {
                input.IsClose = YesNo.NO;
            }

            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType != UserTypes.EPIC && userType != UserTypes.ROOT_EPIC)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            var result = new PagingResult<ViewDistributionDto>();

            input.TradingProviderId = tradingProviderId;
            var query = _distribibutionEFRepository.FindAll(input);
            result.TotalItems = query.Count();
            query = query.OrderDynamic(input.Sort);
           
            if (input.PageSize != -1)
            {
                query = query.Skip(input.Skip).Take(input.PageSize);
            }
            result.Items = query;
            return result;
        }

        
        public ViewDistributionDto FindById(int id)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            int? partnerId = null;

            if (userType == UserTypes.TRADING_PROVIDER && userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }
            else if (userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                partnerId = CommonUtils.GetCurrentPartnerId(_httpContext);
            }
            var first = _distributionRepository.FindById(id, tradingProviderId, partnerId);

            var result = _mapper.Map<ViewDistributionDto>(first);

            result.Policies = new List<ViewPolicyDto>();

            var projectFind = _projectRepository.FindByTradingProvider(first.ProjectId, null);
            if (projectFind != null)
            {
                result.Project = _mapper.Map<ProjectDto>(projectFind);

                var soTienDaDauTu = _orderRepository.SumValue(null, id);
                //Nếu có tổng mức đầu tư sub thì gán lên 
                if (projectFind.TotalInvestmentSub != null)
                {
                    result.Project.TotalInvestment = projectFind.TotalInvestmentSub;
                }

                var projectType = _projectRepository.FindByProjectId(first.ProjectId);
                if (projectType != null)
                {
                    result.Project.ProjectTypes = projectType.Select(r => r.Type).ToList();
                }

                result.HanMucToiDa = _orderRepository.MaxTotalInvestment(first.TradingProviderId, first.Id);
                result.IsInvested = soTienDaDauTu;
            }
            var generalContractorFind = _generalContractorRepository.FindById(result.Project.GeneralContractorId ?? 0);
            if (generalContractorFind != null)
            {
                result.Project.GeneralContractor = _mapper.Map<ViewGeneralContractorDto>(generalContractorFind);
                var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(result.Project.GeneralContractor.BusinessCustomerId);
                if (businessCustomer != null)
                {
                    result.Project.GeneralContractor.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                }
            }

            var listFileDb = _projectOverviewFileRepository.FindListFile(id, first.TradingProviderId);
            result.ProjectOverviewFiles = _mapper.Map<List<ViewProjectOverViewFileDto>>(listFileDb);

            var overviewOrgsFind = _projectOverviewOrgRepository.FindListOrg(id, first.TradingProviderId);
            result.ProjectOverviewOrgs = _mapper.Map<List<ViewProjectOverviewOrgDto>>(overviewOrgsFind);

            var ownerFind = _ownerRepository.FindById(result.Project.OwnerId ?? 0);
            if (ownerFind != null)
            {
                result.Project.Owner = _mapper.Map<ViewOwnerDto>(ownerFind);
                var businessCustom = _businessCustomerRepository.FindBusinessCustomerById(result.Project.Owner.BusinessCustomerId);
                if (businessCustom != null)
                {
                    result.Project.Owner.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustom);
                }
            }

            var policyList = _policyRepository.GetAllPolicy(result.Id, result.TradingProviderId);
            foreach (var policyItem in policyList)
            {
                var policy = _mapper.Map<ViewPolicyDto>(policyItem);

                policy.PolicyDetail = new List<ViewPolicyDetailDto>();

                var policyDetailList = _policyRepository.GetAllPolicyDetail(policy.Id, result.TradingProviderId);
                foreach (var detailItem in policyDetailList)
                {
                    var detail = _mapper.Map<ViewPolicyDetailDto>(detailItem);
                    policy.PolicyDetail.Add(detail);
                }
                result.Policies.Add(policy);
            }
            var businessCustomerBankFind = _businessCustomerRepository.FindBusinessCusBankById(result.BusinessCustomerBankAccId ?? 0);
            if (businessCustomerBankFind != null)
            {
                result.BusinessCustomerBank = _mapper.Map<BusinessCustomerBankDto>(businessCustomerBankFind);
                result.BusinessCustomerBank.BusinessCustomerBankAccId = businessCustomerBankFind.BusinessCustomerBankAccId;
            }
            var tradingProviderFind = _tradingProviderRepository.FindById(result.TradingProviderId);
            if (tradingProviderFind != null)
            {
                var listBank = _businessCustomerRepository.FindAllBusinessCusBank(tradingProviderFind.BusinessCustomerId, -1, 0, null);
                result.ListBusinessCustomerBanks = _mapper.Map<List<BusinessCustomerBankDto>>(listBank.Items);
            }
            List<DistributionTradingBankAccount> listBankTrading = new List<DistributionTradingBankAccount>();
            listBankTrading = _distributionRepository.GetAllTradingBankByDistribution(id);
            result.TradingBankAccouts = listBankTrading.ToList();

            var listBankId = listBankTrading.Where(e => e.Type == DistributionTradingBankAccountTypes.THU).Select(e => e.TradingBankAccId).ToList();
            result.TradingBankAcc = listBankId;

            result.TradingBankAccPays = listBankTrading.Where(e => e.Type == DistributionTradingBankAccountTypes.CHI).Select(e => e.TradingBankAccId).ToList();
            return result;
        }

        public OverViewDistributionDto FindOverViewById(int id)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUsername(_httpContext);
            if (userType != UserTypes.EPIC || userType != UserTypes.ROOT_EPIC || userType == UserTypes.PARTNER || userType == UserTypes.ROOT_PARTNER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            }
            var first = _distributionRepository.FindById(id, tradingProviderId);

            var result = _mapper.Map<OverViewDistributionDto>(first);

            var listFileDb = _projectOverviewFileRepository.FindListFile(id, first.TradingProviderId);
            result.ProjectOverviewFiles = _mapper.Map<List<ViewProjectOverViewFileDto>>(listFileDb);

            var overviewOrgsFind = _projectOverviewOrgRepository.FindListOrg(id, first.TradingProviderId);
            result.ProjectOverviewOrgs = _mapper.Map<List<ViewProjectOverviewOrgDto>>(overviewOrgsFind);

            return result;
        }

        /// <summary>
        /// Thêm bán phân phối
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Distribution Add(CreateDistributionDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            Distribution distribution = new Distribution();
            List<int> listBankId = new List<int>();
            using (TransactionScope scope = new TransactionScope())
            {
                distribution = _distributionRepository.Add(new Distribution
                {
                    TradingProviderId = tradingProviderId,
                    ProjectId = input.ProjectId,
                    OpenCellDate = input.OpenCellDate,
                    CloseCellDate = input.CloseCellDate,
                    MethodInterest = input.MethodInterest,
                    CreatedBy = username
                });

                //thêm tài khoản thụ hưởng
                AddTradingBankAccount(input.TradingBankAcc, input.TradingBankAccPays, distribution.Id, username, tradingProviderId);

                scope.Complete();
            }
            _distributionRepository.CloseConnection();

            return distribution;
        }

        /// <summary>
        /// Cập nhật bán phân phối
        /// </summary>
        /// <param name="input"></param>
        public void Update(UpdateDistributionDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            using (TransactionScope scope = new())
            {
                _distributionRepository.Update(new Distribution
                {
                    Id = input.Id,
                    TradingProviderId = tradingProviderId,
                    OpenCellDate = input.OpenCellDate,
                    CloseCellDate = input.CloseCellDate,
                    MethodInterest = input.MethodInterest,
                    Image = input.Image,
                    ModifiedBy = username,
                });

                AddTradingBankAccount(input.TradingBankAcc, input.TradingBankAccPays, input.Id, username, tradingProviderId);

                scope.Complete();
            }

            _distributionRepository.CloseConnection();
        }

        /// <summary>
        /// Sửa ngân hàng cho bán phân phối
        /// </summary>
        /// <param name="id"></param>
        /// <param name="businessCustomerBankAccId"></param>
        public void UpdateBank(int id, int businessCustomerBankAccId)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _distributionRepository.UpdateBank(id, businessCustomerBankAccId, username, tradingProviderId);
        }

        /// <summary>
        /// Update nội dung tổng quan
        /// </summary>
        /// <param name="input"></param>
        public void UpdateOverviewContent(UpdateDistributionOverviewContentDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var username = CommonUtils.GetCurrentUsername(_httpContext);
            using (TransactionScope scope = new TransactionScope())
            {
                _distributionRepository.UpdateOverviewContent(input, tradingProviderId);

                #region Xử lý các file tổng quan
                // Lấy danh sách file truyền vào
                var listFileInput = input.ProjectOverviewFiles.ToList();

                //Lấy danh sách file từ Db
                var listFileDb = _projectOverviewFileRepository.FindListFile(input.Id);
                foreach (var fileDb in listFileDb)
                {
                    //Nếu file 
                    var checkFileId = listFileInput.FirstOrDefault(f => f.Id == fileDb.Id);
                    if (checkFileId == null)
                    {
                        _projectOverviewFileRepository.Delete(fileDb.Id);
                    };
                }

                foreach (var file in input.ProjectOverviewFiles)
                {
                    // Thêm mới file
                    if (file.Id == 0)
                    {
                        _projectOverviewFileRepository.Add(new ProjectOverviewFile
                        {
                            DistributionId = input.Id,
                            Title = file.Title,
                            Url = file.Url,
                            CreatedBy = username,
                            SortOrder = file.SortOrder
                        }, tradingProviderId);
                    }
                    //Cập nhật file
                    else
                    {
                        _projectOverviewFileRepository.Update(new ProjectOverviewFile
                        {
                            Id = file.Id,
                            DistributionId = input.Id,
                            Title = file.Title,
                            Url = file.Url,
                            ModifiedBy = username,
                            SortOrder = file.SortOrder
                        }, tradingProviderId);
                    }
                }
                #endregion

                #region Xử lý các thông tin tổ chức liên quan
                // Lấy danh sách file truyền vào
                var listOrgInput = input.ProjectOverviewOrgs.ToList();

                //Lấy danh sách file từ Db
                var listOrgDb = _projectOverviewOrgRepository.FindListOrg(input.Id);
                foreach (var orgItem in listOrgDb)
                {
                    //Nếu file 
                    var checkOrgId = listOrgInput.FirstOrDefault(f => f.Id == orgItem.Id);
                    if (checkOrgId == null)
                    {
                        _projectOverviewOrgRepository.Delete(orgItem.Id);
                    };
                }

                foreach (var item in input.ProjectOverviewOrgs)
                {
                    // Thêm mới file
                    if (item.Id == 0)
                    {
                        _projectOverviewOrgRepository.Add(new ProjectOverviewOrg
                        {
                            DistributionId = input.Id,
                            Name = item.Name,
                            OrgCode = item.OrgCode,
                            Icon = item.Icon,
                            Url = item.Url,
                            CreatedBy = username
                        }, tradingProviderId);
                    }
                    //Cập nhật file
                    else
                    {
                        _projectOverviewOrgRepository.Update(new ProjectOverviewOrg
                        {
                            Id = item.Id,
                            DistributionId = input.Id,
                            Name = item.Name,
                            OrgCode = item.OrgCode,
                            Icon = item.Icon,
                            Url = item.Url,
                            ModifiedBy = username
                        }, tradingProviderId);
                    }
                }
                #endregion
                scope.Complete();
            }
            _projectOverviewFileRepository.CloseConnection();
        }

        public int IsClose(int distributionId)
        {
            int? tradingProviderId = null;
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            }

            var distributionFind = _distributionRepository.FindById(distributionId, tradingProviderId);
            var isClose = YesNo.YES;
            if (distributionFind.IsClose == YesNo.NO)
            {
                isClose = YesNo.YES;
            }
            else
            {
                isClose = YesNo.NO;
            }

            return _distributionRepository.IsClose(distributionId, isClose, tradingProviderId);
        }

        public int PolicyIsShowApp(int policyId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyFind = _policyRepository.FindPolicyById(policyId, tradingProviderId);
            var isShowApp = YesNo.YES;
            if (policyFind.IsShowApp == YesNo.NO)
            {
                isShowApp = YesNo.YES;
            }
            else
            {
                isShowApp = YesNo.NO;
            }
            return _policyRepository.PolicyIsShowApp(policyId, isShowApp, tradingProviderId);
        }

        public int PolicyDetailIsShowApp(int policyDetailId)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var distributionFind = _policyRepository.FindPolicyDetailById(policyDetailId, tradingProviderId);
            var isShowApp = YesNo.YES;
            if (distributionFind.IsShowApp == YesNo.NO)
            {
                isShowApp = YesNo.YES;
            }
            else
            {
                isShowApp = YesNo.NO;
            }
            return _policyRepository.PolicyDetailIsShowApp(policyDetailId, isShowApp, tradingProviderId);
        }

        public void UpdatePolicy(UpdatePolicyDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            _policyRepository.UpdatePolicy(new Policy
            {
                TradingProviderId = tradingProviderId,
                Id = body.Id,
                Code = body.Code,
                Name = body.Name,
                Type = body.Type,
                IncomeTax = body.IncomeTax,
                TransferTax = body.TransferTax,
                MinMoney = body.MinMoney,
                MaxMoney = body.MaxMoney,
                IsTransfer = body.IsTransfer,
                Classify = body.Classify,
                StartDate = body.StartDate,
                EndDate = body.EndDate,
                Description = body.Description,
                MinWithDraw = body.MinWithDraw,
                CalculateType = body.CalculateType,
                ExitFee = body.ExitFee,
                IsShowApp = body.IsShowApp,
                ExitFeeType = body.ExitFeeType,
                PolicyDisplayOrder = body.PolicyDisplayOrder,
                RenewalsType = body.RenewalsType,
                RemindRenewals = body.RemindRenewals,
                ExpirationRenewals = body.ExpirationRenewals,
                MaxWithDraw = body.MaxWithDraw,
                MinTakeContract = body.MinTakeContract,
                MinInvestDay = body.MinInvestDay,
                ProfitRateDefault = body.CalculateWithdrawType == InvestCalculateWithdrawTypes.KY_HAN_THAP_HON_GAN_NHAT ? 0 : body.ProfitRateDefault,
                CalculateWithdrawType = body.CalculateWithdrawType,
                ModifiedBy = username,
            });

            var policy = _dbContext.InvestPolicies.FirstOrDefault(e => e.Id == body.Id && e.Deleted == YesNo.NO && e.Status == Status.ACTIVE).ThrowIfNull(_dbContext, ErrorCode.InvestPolicyNotFound);
            //back ground chạy show app
            if (policy.EndDate != null)
            {
                if (policy.OffShowAppJobId != null)
                {
                    // Xóa job hiện có của chính sách
                    _backgroundJobs.Delete(policy.OffShowAppJobId);
                }

                if (policy.EndDate >= DateTime.Now)
                {
                    // Sinh job mới
                    string offShowAppJobId = _backgroundJobs.Schedule(() => _investBackgroundJobService.InvestPolicyShowApp(policy.Id, YesNo.NO), policy.EndDate.Value);
                    policy.OffShowAppJobId = offShowAppJobId;
                }
            }

            if (policy.StartDate != null)
            {
                if (policy.ShowAppJobId != null)
                {
                    // Xóa job hiện có của chính sách
                    _backgroundJobs.Delete(policy.ShowAppJobId);
                }

                if (policy.StartDate >= DateTime.Now)
                {
                    // Sinh job mới
                    string showAppJobId = _backgroundJobs.Schedule(() => _investBackgroundJobService.InvestPolicyShowApp(policy.Id, YesNo.YES), policy.StartDate.Value);
                    policy.ShowAppJobId = showAppJobId;
                }
            }
            _dbContext.SaveChanges();
        }

        public void UpdatePolicyDetail(UpdatePolicyDetailDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new PolicyDetail
            {
                TradingProviderId = tradingProviderId,
                Id = body.Id,
                PolicyId = body.PolicyId,
                STT = body.STT,
                ShortName = body.ShortName,
                Name = body.Name,
                PeriodType = body.PeriodType,
                PeriodQuantity = body.PeriodQuantity,
                Profit = body.Profit,
                InterestDays = body.InterestDays,
                InterestType = body.InterestType,
                InterestPeriodType = body.InterestPeriodType,
                InterestPeriodQuantity = body.InterestPeriodQuantity,
                FixedPaymentDate = body.FixedPaymentDate,
                ModifiedBy = username,
            };

            var policy = _policyRepository.FindPolicyById(result.PolicyId, tradingProviderId);
            if (policy != null)
            {
                if (policy.Type == InvPolicyType.FIXED_PAYMENT_DATE)
                {
                    if (result.FixedPaymentDate == null || result.InterestPeriodType == null)
                    {
                        throw new FaultException(new FaultReason($" Kỳ hạn {result.Name} không được bỏ trống số ngày chi trả cố định"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }
                }
                else
                {
                    if (result.InterestType == InterestTypes.DINH_KY)
                    {
                        if (result.InterestPeriodQuantity == null || result.InterestPeriodType == null)
                        {
                            throw new FaultException(new FaultReason($"Kỳ hạn {result.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                        }
                    };
                }
            }

            // Kiểm tra xem đã có chi trả hay chưa
            if (_dbContext.InvestInterestPayments.Any(p => p.Status != InterestPaymentStatus.HUY_DUYET && p.Deleted == YesNo.NO
                        && _dbContext.InvOrders.Any(o => o.Id == p.OrderId && o.PolicyDetailId == body.Id && p.Deleted == YesNo.NO)))
            {
                _investOrderEFRepository.ThrowException(ErrorCode.InvestPolicyDetailExistInterestPayment);
            }
            // Kiểm tra xem có rút vốn hay chưa
            else if (_dbContext.InvestWithdrawals.Any(w => w.Status != WithdrawalStatus.HUY_YEU_CAU && w.Deleted == YesNo.NO
                        && _dbContext.InvOrders.Any(o => o.Id == w.OrderId && o.PolicyDetailId == body.Id && w.Deleted == YesNo.NO)))
            {
                _investOrderEFRepository.ThrowException(ErrorCode.InvestPolicyDetailExistWithdrawal);
            }
            _policyRepository.UpdatePolicyDetail(result);
        }

        public void DeletePolicy(int policyId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _policyRepository.DeletePolicy(policyId, tradingProviderId);
        }

        public void DeletePolicyDetail(int policyDetailId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            _policyRepository.DeletePolicyDetail(policyDetailId, tradingProviderId);
        }

        public int IsShowApp(int id)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var distributionFind = _distributionRepository.FindById(id, tradingProviderId);
            var userName = CommonUtils.GetCurrentUsername(_httpContext);
            var isShowApp = YesNo.YES;
            if (distributionFind.IsShowApp == YesNo.NO)
            {
                isShowApp = YesNo.YES;
                _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate(id, YesNo.NO, YesNo.YES, InvestFieldName.UPDATE_DISTRIBUTION_IS_SHOW_APP, InvestHistoryUpdateTables.INV_DISTRIBUTION, ActionTypes.CAP_NHAT, "bật show app", DateTime.Now), userName);
            }
            else
            {
                isShowApp = YesNo.NO;
                _investHistoryUpdateEFRepository.Add(new InvestHistoryUpdate(id, YesNo.YES, YesNo.NO, InvestFieldName.UPDATE_DISTRIBUTION_IS_SHOW_APP, InvestHistoryUpdateTables.INV_DISTRIBUTION, ActionTypes.CAP_NHAT, "tắt show app", DateTime.Now), userName);
            }
            _dbContext.SaveChanges();
            return _distributionRepository.IsShowApp(id, isShowApp, tradingProviderId);
        }

        public ViewPolicyDto AddPolicy(CreatePolicySpecificDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var policy = _policyRepository.AddPolicy(new Policy
            {
                DistributionId = body.DistributionId,
                TradingProviderId = tradingProviderId,
                Code = body.Code,
                Name = body.Name,
                Type = body.Type,
                IncomeTax = body.IncomeTax,
                TransferTax = body.TransferTax,
                MinMoney = body.MinMoney,
                MaxMoney = body.MaxMoney,
                IsTransfer = body.IsTransfer,
                Classify = body.Classify,
                StartDate = body.StartDate,
                EndDate = body.EndDate,
                Description = body.Description,
                MinWithDraw = body.MinWithDraw,
                CalculateType = body.CalculateType,
                ExitFee = body.ExitFee,
                ExitFeeType = body.ExitFeeType,
                IsShowApp = body.IsShowApp,
                PolicyDisplayOrder = body.PolicyDisplayOrder,
                RenewalsType = body.RenewalsType,
                RemindRenewals = body.RemindRenewals,
                ExpirationRenewals = body.ExpirationRenewals,
                MaxWithDraw = body.MaxWithDraw,
                MinTakeContract = body.MinTakeContract,
                MinInvestDay = body.MinInvestDay,
                ProfitRateDefault = body.CalculateWithdrawType == InvestCalculateWithdrawTypes.KY_HAN_THAP_HON_GAN_NHAT ? 0 : body.ProfitRateDefault,
                CalculateWithdrawType = body.CalculateWithdrawType,
                CreatedBy = username
            });
            var result = _mapper.Map<ViewPolicyDto>(policy);
            var policyDetailTemps = _policyTempRepository.FindBondPolicyDetailTempByPolicyTempId(body.PolicyTempId);
            if (policyDetailTemps != null)
            {
                result.PolicyDetail = new List<ViewPolicyDetailDto>();
                foreach (var policyDetailTemp in policyDetailTemps)
                {
                    var policyDe = new PolicyDetail();
                    policyDe.PolicyId = policy.Id;
                    policyDe.PeriodQuantity = (int)policyDetailTemp.PeriodQuantity;
                    policyDe.DistributionId = policy.DistributionId;
                    policyDe.STT = policyDetailTemp.STT;
                    policyDe.ShortName = policyDetailTemp.ShortName;
                    policyDe.Name = policyDetailTemp.Name;
                    policyDe.PeriodType = policyDetailTemp.PeriodType;
                    policyDe.Status = policyDetailTemp.Status;
                    policyDe.Profit = policyDetailTemp.Profit;
                    policyDe.InterestDays = policyDetailTemp.InterestDays;
                    policyDe.CreatedDate = policyDetailTemp.CreatedDate;
                    policyDe.CreatedBy = policyDetailTemp.CreatedBy;
                    policyDe.ModifiedBy = policyDetailTemp.ModifiedBy;
                    policyDe.ModifiedDate = policyDetailTemp.ModifiedDate;
                    policyDe.Deleted = policyDetailTemp.Deleted;
                    policyDe.TradingProviderId = tradingProviderId;
                    policyDe.InterestType = policyDetailTemp.InterestType;
                    policyDe.InterestPeriodType = policyDetailTemp.InterestPeriodType;
                    policyDe.InterestPeriodQuantity = policyDetailTemp.InterestPeriodQuantity;
                    policyDe.FixedPaymentDate = policyDetailTemp.FixedPaymentDate;
                    var policyDetail = _policyRepository.AddPolicyDetailReturnResult(policyDe);
                    var policyDetailResult = _mapper.Map<ViewPolicyDetailDto>(policyDetail);
                    result.PolicyDetail.Add(policyDetailResult);
                }
            }

            //back ground chạy show app
            if (policy.EndDate != null)
            {
                // Sinh job mới
                string offShowAppJobId = _backgroundJobs.Schedule(() => _investBackgroundJobService.InvestPolicyShowApp(policy.Id, YesNo.NO), policy.EndDate.Value);
                policy.OffShowAppJobId = offShowAppJobId;
            }
            if (policy.StartDate != null)
            {
                // Sinh job mới
                string showAppjobId = _backgroundJobs.Schedule(() => _investBackgroundJobService.InvestPolicyShowApp(policy.Id, YesNo.YES), policy.StartDate.Value);
                policy.ShowAppJobId = showAppjobId;
            }

            return result;
        }

        public void AddPolicyDetail(CreatePolicyDetailDto body)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);

            var result = new PolicyDetail
            {
                TradingProviderId = tradingProviderId,
                PolicyId = body.PolicyId,
                STT = body.STT,
                ShortName = body.ShortName,
                Name = body.Name,
                PeriodType = body.PeriodType,
                PeriodQuantity = body.PeriodQuantity,
                Profit = body.Profit,
                InterestDays = body.InterestDays,
                InterestType = body.InterestType,
                InterestPeriodType = body.InterestPeriodType,
                InterestPeriodQuantity = body.InterestPeriodQuantity,
                FixedPaymentDate = body.FixedPaymentDate,
                CreatedBy = username
            };

            var policy = _policyRepository.FindPolicyById(result.PolicyId, tradingProviderId);
            if (policy != null)
            {
                if (policy.Type == InvPolicyType.FIXED_PAYMENT_DATE)
                {
                    if (result.FixedPaymentDate == null || result.InterestPeriodType == null)
                    {
                        throw new FaultException(new FaultReason($" Kỳ hạn {result.Name} không được bỏ trống số ngày chi trả cố định"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                    }
                }
                else
                {
                    if (result.InterestType == InterestTypes.DINH_KY)
                    {
                        if (result.InterestPeriodQuantity == null || result.InterestPeriodType == null)
                        {
                            throw new FaultException(new FaultReason($"Kỳ hạn {result.Name} không được bỏ trống số kỳ lợi tức"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
                        }
                    };
                }
            }
            _policyRepository.AddPolicyDetail(result);
        }

        public void Request(RequestStatusDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var actionType = InvestApproveAction.THEM;
                var checkIsUpdate = _investApproveRepository.GetOneByActual(input.Id, InvestApproveDataTypes.EP_INV_DISTRIBUTION);
                if (checkIsUpdate != null)
                {
                    actionType = InvestApproveAction.CAP_NHAT;
                }
                _investApproveRepository.CreateApproveRequest(new CreateInvestRequestDto
                {
                    UserRequestId = userId,
                    UserApproveId = input.UserApproveId,
                    RequestNote = input.RequestNote,
                    ActionType = actionType,
                    DataType = InvestApproveDataTypes.EP_INV_DISTRIBUTION,
                    ReferId = input.Id,
                    Summary = input.Summary
                }, tradingProviderId);
                _distributionRepository.DistributionRequest(input.Id);
                transaction.Complete();
            }
            _distributionRepository.CloseConnection();
        }

        /// <summary>
        /// Duyet
        /// </summary>
        /// <param name="input"></param>
        public void Approve(InvestApproveDto input)
        {
            int userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _investApproveRepository.GetOneByActual(input.Id, InvestApproveDataTypes.EP_INV_DISTRIBUTION);

                if (approve != null)
                {
                    _investApproveRepository.ApproveRequestStatus(new InvestApproveDto
                    {
                        Id = approve.Id,
                        ApproveNote = input.ApproveNote,
                        UserApproveId = userId
                    });
                }
                _distributionRepository.DistributionApprove(input.Id);
                transaction.Complete();
            }
            _distributionRepository.CloseConnection();
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
                var approve = _investApproveRepository.GetOneByActual(input.Id, InvestApproveDataTypes.EP_INV_DISTRIBUTION);
                if (approve != null)
                {
                    _investApproveRepository.CheckRequest(new InvestCheckDto
                    {
                        Id = approve.Id,
                        UserCheckId = userid,
                    });
                }
                _distributionRepository.DistributionCheck(input.Id);
                transaction.Complete();
            }
            _distributionRepository.CloseConnection();
        }

        /// <summary>
        /// Huy
        /// </summary>
        public void Cancel(CancelStatusDto input)
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _investApproveRepository.GetOneByActual(input.Id, InvestApproveDataTypes.EP_INV_DISTRIBUTION);
                if (approve != null)
                {
                    _investApproveRepository.CancelRequest(new InvestCancelDto
                    {
                        Id = approve.Id,
                        CancelNote = input.CancelNote,
                    });
                }
                _distributionRepository.DistributionCancel(input.Id);
                transaction.Complete();
            }
            _distributionRepository.CloseConnection();
        }

        public PagingResult<ViewPolicyDto> GetAllPolicyByDistri(InvestPolicyFilterDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = new PagingResult<ViewPolicyDto>();
            var items = new List<ViewPolicyDto>();
            var policyList = _policyRepository.GetAllPolicyPagingResult(tradingProviderId, input);
            foreach (var policyItem in policyList.Items)
            {
                var policy = _mapper.Map<ViewPolicyDto>(policyItem);

                policy.PolicyDetail = new List<ViewPolicyDetailDto>();

                var policyDetailList = _policyRepository.GetAllPolicyDetail(policy.Id, tradingProviderId);
                foreach (var detailItem in policyDetailList)
                {
                    var detail = _mapper.Map<ViewPolicyDetailDto>(detailItem);
                    policy.PolicyDetail.Add(detail);
                }
                items.Add(policy);
            }
            result.Items = items;
            result.TotalItems = policyList.TotalItems;
            return result;
        }
        /// <summary>
        /// Lấy list kỳ hạn theo Id chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public List<PolicyDetail> GetAllListPolicyDetailByPolicy(int policyId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policyDetails = _policyRepository.GetAllPolicyDetail(policyId, tradingProviderId);
            var result = new List<PolicyDetail>();
            foreach (var item in policyDetails)
            {
                result.Add(item);
            }
            return result;
        }

        public PolicyDetail FindPolicyDetailById(int policyDetailId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _policyRepository.FindPolicyDetailById(policyDetailId, tradingProviderId);
        }

        public ViewPolicyDto FindPolicyAndPolicyDetailByPolicyId(int policyId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var policy = _policyRepository.FindPolicyById(policyId, tradingProviderId);
            if (policy == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy chính sách: {policyId}"), new FaultCode(((int)ErrorCode.BadRequest).ToString()), "");
            }
            var result = _mapper.Map<ViewPolicyDto>(policy);
            result.PolicyDetail = new List<ViewPolicyDetailDto>();
            var policyDetailList = _policyRepository.GetAllPolicyDetail(policy.Id, tradingProviderId);
            foreach (var detailItem in policyDetailList)
            {
                var detail = _mapper.Map<ViewPolicyDetailDto>(detailItem);
                result.PolicyDetail.Add(detail);
            }
            var distributionFind = _distributionRepository.FindById(policy.DistributionId, tradingProviderId);
            var distribution = _mapper.Map<DistributionDto>(distributionFind);
            result.Distribution = distribution;
            return result;
        }

        public int ChangeStatusPolicyDetail(int id)
        {
            var policyDetail = FindPolicyDetailById(id);
            var status = Status.ACTIVE;
            if (policyDetail.Status == Status.ACTIVE)
            {
                status = Status.INACTIVE;
            }
            else
            {
                status = Status.ACTIVE;
            }
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            return _policyRepository.UpdateStatusPolicyDetail(id, status, username);
        }
        
        /// <summary>
        /// Thay đổi chính sách phân phối kèm theo trạng thái của mẫu hợp đồng theo chính sách
        /// </summary>
        /// <param name="policyId"></param>
        /// <returns></returns>
        public int ChangeStatusPolicy(int policyId)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var policy = _dbContext.InvestPolicies.FirstOrDefault(p => p.Id == policyId && p.TradingProviderId == tradingProviderId && p.Deleted == YesNo.NO)
                .ThrowIfNull(_dbContext, ErrorCode.InvestPolicyNotFound);
            // Cập nhật trạng thái chính sách
            policy.Status = (policy.Status == Status.INACTIVE) ? Status.ACTIVE : Status.INACTIVE;
            policy.ModifiedBy = username;
            policy.ModifiedDate = DateTime.Now;

            // Cập nhật trạng thái mẫu hợp đồng Active hoặc Deactive theo chính sách
            var contractTempByPolicy = _dbContext.InvestContractTemplates.Where(c => c.PolicyId == policyId && c.Deleted == YesNo.NO);
            foreach (var item in contractTempByPolicy)
            {
                item.Status = policy.Status;
                item.ModifiedBy = username;
                item.ModifiedDate = DateTime.Now;
            }
            _dbContext.SaveChanges();
            return -1;
        }
        public List<ProjectDistributionDto> FindAllProjectDistribution(AppFilterProjectDistribution input)
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

            var tradingProviderIds = _logicInvestorTradingSharedServices.FindListTradingProviderForApp(investorId, input.IsSaleView);

            //Danh sách đại lý bảng hàng của investor
            var findList = _distributionRepository.FindAllProject(input.Keyword, investorId, input.PeriodType);

            //Nếu có danh sách đại lý mà investor đang thuộc thì lọc bảng hàng
            if (tradingProviderIds.Count > 0)
            {
                //Lọc bảng hàng mà investor đã thuộc đại lý trong đấy
                findList = findList.Where(o => tradingProviderIds.Contains(o.TradingProviderId));
            }

            //Sắp xếp theo lợi tức
            if (input.OrderByInterestDesc != null && input.OrderByInterestDesc.Value)
            {
                if (input.OrderByIdDesc != null && input.OrderByIdDesc.Value)
                {
                    findList = findList.OrderByDescending(x => x.CreatedDate.Value.Date).ThenByDescending(s => s.Profit);
                }
                else if (input.OrderByIdDesc != null && !input.OrderByIdDesc.Value)
                {

                    findList = findList.OrderBy(x => x.CreatedDate.Value.Date).ThenByDescending(s => s.Profit);
                }
                else
                {
                    findList = findList.OrderByDescending(s => s.Profit);
                }
            }
            else if (input.OrderByInterestDesc != null && !input.OrderByInterestDesc.Value)
            {

                if (input.OrderByIdDesc != null && input.OrderByIdDesc.Value)
                {
                    findList = findList.OrderByDescending(x => x.DistributionId).ThenBy(s => s.Profit);
                }
                else if (input.OrderByIdDesc != null && !input.OrderByIdDesc.Value)
                {

                    findList = findList.OrderBy(x => x.DistributionId).ThenBy(s => s.Profit);
                }
                else
                {
                    findList = findList.OrderBy(s => s.Profit);
                }
            }
            else if (input.OrderByIdDesc != null && input.OrderByInterestDesc == null)
            {
                if (input.OrderByIdDesc.Value)
                {
                    findList = findList.OrderByDescending(x => x.DistributionId);
                }
                else
                {
                    findList = findList.OrderBy(x => x.DistributionId);
                }
            }

            var result = _mapper.Map<List<ProjectDistributionDto>>(findList);

            //Kỳ hạn tối thiểu, kỳ hạn tối đa
            foreach (var item in result)
            {
                var listDistributionPolicyDetail = new List<PolicyDetail>();
                //Chính sách đang active
                var policyList = _policyRepository.GetAllPolicy(item.DistributionId, item.TradingProviderId, Status.ACTIVE).Where(e => e.IsShowApp == YesNo.YES);
                foreach (var policyItem in policyList)
                {
                    var policy = _mapper.Map<ViewPolicyDto>(policyItem);
                    policy.PolicyDetail = new List<ViewPolicyDetailDto>();
                    //Lấy những kỳ hạn có trạng thái active
                    var policyDetailList = _policyRepository.GetAllPolicyDetail(policy.Id, item.TradingProviderId, Status.ACTIVE).Where(e => e.IsShowApp == YesNo.YES);
                    if (policyDetailList.Count() > 0)
                    {
                        listDistributionPolicyDetail.AddRange(policyDetailList);
                    }
                }

                //Kỳ hạn tối thiểu, kỳ hạn tối đa
                if (listDistributionPolicyDetail.Count() > 0)
                {
                    var policyDetail = listDistributionPolicyDetail.OrderBy(e => e.PeriodType).ThenBy(e => e.PeriodQuantity);
                    item.MinPeriodQuantity = policyDetail.First().PeriodQuantity;
                    item.MinPeriodType = policyDetail.Last().PeriodType;
                    item.MaxPeriodQuantity = policyDetail.Last().PeriodQuantity;
                    item.MaxPeriodType = policyDetail.Last().PeriodType;
                }
            }

            return result;
        }

        // Lấy thông tin dự án từ Id của bán phân phối
        public AppProjectDto FindProjectById(int id)
        {
            var distribution = _distributionRepository.AppFindDistributionById(id, null);
            if (distribution == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán phân phối"), new FaultCode(((int)ErrorCode.InvestDistributionNotFound).ToString()), "");
            }

            var projectFind = _projectRepository.FindByTradingProvider(distribution.ProjectId, null);
            if (projectFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin dự án"), new FaultCode(((int)ErrorCode.InvestProjectNotFound).ToString()), "");
            }
            //Lấy thông tin của dự án
            var result = _mapper.Map<AppProjectDto>(projectFind);
            result.ProjectId = projectFind.Id;
            //Id của bán phân phối
            result.DistributionId = id;
            result.TradingProviderName = distribution.TradingProviderName;
            result.TradingProviderId = distribution.TradingProviderId;
            result.Image = projectFind.Image;

            var rating = from rate in _dbContext.InvestRatings
                         join order in _dbContext.InvOrders on rate.OrderId equals order.Id
                         where order.DistributionId == id && order.Deleted == YesNo.NO
                         select rate;
            var totalRating = rating.Count();
            decimal avarageRating = 0;
            if (totalRating > 0)
            {
                avarageRating = Decimal.Round(rating.Select(o => (decimal)o.Rate).Average(), 1);
            }
            if (projectFind != null)
            {
                // Lấy thông tin chủ đầu tư
                var ownerFind = _ownerRepository.FindById(projectFind.OwnerId ?? 0);
                if (ownerFind != null)
                {
                    var businessCustomerFind = _businessCustomerRepository.FindBusinessCustomerById(ownerFind.BusinessCustomerId);
                    if (businessCustomerFind != null)
                    {
                        result.Owner = new AppOwnerDto()
                        {
                            Name = businessCustomerFind.Name,
                            TaxCode = businessCustomerFind.TaxCode,
                            TradingAddress = businessCustomerFind.TradingAddress,
                            RepName = businessCustomerFind.RepName,
                            Capital = businessCustomerFind.Capital,
                            BusinessRegistrationImg = businessCustomerFind.BusinessRegistrationImg,
                            Id = ownerFind.Id,
                            BusinessTurover = ownerFind.BusinessTurnover,
                            BusinessProfit = ownerFind.BusinessProfit,
                            ROA = ownerFind.Roa,
                            ROE = ownerFind.Roe,
                            Website = ownerFind.Website,
                            Hotline = ownerFind.Hotline,
                            Fanpage = ownerFind.Fanpage,
                        };
                    }
                }

                //lấy danh sách ảnh của dự án
                var listImage = _projectImageRepository.FindByProjectId(projectFind.Id);
                result.ProjectImages = _mapper.Map<List<ProjectImageDto>>(listImage);

                //Lấy loại hình của dự án
                var projectType = _projectRepository.FindByProjectId(projectFind.Id);
                result.ProjectTypes = _mapper.Map<List<ProjectTypeDto>>(projectType);

                //Lấy hồ sơ pháp lý
                var juridicalFiles = _juridicalFileRepository.FindAll(projectFind.Id, -1, 0, null);
                if (juridicalFiles != null)
                {
                    result.JuridicalFiles = new();
                    foreach (var item in juridicalFiles.Items)
                    {
                        result.JuridicalFiles.Add(_mapper.Map<AppJuridicalFileDto>(item));
                    }
                }
            }

            var distributionFind = _distributionRepository.FindById(id, null);
            //Lấy thông tin chính sách
            if (distributionFind != null)
            {
                result.ProjectOverview = new();
                result.ProjectOverview.ContentType = distributionFind.ContentType;
                result.ProjectOverview.OverviewContent = distributionFind.OverviewContent;
                result.ProjectOverview.OverviewImageUrl = distributionFind.OverviewImageUrl;
                // Cộng thêm 1000 tham gia fake
                result.ProjectOverview.TotalParticipants = 1000 + _investOrderEFRepository.EntityNoTracking
                    .Where(o => o.Status == OrderStatus.DANG_DAU_TU)
                    .Select(o => o.CifCode)
                    .Distinct()
                    .Count();
                result.ProjectOverview.RatingRate = avarageRating;
                result.ProjectOverview.TotalReviewers = totalRating;

                var projectOverviewFile = _projectOverviewFileRepository.FindListFile(distributionFind.Id, distributionFind.TradingProviderId);
                result.ProjectOverview.ProjectOverviewFiles = _mapper.Map<List<ProjectOverviewFileDto>>(projectOverviewFile);

                var projectOverviewOrg = _projectOverviewOrgRepository.FindListOrg(distributionFind.Id, distributionFind.TradingProviderId);
                result.ProjectOverview.ProjectOverviewOrgs = _mapper.Map<List<ProjectOverviewOrgDto>>(projectOverviewOrg);


                result.MaxMoney = _orderRepository.MaxTotalInvestment(distributionFind.TradingProviderId, distributionFind.Id);

                var policyFileFind = _distriPolicyFileRepository.FindAllDistriPolicyFile(distributionFind.Id, null, -1, 0, null);
                if (policyFileFind != null)
                {
                    result.PolicyFiles = _mapper.Map<List<AppPolicyFileDto>>(policyFileFind.Items);
                }

                var distributionFile = _distributionFileRepository.FindAll(distributionFind.Id, -1, 0);
                if (distributionFile != null)
                {
                    result.DistributionFiles = _mapper.Map<List<AppDistributionFileDto>>(distributionFile.Items);
                }

                result.ProjectOverview.ProjectInformationShare = new();
                var projectShares = _dbContext.InvestProjectInformationShares.Where(x => x.ProjectId == distribution.ProjectId && x.Status == Status.ACTIVE && x.Deleted == YesNo.NO);
                foreach (var item in projectShares)
                {
                    var shareItem = _mapper.Map<AppInvProjectInformationShareDto>(item);
                    var projectShareDetail = _dbContext.InvestProjectInformationShareDetails.Where(x => x.ProjectShareId == item.Id && x.Deleted == YesNo.NO);
                    shareItem.DocumentFiles = _mapper.Map<List<InvProjectInformationShareDetailDto>>(projectShareDetail.Where(x => x.Type == ProjectInformationShareFileTypes.Document));
                    shareItem.ImageFiles = _mapper.Map<List<InvProjectInformationShareDetailDto>>(projectShareDetail.Where(x => x.Type == ProjectInformationShareFileTypes.Image));
                    result.ProjectOverview.ProjectInformationShare.Add(shareItem);
                }
            }

            return result;
        }

        public List<AppInvestPolicyDto> FindAllListPolicy(int distributionId)
        {
            var distribution = _distributionRepository.FindById(distributionId, null);
            if (distribution == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin bán phân phối"), new FaultCode(((int)ErrorCode.InvestDistributionNotFound).ToString()), "");
            }

            var projectFind = _projectRepository.FindById(distribution.ProjectId, null);
            if (projectFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin dự án"), new FaultCode(((int)ErrorCode.InvestProjectNotFound).ToString()), "");
            }

            var soTienDaDauTu = _orderRepository.SumValue(null, distributionId);
            var investPolicy = _policyRepository.FindAllProjectPolicy(distributionId, distribution.TradingProviderId);
            var result = new List<AppInvestPolicyDto>();

            foreach (var item in investPolicy)
            {
                //Lấy thông tin lợi tức lớn nhất của policy trong
                var policyDetailFind = _policyRepository.FindPolicyDetailMaxProfit(distributionId);
                if (policyDetailFind != null)
                {
                    //item.InterestType = policyDetailFind.InterestType;
                }
                // Hạn mức đầu tư tối đa của phân phối
                item.MaxMoney = _orderRepository.MaxTotalInvestment(distribution.TradingProviderId, distributionId);
                result.Add(item);
            }
            return result;
        }

        public List<AppInvestPolicyDetailDto> FindAllListPolicyDetail(int policyId, decimal? totalValue)
        {
            var policyFind = _policyRepository.FindPolicyById(policyId, null);
            if (policyFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin chính sách"), new FaultCode(((int)ErrorCode.InvestPolicyNotFound).ToString()), "");
            }

            var policyDetailFind = _policyRepository.FindAllProjectPolicyDetail(policyId, null);
            var result = _mapper.Map<List<AppInvestPolicyDetailDto>>(policyDetailFind);

            DateTime ngayBatDauTinhLai = DateTime.Now.Date;
            foreach (var item in result)
            {
                var policyDetail = _policyRepository.FindPolicyDetailById(item.Id);
                if (policyDetail == null)
                {
                    throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
                }
                var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policyDetail.DistributionId && r.Deleted == YesNo.NO);
                var calProfitResult = _investSharedServices.CalculateProfit(policyFind, policyDetail, ngayBatDauTinhLai, totalValue ?? 0, true, distribution?.CloseCellDate);
                item.CalProfit = calProfitResult.ActuallyProfit;
            }
            return result;
        }

        public List<ViewDistributionDto> FindAllOrder(FilterInvestDistributionDto input)
        {
            var result = new List<ViewDistributionDto>();
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

            var distributionList = _distributionRepository.FindAllOrder(tradingProviderId, input.TradingProviderIds);

            foreach (var distributionItem in distributionList) //lặp distribution
            {
                var distribution = _mapper.Map<ViewDistributionDto>(distributionItem);
                distribution.Policies = new List<ViewPolicyDto>();

                var projectFind = _projectRepository.FindById(distributionItem.ProjectId, null);
                if (projectFind != null)
                {
                    distribution.Project = _mapper.Map<ProjectDto>(projectFind);
                    distribution.InvName = projectFind.InvName;
                }

                var generalContractorFind = _generalContractorRepository.FindById(distribution.Project.GeneralContractorId ?? 0);
                if (generalContractorFind != null)
                {
                    distribution.Project.GeneralContractor = _mapper.Map<ViewGeneralContractorDto>(generalContractorFind);
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(distribution.Project.GeneralContractor.BusinessCustomerId);
                    if (businessCustomer != null)
                    {
                        distribution.Project.GeneralContractor.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustomer);
                    }
                }

                var ownerFind = _ownerRepository.FindById(distribution.Project.OwnerId ?? 0);

                if (ownerFind != null)
                {
                    distribution.Project.Owner = _mapper.Map<ViewOwnerDto>(ownerFind);
                    var businessCustom = _businessCustomerRepository.FindBusinessCustomerById(distribution.Project.Owner.BusinessCustomerId);
                    if (businessCustom != null)
                    {
                        distribution.Project.Owner.BusinessCustomer = _mapper.Map<BusinessCustomerDto>(businessCustom);
                    }
                }

                var policyList = _policyRepository.GetAllPolicyOrder(distribution.Id, distribution.TradingProviderId);
                foreach (var policyItem in policyList)
                {
                    var policy = _mapper.Map<ViewPolicyDto>(policyItem);

                    policy.PolicyDetail = new List<ViewPolicyDetailDto>();

                    var policyDetailList = _policyRepository.GetAllPolicyDetail(policy.Id, distribution.TradingProviderId, Status.ACTIVE);
                    foreach (var detailItem in policyDetailList)
                    {
                        var detail = _mapper.Map<ViewPolicyDetailDto>(detailItem);
                        policy.PolicyDetail.Add(detail);
                    }
                    distribution.Policies.Add(policy);
                }
                distribution.HanMucToiDa = _orderRepository.MaxTotalInvestment(tradingProviderId, distributionItem.Id);
                result.Add(distribution);
            }
            return result;
        }

        public List<BusinessCustomerBankDto> FindBankByTradingInvest(int? distributionId = null, int? type = null)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            return _tradingProviderRepository.FindBankByTrading(tradingProviderId, distributionId, type);
        }

        public List<BusinessCustomerBankDto> FindBankByDistributionId(int distributionId, int type)
        {
            var listTradingBankAcc = _distributionRepository.FindBankByDistributionId(distributionId, type);
            var result = new List<BusinessCustomerBankDto>();
            foreach (var bank in listTradingBankAcc)
            {
                var businessCustomerBanks = _businessCustomerEFRepository.FindBankById(bank.TradingBankAccId);
                if (businessCustomerBanks != null)
                {
                    result.Add(businessCustomerBanks);
                }
            }
            return result;
        }

        public IEnumerable<ViewDistributionByTradingDto> FindDistributionBanHoByTrading()
        {
            var userType = CommonUtils.GetCurrentUserType(_httpContext);
            var (tradingProviderId, tradingBanHo) = userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER
             ? (CommonUtils.GetCurrentTradingProviderId(_httpContext), _saleEFRepository.FindTradingProviderBanHo(CommonUtils.GetCurrentTradingProviderId(_httpContext)))
             : (0, new List<int>());

            var result = _dbContext.InvestDistributions
                .Include(e => e.Project)
                .ThenInclude(e => e.ProjectTradingProviders)
                .Where(e => (userType == UserTypes.ROOT_EPIC || (tradingBanHo.Contains(e.TradingProviderId) || e.TradingProviderId == tradingProviderId)) &&
                              e.IsShowApp == YesNo.YES &&
                              e.Status == DistributionStatus.HOAT_DONG &&
                              e.Deleted == YesNo.NO &&
                              e.Project.Deleted == YesNo.NO &&
                              e.IsClose == YesNo.NO)
                .Select(e => new ViewDistributionByTradingDto
                {
                    Id = e.Id,
                    Code = e.Project.InvCode,
                    Name = e.Project.InvName,
                    IsSalePartnership = tradingBanHo.Contains(e.TradingProviderId)
                })
                .AsEnumerable();

            return result;
        }
    }
}