using AutoMapper;
using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.DataEntities;
using EPIC.BondEntities.Dto.AppOrder;
using EPIC.BondEntities.Dto.InterestPayment;
using EPIC.BondRepositories;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Models;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.InvestRepositories;
using EPIC.Notification.Services;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Shared;
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
    public class BondInterestPaymentService : IBondInterestPaymentService
    {
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly string _connectionString;
        private readonly BondInterestPaymentRepository _interestPaymentRepository;
        private readonly BondOrderRepository _orderRepository;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IBondSharedService _bondSharedService;
        private readonly NotificationServices _sendEmailServices;
        private readonly IMapper _mapper;
        private readonly BondSecondaryRepository _bondSecondaryRepository;
        private readonly CifCodeRepository _cifCodeRepository;
        private readonly InvestorIdentificationRepository _investorIdentificationRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;

        public BondInterestPaymentService(ILogger<BondInterestPaymentService> logger,
            IConfiguration configuration,
            DatabaseOptions databaseOptions,
            IHttpContextAccessor httpContext,
            IBondSharedService bondSharedService,
            NotificationServices sendEmailServices,
            IMapper mapper)
        {
            _logger = logger;
            _configuration = configuration;
            _connectionString = databaseOptions.ConnectionString;
            _interestPaymentRepository = new BondInterestPaymentRepository(_connectionString, _logger);
            _orderRepository = new BondOrderRepository(_connectionString, _logger);
            _bondSecondaryRepository = new BondSecondaryRepository(_connectionString, _logger);
            _cifCodeRepository = new CifCodeRepository(_connectionString, _logger);
            _investorIdentificationRepository = new InvestorIdentificationRepository(_connectionString, _logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, _logger);
            _httpContext = httpContext;
            _bondSharedService = bondSharedService;
            _sendEmailServices = sendEmailServices;
            _mapper = mapper;
        }
        public void InterestPaymentAdd(InterestPaymentCreateListDto input)
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            foreach (var item in input.InterestPayments)
            {
                _interestPaymentRepository.InterestPaymentAdd(new BondInterestPayment
                {
                    TradingProviderId = tradingProviderId,
                    OrderId = item.OrderId,
                    PeriodIndex = item.PeriodIndex,
                    CifCode = item.CifCode,
                    AmountMoney = item.AmountMoney,
                    PolicyDetailId = item.PolicyDetailId,
                    PayDate = item.PayDate,
                    IsLastPeriod = item.IsLastPeriod,
                    ModifiedBy = username
                });
            }
        }

        /// <summary>
        /// Đổi trạng thái chi trả, nếu hợp động chọn tái tục vốn thì xử lý tái tục vốn
        /// </summary>
        /// <param name="input"></param>
        /// <exception cref="FaultException"></exception>
        public async Task ChangeEstablishedWithOutPayingToPaidStatus(ChangeStatusDto input)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var username = CommonUtils.GetCurrentUsername(_httpContext);
            var approveIp = CommonUtils.GetCurrentRemoteIpAddress(_httpContext);

            //Nếu tái tục sau chi trả thì sẽ dùng để đầu tư tiếp
            var ngayDauTuSauTaiTuc = DateTime.Now.Date;

            using (var transaction = new TransactionScope())
            {
                foreach (var id in input.Ids)
                {
                    var interestPaymentFind = _interestPaymentRepository.FindById(id, tradingProviderId);
                    var orderFind = _orderRepository.FindById(interestPaymentFind.OrderId, tradingProviderId);

                    //Nếu sổ lệnh đang active
                    if (orderFind != null || orderFind.InvestDate != null || orderFind.Status == OrderStatus.DANG_DAU_TU)
                    {
                        //Tìm kiếm kỳ hạn để tìm ngày đáo hạn của order
                        var policyDetailFind = _bondSecondaryRepository.FindPolicyDetailById(orderFind.PolicyDetailId);
                        if (policyDetailFind == null)
                        {
                            throw new FaultException(new FaultReason($"Không tìm thấy thông tin kỳ hạn"), new FaultCode(((int)ErrorCode.BondPolicyDetailNotFound).ToString()), "");
                        }
                        DateTime ngayDauTu = orderFind.InvestDate.Value.Date;

                        //Tính ngày đáo hạn
                        ngayDauTuSauTaiTuc = _bondSharedService.CalculateDueDate(policyDetailFind, ngayDauTu);
                    }
                    var result = _interestPaymentRepository.ChangeEstablishedWithOutPayingToPaidStatus(id, tradingProviderId, ngayDauTuSauTaiTuc, approveIp, username);
                    if (result.IsLastPeriod == YesNo.YES)
                    {
                        await _sendEmailServices.SendEmailBondRenewalsSuccess(result.Id, result.OrderId);
                    }    
                }
                transaction.Complete();
            }
            _interestPaymentRepository.CloseConnection();
        }

        public PagingResult<DanhSachChiTraDto> FindAll(int pageSize, int pageNumber, string keyword, int? status, string phone, string contractCode)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var interestPaymentList = _interestPaymentRepository.FindAll(pageSize, pageNumber, keyword, status, phone, contractCode, tradingProviderId);
            var result = new PagingResult<DanhSachChiTraDto>();
            var items = new List<DanhSachChiTraDto>();

            foreach (var item in interestPaymentList.Items)
            {
                var paymentItem = _mapper.Map<DanhSachChiTraDto>(item);

                var orderFind = _orderRepository.FindById(item.OrderId, tradingProviderId);

                //Lấy thông tin kỳ hạn
                var policyDetailFind = _bondSecondaryRepository.FindPolicyDetailById(item.PolicyDetailId);

                //Lấy thông tin chính sách
                var policyFind = _bondSecondaryRepository.FindPolicyById(policyDetailFind.PolicyId, policyDetailFind.TradingProviderId);
                paymentItem.ContractCode = orderFind.ContractCode;
                paymentItem.BondOrder = orderFind;
                paymentItem.PolicyDetail = policyDetailFind;

                var cifCodeFind = _cifCodeRepository.GetByCifCode(orderFind.CifCode);
                if (cifCodeFind != null && cifCodeFind.InvestorId != null)
                {
                    var investerIden = _investorIdentificationRepository.FindById(orderFind.InvestorIdenId ?? 0);
                    if (investerIden != null)
                        paymentItem.Name = investerIden.Fullname;
                }
                else if (cifCodeFind != null && cifCodeFind.BusinessCustomerId != null)
                {
                    var businessCustomer = _businessCustomerRepository.FindBusinessCustomerById(cifCodeFind.BusinessCustomerId ?? 0);
                    if (businessCustomer != null)
                    {
                        paymentItem.Name = businessCustomer.Name;
                    }
                }
                items.Add(paymentItem);
            }
            result.Items = items;
            return result;
        }

        public BondInterestPayment FindById(int id)
        {
            int tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
            var result = _interestPaymentRepository.FindById(id, tradingProviderId);
            if (result == null)
            {
                throw new FaultException(new FaultReason($"Không tìm thấy danh sách chi trả"), new FaultCode(((int)ErrorCode.NotFound).ToString()), "");
            }
            return result;
        }
    }
}
