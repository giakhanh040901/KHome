using AutoMapper;
using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.User;
using EPIC.EventRepositories;
using EPIC.IdentityRepositories;
using EPIC.InvestEntities.DataEntities;
using EPIC.Notification.Dto.CoreNotification;
using EPIC.Notification.Dto.DataEmail;
using EPIC.Notification.Dto.EventNotification;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Event;
using EPIC.Utils.ConstantVariables.Notification;
using EPIC.Utils.ConstantVariables.Shared;
using EPIC.Utils.DataUtils;
using EPIC.Utils.SharedApiService;
using EPIC.Utils.SharedApiService.Dto.SharedEmailApiDto;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MySqlX.XDevAPI.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static IdentityModel.OidcConstants;

namespace EPIC.Notification.Services
{
    /// <summary>
    /// Thông báo Event
    /// </summary>
    public class EventNotificationServices
    {
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly IWebHostEnvironment _env;
        //Common
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly SharedNotificationApiUtils _sharedEmailApiUtils;
        private readonly IConfiguration _configuration;
        private readonly string _baseUrl;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly UsersEFRepository _usersEFRepository;
        private readonly TradingProviderEFRepository _tradingProviderEFRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;

        public EventNotificationServices(EpicSchemaDbContext dbContext,
            ILogger<EventNotificationServices> logger,
            IMapper mapper,
            IWebHostEnvironment env,
            IHttpContextAccessor httpContextAccessor,
            SharedNotificationApiUtils sharedEmailApiUtils,
            IConfiguration configuration)
        {
            _dbContext = dbContext;
            _logger = logger;
            _mapper = mapper;
            _env = env;
            //Common
            _httpContextAccessor = httpContextAccessor;
            _sharedEmailApiUtils = sharedEmailApiUtils;
            _configuration = configuration;
            _baseUrl = _configuration["SharedApi:BaseUrl"];
            _investorEFRepository = new InvestorEFRepository(_dbContext, _logger);
            _usersEFRepository = new UsersEFRepository(_dbContext, _logger);
            _tradingProviderEFRepository = new TradingProviderEFRepository(_dbContext, _logger);
            _defErrorEFRepository = new DefErrorEFRepository(_dbContext);
        }

        /// <summary>
        /// Lấy dữ liệu cho gửi mail
        /// </summary>
        /// <param name="investorId"></param>
        /// <returns></returns>
        private DataEmail GetData(int investorId, int identificationId, int tradingProviderId)
        {
            var result = new DataEmail();
            result.Investor = _investorEFRepository.FindById(investorId);
            if (result.Investor == null)
            {
                _logger.LogError($"{nameof(GetData)}: Không tìm thấy khách hàng khi gửi email/ sms/ push app. investorId: {investorId}");
                return new();
            }
            result.InvestorIdentification = _investorEFRepository.GetIdentificationById(identificationId);
            if (result.InvestorIdentification == null)
            {
                _logger.LogError($"{nameof(GetData)}: Không tìm thấy thông tin giấy tờ khách hàng khi gửi email/ sms/ push app. investorIdentificationId: {identificationId}");
                return new();
            }

            var user = _usersEFRepository.FindByInvestorId(result.Investor.InvestorId);
            if (user != null)
            {
                result.Users = _mapper.Map<UserDto>(user);
                result.FcmTokens = _usersEFRepository.GetFcmTokenByUserId(result.Users.UserId);
            }
            result.OtherParams = new ParamsChooseTemplate
            {
                TradingProviderId = tradingProviderId
            };
            return result;
        }

        /// <summary>
        /// Gửi thông báo đăng ký vé thành công
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task SendNotifyRegisterTicket(int orderId)
        {
            _logger.LogInformation($"{nameof(SendNotifyPauseEvent)}: orderId = {orderId}");
            var order = _dbContext.EvtOrders.FirstOrDefault(e => e.Id == orderId && e.Deleted == YesNo.NO);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy sổ lệnh orderId: {orderId}");
                return;
            }
            if (order.Status != EvtOrderStatus.HOP_LE)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Sổ lệnh đang không ở trạng thái hợp lệ: {orderId}");
                return;
            }
            var ticketQuantity = 0;
            var orderDetails = _dbContext.EvtOrderDetails.Where(od => od.OrderId == order.Id);
            if (orderDetails.Any())
            {
                ticketQuantity = orderDetails.Sum(od => od.Quantity);
            }

            var eventDetail = _dbContext.EvtEventDetails.FirstOrDefault(ed => ed.Id == order.EventDetailId && ed.Deleted == YesNo.NO);
            if (eventDetail == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy khung giờ sự kiện: {order.EventDetailId}");
                return;
            }
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == eventDetail.EventId && e.Deleted == YesNo.NO);
            if (eventFind == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy sự kiện: {order.EventDetailId}");
                return;
            }
            DataEmail data = GetData(order.InvestorId, order.InvestorIdenId, order.TradingProviderId);
            var content = new EventRegisterTicketSuccessContent
            {
                CustomerName = data?.InvestorIdentification?.Fullname,
                TicketQuantity = ticketQuantity.ToString(),
                EventName = eventFind.Name,
                StartDate = eventDetail.StartDate?.ToString("dd/MM/yyyy HH:mm"),
                EndDate = eventDetail.EndDate?.ToString("dd/MM/yyyy HH:mm")
            };

            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.TB_DANG_KY_THAM_GIA_SU_KIEN
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.EVENT_DANG_KY_VE_THANH_CONG, receiver, data.OtherParams);
        }

        /// <summary>
        /// Gửi thông báo tạm dừng sự kiện
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task SendNotifyPauseEvent(int eventId) 
        {
            _logger.LogInformation($"{nameof(SendNotifyPauseEvent)}: eventId = {eventId}");
            var eventFind = _dbContext.EvtEvents.Include(e => e.EventDetails).FirstOrDefault(e => e.Id == eventId && e.Deleted == YesNo.NO);
            if (eventFind == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy sự kiện: {eventId}");
                return;
            }

            if (eventFind.Status != EventStatus.TAM_DUNG)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Sự kiện {eventId} chưa được tạm dừng");
                return;
            }
            var investors = _dbContext.EvtOrders.Include(o => o.EventDetail)
                                             .ThenInclude(ed => ed.Event)
                                             .Where(o => o.Deleted == YesNo.NO
                                             && o.EventDetail.Event.Id == eventId)
                                             .Select(e => e.InvestorId).Distinct();

            foreach (var investor in investors)
            {
                var investorIdentification = _dbContext.InvestorIdentifications.Where(i => i.InvestorId == investor && i.Deleted == YesNo.NO)
                        .OrderByDescending(c => c.IsDefault).ThenByDescending(c => c.Id).FirstOrDefault().ThrowIfNull(_dbContext, ErrorCode.InvestorIdentificationNotFound);
                DataEmail data = GetData(investor, investorIdentification.Id, eventFind.TradingProviderId);
                var content = new EventRegisterTicketSuccessContent
                {
                    CustomerName = data?.InvestorIdentification?.Fullname,
                    EventName = eventFind.Name,
                    StartDate = eventFind.EventDetails.Min(e => e.StartDate)?.ToString("dd/MM/yyyy HH:mm"),
                    EndDate = eventFind.EventDetails.Max(e => e.EndDate)?.ToString("dd/MM/yyyy HH:mm")
                };

                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.TB_TAM_DUNG_TO_CHUC_SU_KIEN
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.EVENT_TAM_DUNG_SU_KIEN, receiver, data.OtherParams);
            }
        }

        /// <summary>
        /// Thông báo duyệt thanh toán thành công
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        public async Task SendNotifyApprovePaymentOrder(int orderPaymentId)
        {
            _logger.LogInformation($"{nameof(SendNotifyApprovePaymentOrder)}: orderPaymentId = {orderPaymentId}");
            var orderPayment = _dbContext.EvtOrderPayments.Include(op => op.Order).ThenInclude(o => o.EventDetail).ThenInclude(ed => ed.Event)
                                                          .Include(op => op.Order).ThenInclude(o => o.OrderDetails)
                                                          .FirstOrDefault(e => e.Id == orderPaymentId && e.Deleted == YesNo.NO);
            if(orderPayment == null)
            {
                _logger.LogError($"{nameof(SendNotifyApprovePaymentOrder)}: Không tìm thấy thanh toán: {orderPaymentId}");
                return;
            }
            if (orderPayment.Status != OrderPaymentStatus.DA_THANH_TOAN)
            {
                _logger.LogError($"{nameof(SendNotifyApprovePaymentOrder)}: Thanh toán {orderPaymentId} chưa được duyệt hoặc chưa thanh toán: ");
                return;
            }

            if (orderPayment.Order == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy sổ lệnh orderId: {orderPayment.Order.Id}");
                return;
            }

            DataEmail data = GetData(orderPayment.Order.InvestorId, orderPayment.Order.InvestorIdenId, orderPayment.Order.TradingProviderId);
            var content = new EventApprovrePaymentSucessContent
            {
                CustomerName = data?.InvestorIdentification?.Fullname,
                EventName = orderPayment.Order.EventDetail.Event.Name,
                StartDate = orderPayment.Order.EventDetail.StartDate?.ToString("dd/MM/yyyy HH:mm"),
                EndDate = orderPayment.Order.EventDetail.EndDate?.ToString("dd/MM/yyyy HH:mm"),
                TicketQuantity = orderPayment.Order.OrderDetails.Sum(e => e.Quantity).ToString(),
                TotalMoney = NumberToText.ConvertNumberIS((double?)orderPayment.PaymentAmount),
            };

            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.TB_MUA_VE_THANH_CONG
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.EVENT_THANH_TOAN_THANH_CONG, receiver, data.OtherParams);
        }

        /// <summary>
        /// Thông báo tham gia sự kiện thành công
        /// </summary>
        /// <param name="orderTicketDetailId"></param>
        /// <returns></returns>
        public async Task SendNotifyJoinEvent(int orderTicketDetailId)
        {
            _logger.LogInformation($"{nameof(SendNotifyJoinEvent)}: orderTicketDetailId = {orderTicketDetailId}");
            var orderTicketDetail = _dbContext.EvtOrderTicketDetails.Include(op => op.OrderDetail)
                                                               .ThenInclude(o => o.Order)
                                                               .ThenInclude(ed => ed.EventDetail)
                                                               .ThenInclude(ed => ed.Event)
                                                               .FirstOrDefault(e => e.Id == orderTicketDetailId);
            if (orderTicketDetail == null)
            {
                _logger.LogError($"{nameof(SendNotifyApprovePaymentOrder)}: Không tìm thấy vé: {orderTicketDetailId}");
                return;
            }
            if (orderTicketDetail.Status != EvtOrderTicketStatus.DA_THAM_GIA)
            {
                _logger.LogError($"{nameof(SendNotifyApprovePaymentOrder)}: Vé {orderTicketDetail} chưa tham gia");
                return;
            }

            DataEmail data = GetData(orderTicketDetail.OrderDetail.Order.InvestorId, orderTicketDetail.OrderDetail.Order.InvestorIdenId, orderTicketDetail.OrderDetail.Order.TradingProviderId);
            var content = new EventJoinEventSuccessContent
            {
                CustomerName = data?.InvestorIdentification?.Fullname,
                EventName = orderTicketDetail.OrderDetail.Order.EventDetail.Event.Name,
                StartDate = orderTicketDetail.OrderDetail.Order.EventDetail.StartDate?.ToString("dd/MM/yyyy HH:mm"),
                EndDate = orderTicketDetail.OrderDetail.Order.EventDetail.EndDate?.ToString("dd/MM/yyyy HH:mm"),
                CheckIn = orderTicketDetail.CheckIn?.ToString("dd/MM/yyyy HH:mm:ss")
            };

            var receiver = new Receiver
            {
                Phone = data.Investor?.Phone,
                Email = new EmailNotifi
                {
                    Address = data.Investor?.Email,
                    Title = TitleEmail.TB_THAM_GIA_SU_KIEN_THANH_CONG
                },
                UserId = data.Users?.UserId.ToString(),
                FcmTokens = data.FcmTokens
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.EVENT_THAM_GIA_SU_KIEN_THANH_CONG, receiver, data.OtherParams);
        }

        /// <summary>
        /// Thông báo quản trị khi khách hàng chuyển tiền thành công
        /// </summary>
        /// <param name="orderPaymentId"></param>
        /// <returns></returns>
        public async Task SendAdminNotifyApprovePaymentOrder(int orderPaymentId)
        {
            _logger.LogInformation($"{nameof(SendNotifyApprovePaymentOrder)}: orderPaymentId = {orderPaymentId}");
            var orderPayment = _dbContext.EvtOrderPayments.Include(op => op.Order).ThenInclude(o => o.EventDetail).ThenInclude(ed => ed.Event)
                                                          .Include(op => op.Order).ThenInclude(o => o.OrderDetails)
                                                          .FirstOrDefault(e => e.Id == orderPaymentId && e.Deleted == YesNo.NO);

            var totalOrderPaymet = _dbContext.EvtOrderPayments.Where(e => e.OrderId == orderPayment.OrderId && e.Deleted == YesNo.NO && e.Status == OrderPaymentStatus.DA_THANH_TOAN).Sum(e => e.PaymentAmount) + orderPayment.PaymentAmount;
            if (orderPayment == null)
            {
                _logger.LogError($"{nameof(SendNotifyApprovePaymentOrder)}: Không tìm thấy thanh toán: {orderPaymentId}");
                return;
            }
            if (orderPayment.Status != OrderPaymentStatus.DA_THANH_TOAN)
            {
                _logger.LogError($"{nameof(SendNotifyApprovePaymentOrder)}: Thanh toán {orderPaymentId} chưa được duyệt hoặc chưa thanh toán: ");
                return;
            }

            if (orderPayment.Order == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy sổ lệnh orderId: {orderPayment.Order.Id}");
                return;
            }

            DataEmail data = GetData(orderPayment.Order.InvestorId, orderPayment.Order.InvestorIdenId, orderPayment.Order.TradingProviderId);
            var content = new EventAdminApprovePaymentSuccessContent
            {
                CustomerName = data?.InvestorIdentification?.Fullname,
                EventName = orderPayment.Order.EventDetail.Event.Name,
                StartDate = orderPayment.Order.EventDetail.StartDate?.ToString("dd/MM/yyyy HH:mm"),
                EndDate = orderPayment.Order.EventDetail.EndDate?.ToString("dd/MM/yyyy HH:mm"),
                TicketQuantity = orderPayment.Order.OrderDetails.Sum(e => e.Quantity).ToString(),
                PaymentAmount = NumberToText.ConvertNumberIS((double)orderPayment.PaymentAmount),
                TotalPayment = NumberToText.ConvertNumberIS((double)totalOrderPaymet),
                ContractCode = orderPayment.Order.ContractCodeGen ?? orderPayment.Order.ContractCode,
                Phone = data?.Investor.Phone
            };

            var tradingProvider = _tradingProviderEFRepository.FindById(orderPayment.Order.EventDetail.Event.TradingProviderId);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.TradingProviderNotFound);
                return;
            }

            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Title = TitleEmail.TB_AMDIN_KHACH_CHUYEN_TIEN_MUA_VE_THAM_GIA_SU_KIEN
                },
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.EVENT_ADMIN_CHUYEN_TIEN_MUA_VE_THANH_CONG, receiver,
                new ParamsChooseTemplate { TradingProviderId = orderPayment.Order.EventDetail.Event.TradingProviderId });
        }

        /// <summary>
        /// Thông báo sự kiện sắp diễn ra
        /// </summary>
        /// <param name="eventId"></param>
        /// <returns></returns>
        public async Task SendEventUpComing(int eventDetailId)
        {
            var orders = _dbContext.EvtOrders.Include(o => o.EventDetail)
                                             .ThenInclude(ed => ed.Event)
                                             .Where(o => o.Status == EvtOrderStatus.HOP_LE && o.Deleted == YesNo.NO
                                             && o.EventDetail.Id == eventDetailId);

            foreach (var order in orders)
            {
                DataEmail data = GetData(order.InvestorId, order.InvestorIdenId, order.TradingProviderId);
                var content = new EventSuccessContent
                {
                    CustomerName = data?.InvestorIdentification?.Fullname,
                    EventName = order.EventDetail?.Event?.Name,
                    StartDate = order.EventDetail.StartDate?.ToString("dd/MM/yyyy HH:mm"),
                    EndDate = order.EventDetail.EndDate?.ToString("dd/MM/yyyy HH:mm"),
                };

                var receiver = new Receiver
                {
                    Phone = data.Investor?.Phone,
                    Email = new EmailNotifi
                    {
                        Address = data.Investor?.Email,
                        Title = TitleEmail.TB_SU_KIEN_SAP_DIEN_RA
                    },
                    UserId = data.Users?.UserId.ToString(),
                    FcmTokens = data.FcmTokens
                };
                await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.EVENT_SU_KIEN_SAP_DIEN_RA, receiver, data.OtherParams);
            }
        }

        /// <summary>
        /// Thông báo cho quản trị vien khách hàng yêu cầu nhận vé bản cứng
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task SendAdminNotifyReceiveHardTicket(int orderId)
        {
            _logger.LogInformation($"{nameof(SendAdminNotifyReceiveHardTicket)}: orderId = {orderId}");
            var order = _dbContext.EvtOrders.FirstOrDefault(e => e.Id == orderId && e.Deleted == YesNo.NO);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy sổ lệnh orderId: {orderId}");
                return;
            }
           
            var ticketQuantity = 0;
            var orderDetails = _dbContext.EvtOrderDetails.Where(od => od.OrderId == order.Id);
            if (orderDetails.Any())
            {
                ticketQuantity = orderDetails.Sum(od => od.Quantity);
            }

            var eventDetail = _dbContext.EvtEventDetails.FirstOrDefault(ed => ed.Id == order.EventDetailId && ed.Deleted == YesNo.NO);
            if (eventDetail == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy khung giờ sự kiện: {order.EventDetailId}");
                return;
            }
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == eventDetail.EventId && e.Deleted == YesNo.NO);
            if (eventFind == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy sự kiện: {order.EventDetailId}");
                return;
            }
            DataEmail data = GetData(order.InvestorId, order.InvestorIdenId, order.TradingProviderId);
            var content = new EventAdminNotifyReceiveHardTicketSuccessContent
            {
                CustomerName = data?.InvestorIdentification?.Fullname,
                TicketQuantity = ticketQuantity.ToString(),
                EventName = eventFind.Name,
                StartDate = eventDetail.StartDate?.ToString("dd/MM/yyyy HH:mm"),
                EndDate = eventDetail.EndDate?.ToString("dd/MM/yyyy HH:mm"),
                CreatedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                ContractCode = order.ContractCodeGen ?? order.ContractCode,
                Phone = data?.Investor?.Phone,
            };

            var tradingProvider = _tradingProviderEFRepository.FindById(eventFind.TradingProviderId);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.TradingProviderNotFound);
                return;
            }

            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Title = TitleEmail.TB_ADMIN_KHACH_HANG_NHAN_VE_BAN_CUNG
                },
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.EVENT_ADMIN_KHACH_HANG_NHAN_VE_BAN_CUNG, receiver,
                new ParamsChooseTemplate { TradingProviderId = eventFind.TradingProviderId });
        }

        /// <summary>
        /// Thông báo cho quản trị vien khách hàng yêu cầu nhận hóa đơn
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task SendAdminNotifyReceiveRecipt(int orderId)
        {
            _logger.LogInformation($"{nameof(SendAdminNotifyReceiveRecipt)}: orderId = {orderId}");
            var order = _dbContext.EvtOrders.FirstOrDefault(e => e.Id == orderId && e.Deleted == YesNo.NO);
            if (order == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy sổ lệnh orderId: {orderId}");
                return;
            }

            var ticketQuantity = 0;
            var orderDetails = _dbContext.EvtOrderDetails.Where(od => od.OrderId == order.Id);
            if (orderDetails.Any())
            {
                ticketQuantity = orderDetails.Sum(od => od.Quantity);
            }

            var eventDetail = _dbContext.EvtEventDetails.FirstOrDefault(ed => ed.Id == order.EventDetailId && ed.Deleted == YesNo.NO);
            if (eventDetail == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy khung giờ sự kiện: {order.EventDetailId}");
                return;
            }
            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == eventDetail.EventId && e.Deleted == YesNo.NO);
            if (eventFind == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy sự kiện: {order.EventDetailId}");
                return;
            }
            DataEmail data = GetData(order.InvestorId, order.InvestorIdenId, order.TradingProviderId);
            var content = new EventAdminNotifyReceiveHardTicketSuccessContent
            {
                CustomerName = data?.InvestorIdentification?.Fullname,
                TicketQuantity = ticketQuantity.ToString(),
                EventName = eventFind.Name,
                StartDate = eventDetail.StartDate?.ToString("dd/MM/yyyy HH:mm"),
                EndDate = eventDetail.EndDate?.ToString("dd/MM/yyyy HH:mm"),
                CreatedDate = DateTime.Now.ToString("dd/MM/yyyy HH:mm"),
                Phone = data?.Investor?.Phone,
                ContractCode = order.ContractCodeGen ?? order.ContractCode,
            };

            var tradingProvider = _tradingProviderEFRepository.FindById(eventFind.TradingProviderId);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.TradingProviderNotFound);
                return;
            }

            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Title = TitleEmail.TB_ADMIN_KHACH_HANG_NHAN_HOA_DON
                },
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.EVENT_ADMIN_KHACH_HANG_NHAN_HOA_DON, receiver,
                new ParamsChooseTemplate { TradingProviderId = eventFind.TradingProviderId });
        }

        /// <summary>
        /// Thông báo sự kiện kết thúc
        /// </summary>
        /// <param name="eventId">Id sự kiện</param>
        /// <returns></returns>
        public async Task SendAdminNotifyEventFinished(int eventId)
        {
            _logger.LogInformation($"{nameof(SendAdminNotifyEventFinished)}: eventId = {eventId}");

            var eventFind = _dbContext.EvtEvents.FirstOrDefault(e => e.Id == eventId && e.Deleted == YesNo.NO);
            if (eventFind == null)
            {
                _logger.LogError($"{nameof(SendNotifyRegisterTicket)}: Không tìm thấy sự kiện: {eventId}");
                return;
            }

            var eventDetails = _dbContext.EvtEventDetails.Include(e => e.Tickets)
                                                         .Include(e => e.Orders).ThenInclude(o => o.OrderDetails).ThenInclude(od => od.OrderTicketDetails)
                                                         .Where(e => e.EventId == eventFind.Id && e.Deleted == YesNo.NO && e.Status == EventDetailStatus.KICH_HOAT);

            DateTime? startDate = null;
            DateTime? endDate = null;
            if (eventDetails.Count() > 0)
            {
                endDate = eventDetails.Max(e => e.EndDate);
                startDate = eventDetails.Min(e => e.StartDate);
            }
            var content = new EventAdminEventFinishedContent
            {
                EventName = eventFind.Name,
                StartDate = startDate?.ToString("dd/MM/yyyy HH:mm"),
                EndDate = endDate?.ToString("dd/MM/yyyy HH:mm"),
                TicketQuantity = eventDetails.Sum(e => e.Tickets.Where(e => e.Status == EvtTicketStatus.KICH_HOAT).Sum(t => t.Quantity)).ToString(),
                TicketCheckInQuantity = eventDetails.Sum(e => e.Orders.Sum(o => o.OrderDetails.Sum(od => od.OrderTicketDetails.Where(e => e.CheckIn != null).Count()))).ToString(),
                TicketRegisterSuccessQuantity = eventDetails.Sum(e => e.Orders.Where(o => o.Status == EvtOrderStatus.HOP_LE).Sum(o => o.OrderDetails.Sum(od => od.Quantity))).ToString(),
            };

            var tradingProvider = _tradingProviderEFRepository.FindById(eventFind.TradingProviderId);
            if (tradingProvider == null)
            {
                _defErrorEFRepository.LogError(ErrorCode.TradingProviderNotFound);
                return;
            }

            var receiver = new Receiver
            {
                Email = new EmailNotifi
                {
                    Title = TitleEmail.TB_ADMIN_SU_KIEN_KET_THUC
                },
            };
            await _sharedEmailApiUtils.SendEmailAsync(content, KeyTemplate.EVENT_ADMIN_SU_KIEN_KET_THUC, receiver,
                new ParamsChooseTemplate { TradingProviderId = eventFind.TradingProviderId });
        }
    }
}
