using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.RenewalsRequest;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.Dto.CoreApprove;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
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
    public class BondRenewalsRequestService : IBondRenewalsRequestService
    {
        private readonly ILogger<BondRenewalsRequestService> _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly BondRenewalsRequestRepository _renewalsRequestRepository;
        private readonly ApproveRepository _approveRepository;
        private readonly BondSecondaryRepository _bondSecondaryRepository;
        private readonly BondInfoRepository _bondInfoRepository;
        private readonly BondOrderRepository _bondOrderRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IMapper _mapper;

        public BondRenewalsRequestService(ILogger<BondRenewalsRequestService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _renewalsRequestRepository = new BondRenewalsRequestRepository(_connectionString, _logger);
            _approveRepository = new ApproveRepository(_connectionString, _logger);
            _bondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _bondInfoRepository = new BondInfoRepository(_connectionString, _logger);
            _bondOrderRepository = new BondOrderRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _mapper = mapper;
        }

        public PagingResult<ViewRenewalsRequestDto> Find(FilterRenewalsRequestDto dto)
        {
            int? tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            int? partnerId = null;
            string userType = CommonUtils.GetCurrentUserType(_httpContext);
            dto.DataType = CoreApproveDataTypes.EP_RENEWALS_REQUEST;

            var query = _approveRepository.GetAll(dto, tradingProviderId, partnerId, userType);

            var result = new PagingResult<ViewRenewalsRequestDto>
            {
                TotalItems = query.TotalItems,
            };

            if (result.TotalItems > 0)
            {
                var itemRequest = new ViewRenewalsRequestDto();
                var totalItemRequests = new List<ViewRenewalsRequestDto>();

                foreach (var item in query.Items)
                {
                    var renewalsRequest = _renewalsRequestRepository.GetById(item.ReferId ?? 0);
                    itemRequest = _mapper.Map<ViewRenewalsRequestDto>(item);
                    if (renewalsRequest != null)
                    {
                        itemRequest.SettlementMethod = renewalsRequest.SettlementMethod;

                        var policyDetailFind = _bondSecondaryRepository.FindPolicyDetailById(renewalsRequest.RenewarsPolicyDetailId, tradingProviderId ?? 0);
                        if (policyDetailFind != null)
                        {
                            var policyFind = _bondSecondaryRepository.FindPolicyById(policyDetailFind.PolicyId, tradingProviderId);
                            if (policyFind != null)
                            {
                                var secondaryFind = _bondSecondaryRepository.FindSecondaryById(policyDetailFind.SecondaryId, tradingProviderId);
                                if (secondaryFind != null)
                                {
                                    var bondInfoFind = _bondInfoRepository.FindById(secondaryFind.BondId);
                                    if (bondInfoFind != null)
                                    {
                                        itemRequest.Policy = policyFind;
                                        itemRequest.PolicyDetail = policyDetailFind;
                                        itemRequest.ProductBondInfo = bondInfoFind;
                                    }
                                }
                            }
                        }
                        totalItemRequests.Add(itemRequest);
                    }
                }

                if (dto.SettlementMethod != null)
                {
                    totalItemRequests = totalItemRequests.Where(r => r.SettlementMethod != null && r.SettlementMethod == dto.SettlementMethod).ToList();
                }

                result.Items = totalItemRequests;
                result.TotalItems = totalItemRequests.Count();
            }
            return result;
        }

        public BondRenewalsRequest AddRequest(CreateRenewalsRequestDto input)
        {
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);

            return _renewalsRequestRepository.AddRequest(input, tradingProviderId, userId);
        }

        public BondRenewalsRequest AppRenewalsRequest(AppCreateRenewalsRequestDto input)
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var orderFind = _bondOrderRepository.FindById(input.OrderId);
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

            return _renewalsRequestRepository.AppRenewalsRequest(input, investorId, userId, requestNote, summary);
        }

        public void ApproveRequest(ApproveStatusDto input)
        {
            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            using (var transaction = new TransactionScope())
            {
                var approve = _approveRepository.GetOneByActual(input.Id, CoreApproveDataType.INV_RENEWALS_REQUEST);

                if (approve != null)
                {
                    _approveRepository.ApproveRequestStatus(new ApproveRequestDto
                    {
                        ApproveID = approve.ApproveID,
                        ApproveNote = input.ApproveNote,
                        UserApproveId = userId
                    });
                }
                _renewalsRequestRepository.ApproveRequest(input.Id);
                transaction.Complete();
            }
            _renewalsRequestRepository.CloseConnection();
        }

        public void CancelRequest(CancelStatusDto input)
        {
            _renewalsRequestRepository.CancelRequest(input.Id, input.CancelNote);
        }
    }
}
