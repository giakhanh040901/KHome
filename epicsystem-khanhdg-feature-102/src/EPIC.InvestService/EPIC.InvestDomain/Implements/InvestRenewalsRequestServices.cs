using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ContractData;
using EPIC.InvestDomain.Interfaces;
using EPIC.InvestEntities.DataEntities;
using EPIC.InvestEntities.Dto.InvestApprove;
using EPIC.InvestEntities.Dto.RenewalsRequest;
using EPIC.InvestRepositories;
using EPIC.InvestSharedDomain.Interfaces;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Contract;
using EPIC.Utils.ConstantVariables.Invest;
using EPIC.Utils.ConstantVariables.Shared;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading.Tasks;
using System.Transactions;
using static EPIC.Utils.DataUtils.ContractDataUtils;

namespace EPIC.InvestDomain.Implements
{
    public class InvestRenewalsRequestServices : IInvestRenewalsRequestServices
    {
        private readonly EpicSchemaDbContext _dbContext;
        private readonly ILogger<InvestRenewalsRequestServices> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly InvRenewalsRequestRepository _renewalsRequestRepository;
        private readonly InvestApproveRepository _approveRepository;
        private readonly InvestPolicyRepository _policyRepository;
        private readonly DistributionRepository _distributionRepository;
        private readonly InvestOrderRepository _investOrderRepository;
        private readonly ProjectRepository _projectRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvestPolicyRepository _investPolicyRepository;
        private readonly OrderContractFileRepository _orderContractFileRepository;
        private readonly InvestNotificationServices _investNotificationServices;
        private readonly IContractTemplateServices _contractTemplateServices;
        private readonly IContractDataServices _contractDataServices;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;
        private readonly IInvestContractCodeServices _investContractCodeServices;
        private readonly IInvestSharedServices _investSharedServices;
        private readonly InvestOrderEFRepository _investOrderEFRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly InvestRenewalsRequestEFRepository _investRenewalsRequestEFRepository;
        private readonly InvestPolicyDetailEFRepository _investPolicyDetailEFRepository;

        public InvestRenewalsRequestServices(ILogger<InvestRenewalsRequestServices> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IContractTemplateServices contractTemplateServices,
            IContractDataServices contractDataServices,
            IInvestContractCodeServices investContractCodeServices,
            InvestNotificationServices investNotificationServices,
            IInvestSharedServices investSharedServices,
            IMapper mapper,
            EpicSchemaDbContext dbContext)
        {
            _dbContext = dbContext;
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _renewalsRequestRepository = new InvRenewalsRequestRepository(_connectionString, _logger);
            _approveRepository = new InvestApproveRepository(_connectionString, _logger);
            _policyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _distributionRepository = new DistributionRepository(_connectionString, _logger);
            _investOrderRepository = new InvestOrderRepository(_connectionString, _logger);
            _projectRepository = new ProjectRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investPolicyRepository = new InvestPolicyRepository(_connectionString, _logger);
            _orderContractFileRepository = new OrderContractFileRepository(_connectionString, _logger);
            _investNotificationServices = investNotificationServices;
            _httpContext = httpContext;
            _contractTemplateServices = contractTemplateServices;
            _contractDataServices = contractDataServices;
            _mapper = mapper;
            _investContractCodeServices = investContractCodeServices;
            _investSharedServices = investSharedServices;
            _investOrderEFRepository = new InvestOrderEFRepository(_dbContext, _logger);
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _investRenewalsRequestEFRepository = new InvestRenewalsRequestEFRepository(_dbContext, _logger);
            _investPolicyDetailEFRepository = new InvestPolicyDetailEFRepository(_dbContext, _logger);
        }

        public PagingResult<ViewInvRenewalsRequestDto> Find(FilterInvRenewalsRequestDto dto)
        {
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            int? partnerId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            dto.DataType = InvestApproveDataTypes.EP_INV_RENEWALS_REQUEST;

            var query = _approveRepository.GetAll(dto, tradingProviderId, partnerId);

            var result = new PagingResult<ViewInvRenewalsRequestDto>
            {
                TotalItems = query.TotalItems,
            };

            if (result.TotalItems > 0)
            {
                var renewalsRequestItem = new ViewInvRenewalsRequestDto();
                var itemList = new List<ViewInvRenewalsRequestDto>();
                foreach (var item in result.Items)
                {
                    var renewalsRequestFind = _renewalsRequestRepository.GetById(item.ReferId ?? 0);
                    if (renewalsRequestFind != null)
                    {
                        renewalsRequestItem.SettlementMethod = renewalsRequestFind.SettlementMethod;
                        var policyDetailFind = _policyRepository.FindPolicyDetailById(renewalsRequestFind.RenewalsPolicyDetailId, tradingProviderId);
                        if (policyDetailFind != null)
                        {
                            var policyFind = _policyRepository.FindPolicyById(policyDetailFind.PolicyId, tradingProviderId);
                            if (policyFind != null)
                            {
                                var distributionFind = _distributionRepository.FindById(policyFind.DistributionId, tradingProviderId);
                                if (distributionFind != null)
                                {
                                    var projectFind = _projectRepository.FindById(distributionFind.ProjectId);
                                    if (projectFind != null)
                                    {
                                        renewalsRequestItem.Policy = policyFind;
                                        renewalsRequestItem.PolicyDetail = policyDetailFind;
                                        renewalsRequestItem.Project = projectFind;
                                    }
                                }
                            }
                        }
                    }
                    itemList.Add(renewalsRequestItem);
                }
                if (dto.SettlementMethod != null)
                {
                    itemList = itemList.Where(r => r.SettlementMethod != null && r.SettlementMethod == dto.SettlementMethod).ToList();
                }
                result.Items = itemList;
                result.TotalItems = itemList.Count();
            }
            return result;
        }

        public async Task<int> CreateRenewalsRequestCms(CreateRenewalsRequestDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var result = _renewalsRequestRepository.CreateRenewalsRequestCms(input, tradingProviderId, userId);
            await _investNotificationServices.SendEmailInvestRenewalRequest(result.Id);
            return result.Id;
        }

        public async Task<InvRenewalsRequest> AppRenewalsRequest(AppCreateRenewalsRequestDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var orderFind = _investOrderRepository.FindById(input.OrderId);
            if (orderFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin Hợp đồng sổ lệnh"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }
            var investorIdentificationFind = _investorIdentificationRepository.GetByInvestorId(investorId);
            if (investorIdentificationFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin giấy tờ của nhà đầu tư"), new FaultCode(((int)ErrorCode.InvestorIdentificationNotFound).ToString()), "");
            }

            var investorFind = _investorRepository.FindById(investorId);
            if (investorFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin nhà đầu tư"), new FaultCode(((int)ErrorCode.InvestorNotFound).ToString()), "");
            }

            var summary = $"{orderFind.ContractCode} - {investorIdentificationFind.Fullname} - Phương thức tất toán {input.SettlementMethod} - Kỳ hạn mới: {input.RenewarsPolicyDetailId} ";
            var requestNote = $"Yêu cầu thay đổi phương thức tất toán cuối kỳ cho Hợp đồng : {orderFind.ContractCode}";
            var result = _renewalsRequestRepository.AppRenewalsRequest(input, investorId, userId, requestNote, summary);
            await _investNotificationServices.SendEmailInvestRenewalRequest(result.Id);
            return result;
        }

        public void ApproveRequest(InvestApproveDto input)
        {
            int userId = CommonUtils.GetCurrentUserId(_httpContext); 
            var renewalsRequest = _renewalsRequestRepository.GetById(input.Id, false);
            if (renewalsRequest == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy yêu cầu tái tục"), new FaultCode(((int)ErrorCode.InvestRenewalsNotFound).ToString()), "");
            }
            var orderFind = _investOrderRepository.FindById(renewalsRequest.OrderId, null, null, false);
            if (orderFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin lệnh"), new FaultCode(((int)ErrorCode.InvestOrderNotFound).ToString()), "");
            }
            var renewalsTimes =  _renewalsRequestRepository.GetListByOrderId(renewalsRequest.OrderId).Where(r => r.Status == InvestRenewalsRequestStatus.DA_DUYET).Count();
            var policyDetailFind = _investPolicyRepository.FindPolicyDetailById(orderFind.PolicyDetailId, null, false);
            if (policyDetailFind == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.InvestPolicyDetailNotFound).ToString()), "");
            }
            var transaction = _renewalsRequestRepository.BeginTransaction();
            var approve = _approveRepository.GetOneByActual(input.Id, InvestApproveDataTypes.EP_INV_RENEWALS_REQUEST, false);
            if (approve != null)
            {
                _approveRepository.ApproveRequestStatus(new InvestApproveDto
                {
                    Id = approve.Id,
                    ApproveNote = input.ApproveNote,
                    UserApproveId = userId
                }, false);
            }
            _renewalsRequestRepository.ApproveRequest(input.Id, false);      
            transaction.Commit();
           
        }

        public void CancelRequest(InvestCancelDto input)
        {
            _renewalsRequestRepository.CancelRequest(input.Id, input.CancelNote);
        }

        /// <summary>
        /// Check ngày yêu cầu tái tục, nếu ngày hết hạn gửi yêu cầu trước ngày hiện tại -> hợp đồng hết hạn yêu cầu tái tục
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        /// <exception cref="FaultException"></exception>
        public bool AppRenewalsRequestNotification(long orderId)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var orderFind = _investOrderEFRepository.FindById(orderId).ThrowIfNull<InvOrder>(_dbContext, ErrorCode.InvestOrderNotFound);
            var investorIdentificationFind = _investorIdentificationRepository.GetByInvestorId(investorId);
            if (investorIdentificationFind == null)
            {
                _investOrderEFRepository.ThrowException(ErrorCode.InvestorIdentificationNotFound);
            }

            var investorFind = _investorEFRepository.FindById(investorId).ThrowIfNull(_dbContext, ErrorCode.InvestorNotFound);

            if (orderFind.Status == OrderStatus.DANG_DAU_TU)
            {
                var policyDetail = _policyRepository.FindPolicyDetailById(orderFind.PolicyDetailId);
                if (policyDetail != null)
                {
                    var distribution = _dbContext.InvestDistributions.FirstOrDefault(r => r.Id == policyDetail.DistributionId && r.Deleted == YesNo.NO)
                                        .ThrowIfNull(_dbContext, ErrorCode.InvestDistributionNotFound);
                    DateTime ngayDaoHan = orderFind.DueDate ?? _investSharedServices.CalculateDueDate(policyDetail, orderFind.InvestDate.Value, distribution.CloseCellDate);
                    var policy = _policyRepository.FindPolicyById(orderFind.PolicyId);
                    if (policy != null)
                    {
                        var ngayHetHanGuiYeuCau = ngayDaoHan.AddDays(-policy.ExpirationRenewals);
                        var ngayHienTai = DateTime.Now;
                        if (ngayHienTai.Date > ngayHetHanGuiYeuCau.Date)
                        {
                            _investOrderEFRepository.ThrowException(ErrorCode.InvestRenewalRequestExpired);
                        }
                    }
                }
            }
            return true;
        }

        public InvestRenewalsRequestInfoDto AppRenewalsRequestInfo(int orderId)
        {
            _logger.LogInformation($"{nameof(AppRenewalsRequestInfo)}: orderId = {orderId}");
            var result = new InvestRenewalsRequestInfoDto();
            var renewalRequestFind = _investRenewalsRequestEFRepository.EntityNoTracking.FirstOrDefault(o => o.OrderId == orderId);
            if (renewalRequestFind != null)
            {
                var policyFind = _investPolicyDetailEFRepository.EntityNoTracking.Where(o => o.Id == renewalRequestFind.RenewalsPolicyDetailId).OrderByDescending(o => o.Id).FirstOrDefault();
                result = new InvestRenewalsRequestInfoDto()
                {
                    PolicyDetailId = renewalRequestFind.RenewalsPolicyDetailId,
                    PolicyId = policyFind.PolicyId,
                    IsRenewal = YesNo.YES
                };
                
            }
            else
            {
                var orderFind = _investOrderEFRepository.EntityNoTracking.Where(o => o.Id == orderId).OrderByDescending(o => o.Id).FirstOrDefault();
                if (orderFind != null)
                {
                    result = new InvestRenewalsRequestInfoDto()
                    {
                        PolicyDetailId = orderFind.PolicyDetailId,
                        PolicyId = orderFind.PolicyId,
                        IsRenewal = YesNo.NO
                    };
                }
            }
            
            return result;
        }
    }
}
