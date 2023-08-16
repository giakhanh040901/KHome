using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.RocketChat;
using EPIC.IdentityEntities.DataEntities;
using EPIC.RocketchatEntities.Dto.Callback;
using EPIC.RocketchatEntities.Dto.Integration;
using EPIC.RocketchatEntities.Dto.Livechat;
using EPIC.RocketchatEntities.Dto.Message;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EPIC.RocketchatDomain.Interfaces
{
    public interface IRocketChatServices
    {
        /// <summary>
        /// Login tài khoản admin
        /// </summary>
        /// <returns></returns>
        Task<ReponseLoginDto> LoginAdmin();
        /// <summary>
        /// Login tài khoản bt
        /// </summary>
        /// <returns></returns>
        Task<ReponseLoginDto> LoginRocketchat();
        /// <summary>
        /// Tạo tài khoản rocketchat
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResponseCreateUserDto> CreateUser(CreateRocketChatUserDto dto);
        /// <summary>
        /// Tạo channel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResponseCreateChannelDto> CreateChannel(CreateChannelDto dto);
        /// <summary>
        /// Sinh pass từ user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        string genPasswordByUser(Users user);
        /// <summary>
        /// Thêm user vào channel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        Task<ResponseAddUserToChannelDto> AddUserToChannel(AddUserToChannelDto dto);
        /// <summary>
        /// Tìm channel
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        Task<ResponseChannelnfoDto> ChannelInfo(string roomName);
        /// <summary>
        /// Sinh channel name
        /// </summary>
        /// <param name="tradingProvider"></param>
        /// <param name="businessCustomer"></param>
        /// <returns></returns>
        string genChannelName(TradingProvider tradingProvider, BusinessCustomer businessCustomer);
        /// <summary>
        /// Xử lý login sso
        /// </summary>
        /// <returns></returns>
        Task<string> processLoginSSO();
        /// <summary>
        /// Logout rocketchat
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public Task<LogoutResponseDto> LogOutSSO(string authToken);

        /// <summary>
        /// Sinh path (channel, group) cho iframe rocketchat
        /// </summary>
        /// <returns></returns>
        public ChannelNameResponseDto GetChannelName();

        /// <summary>
        /// Lấy thông tin của user hiện tại
        /// </summary>
        /// <returns></returns>
        public Task<ResponseCreateUserDto> getUserInfo();

        /// <summary>
        /// Thêm tài khoản rocketchat của user vào channel thuộc đlsc khi đặt lệnh
        /// </summary>
        /// <returns></returns>
        void AddCurrentUserToTradingProviderChannel(int tradingProviderId);

        /// <summary>
        /// Tạo department
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<ResponseCreateDepartmentDto> CreateDepartment(CreateDepartmentDto dto);

        /// <summary>
        /// Đăng ký user làm agent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<ResponseCreateAgentDto> CreateAgent(CreateAgentDto dto);

        /// <summary>
        /// Lấy danh sách department
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public Task<List<DepartmentRocketchat>> GetListDepartment(string name = "", bool useAdmin = false);
        public Task<List<DepartmentRocketchat>> GetListDepartment(List<string> listName);

        /// <summary>
        /// Cập nhật agent của department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<ResponseCommonDto> UpdateAgentDepartment(string departmentId, UpdateAgentDepartmentDto dto);

        /// <summary>
        /// Lấy tên của department mặc định
        /// </summary>
        /// <returns></returns>
        public string GetDefaultDepartmentName();

        /// <summary>
        /// Tạo user trên rocketchat khi app đăng ký investor, cms duyệt thêm mới investor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task CreateRocketchatUserForInvestor(CreateRocketChatUserDto dto);

        /// <summary>
        /// Lấy ds rocketchat department theo investorid
        /// </summary>
        /// <returns></returns>
        public Task<List<DepartmentRocketchat>> GetListDepartmentForInvestor();

        /// <summary>
        /// Xử lý callback request từ webhook omni channel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task ProcessCallbackWebhook(WebhookRequestDto dto);

        /// <summary>
        /// Gửi tin nhắn livechat
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public Task<ResponseCommonDto> SendMessage(SendMessageDto dto);

        /// <summary>
        /// Gửi thông báo tin nhắn giữa 2 investor
        /// </summary>
        /// <param name="input"></param>
        /// <param name="senderPhone"></param>
        /// <param name="receiverPhone"></param>
        /// <returns></returns>
        Task SendDirectMessage(IntegrationDirectDto input);
        /// <summary>
        /// Xử lý gửi hộ app thông tin chat
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<ResponseIntegrationDto> ProcessIntegration(ProcessIntegrationDto input);
    }
}
