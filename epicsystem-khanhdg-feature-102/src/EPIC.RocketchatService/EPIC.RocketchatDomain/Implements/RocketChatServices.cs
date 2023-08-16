using EPIC.CoreRepositories;
using EPIC.DataAccess.Base;
using EPIC.Entities;
using EPIC.Entities.DataEntities;
using EPIC.Entities.Dto.ManagerInvestor;
using EPIC.Entities.Dto.RocketChat;
using EPIC.IdentityEntities.DataEntities;
using EPIC.IdentityRepositories;
using EPIC.Notification.Services;
using EPIC.RocketchatDomain.Interfaces;
using EPIC.RocketchatEntities.Dto.Callback;
using EPIC.RocketchatEntities.Dto.Integration;
using EPIC.RocketchatEntities.Dto.Message;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.ConstantVariables.Rocketchat;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.ServiceModel;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace EPIC.RocketchatDomain.Implements
{
    public class RocketChatServices : IRocketChatServices
    {
        private readonly IHttpContextAccessor _httpContext;
        private readonly EpicSchemaDbContext _dbContext;
        private readonly string _connectionString;
        private readonly ILogger _logger;
        private readonly IConfiguration _configuration;
        private readonly UserRepository _userRepository;
        private readonly DefErrorEFRepository _defErrorEFRepository;
        private readonly InvestorRepository _investorRepository;
        private readonly InvestorEFRepository _investorEFRepository;
        private readonly TradingProviderRepository _tradingProviderRepository;
        private readonly BusinessCustomerRepository _businessCustomerRepository;
        private readonly TradingFirstMessageEFRepository _tradingFirstMsgRepository;
        private readonly RocketChatNotificationServices _rocketChatNotificationServices;

        private readonly HttpClient _httpClient;
        private const string ENDPOINT_LOGIN = "api/v1/login";
        private const string ENDPOINT_CREATE_USER = "api/v1/users.create";
        private const string ENDPOINT_CREATE_CHANNEL = "api/v1/channels.create";
        private const string ENDPOINT_CREATE_DEPARTMENT = "api/v1/livechat/department";
        private const string ENDPOINT_CREATE_AGENT = "api/v1/livechat/users/agent";

        private const string ENDPOINT_UPDATE_AGENT_IN_DEPARTMENT = "api/v1/livechat/department/:departmentId/agents";

        private const string ENDPOINT_CHANNEL_INFO = "api/v1/channels.info";
        private const string ENDPOINT_USER_INFO = "api/v1/users.info";
        private const string ENDPOINT_GET_LIST_DEPARTMENT = "api/v1/livechat/department";
        private const string ENDPOINT_ADD_USER_TO_CHANNEL = "api/v1/channels.invite";
        private const string ENDPOINT_USER_LOGOUT = "api/v1/users.logout";
        private const string ENDPOINT_LIVECHAT_SEND_MSG = "api/v1/livechat/message";
        private const string ENDPOINT_SEND_MSG = "api/v1/chat.sendMessage";
        private const string ENDPOINT_INTEGRATION_CREATE = "api/v1/integrations.create";


        private const string CONFIG_URL = "RocketchatUrl";
        private const string CONFIG_ADMIN_USERNAME = "Rocketchat:AdminUsername";
        private const string CONFIG_ADMIN_PASSWORD = "Rocketchat:AdminPassword";
        private const string CONFIG_DEFAULT_DEPARTMENT_NAME = "Rocketchat:DefaultDepartmentName";
        private const string CONFIG_SECRET_KEY = "Rocketchat:Secret";

        private readonly string _adminUsername = "";
        private readonly string _adminPassword = "";
        private readonly string _defaultDepartmentName = "";
        private readonly string _secretKey = "";
        private string _authToken = "";
        private string _userId = "";

        public RocketChatServices(IHttpContextAccessor httpContext,
            EpicSchemaDbContext dbContext,
            ILogger<RocketChatServices> logger,
            DatabaseOptions databaseOptions,
            RocketChatNotificationServices rocketChatNotificationServices,
            IConfiguration configuration)
        {
            _configuration = configuration;
            string rocketchatUrl = _configuration.GetSection(CONFIG_URL)?.Value;
            _adminUsername = _configuration.GetSection(CONFIG_ADMIN_USERNAME)?.Value;
            _adminPassword = _configuration.GetSection(CONFIG_ADMIN_PASSWORD)?.Value;
            _defaultDepartmentName = configuration.GetSection(CONFIG_DEFAULT_DEPARTMENT_NAME)?.Value;
            _secretKey = configuration.GetSection(CONFIG_SECRET_KEY)?.Value;
            _httpContext = httpContext;
            _httpClient = new HttpClient()
            {
                Timeout = TimeSpan.FromMilliseconds(4000)
            };
            if (!string.IsNullOrEmpty(rocketchatUrl))
            {
                _httpClient.BaseAddress = new Uri(rocketchatUrl);
            }
            _logger = logger;
            _connectionString = databaseOptions.ConnectionString;
            _dbContext = dbContext;
            _rocketChatNotificationServices = rocketChatNotificationServices;
            _userRepository = new UserRepository(_connectionString, _logger);
            _investorRepository = new InvestorRepository(_connectionString, logger);
            _investorEFRepository = new InvestorEFRepository(dbContext, logger);
            _tradingProviderRepository = new TradingProviderRepository(_connectionString, logger);
            _businessCustomerRepository = new BusinessCustomerRepository(_connectionString, logger);
            _tradingFirstMsgRepository = new TradingFirstMessageEFRepository(dbContext, logger);
            _defErrorEFRepository = new DefErrorEFRepository(_dbContext);
        }

        /// <summary>
        /// Thêm tài khoản rocketchat của user vào channel thuộc đlsc khi đặt lệnh
        /// </summary>
        /// <returns></returns>
        public void AddCurrentUserToTradingProviderChannel(int tradingProviderId)
        {
            // Tạm bỏ chuyển user thành agent khi đặt lệnh

            //var resLoginAdmin = await LoginAdmin();

            //if (resLoginAdmin.status == "success")
            //{
            //    var userInfo = await getUserInfo();

            //    // get department
            //    var departmentName = genChannelName(new TradingProvider { TradingProviderId = tradingProviderId }, new BusinessCustomer() { });
            //    var listDepartment = await GetListDepartment(departmentName);

            //    // add agent vào department
            //    if (userInfo.success && listDepartment != null && listDepartment.Count > 0)
            //    {
            //        await UpdateAgentDepartment(listDepartment[0].Id, new UpdateAgentDepartmentDto
            //        {
            //            Remove = new List<Upsert>(),
            //            Upsert = new List<Upsert>()
            //            {
            //                new Upsert
            //                {
            //                    AgentId = userInfo?.user._id,
            //                    Username = userInfo?.user?.username
            //                }
            //            }
            //        });
            //    }
            //}
        }

        /// <summary>
        /// login
        /// </summary>
        /// <returns></returns>
        public async Task<ReponseLoginDto> LoginAdmin()
        {
            var body = new LoginRocketchatDto
            {
                username = _adminUsername,
                password = _adminPassword
            };
            var response = await login(body);

            if (response.status != "success")
            {
                _logger.LogError($"Không đăng nhập được tài khoản admin với body " + JsonSerializer.Serialize(body));
            }

            return response;
        }

        /// <summary>
        /// Tạo rocket chat user
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ResponseCreateUserDto> CreateUser(CreateRocketChatUserDto dto)
        {
            try
            {
                addHeader(_httpClient.DefaultRequestHeaders);

                string json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                _logger.LogInformation($"Body gọi api tạo user rocketchat => {JsonSerializer.Serialize(dto)}");

                HttpResponseMessage res = await _httpClient.PostAsync(ENDPOINT_CREATE_USER, content);

                _logger.LogInformation($"Gọi xong api tạo user rocketchat => {JsonSerializer.Serialize(res)}");

                string resContentString = await res.Content.ReadAsStringAsync();
                _logger.LogInformation($"Gọi xong api tạo user rocketchat (ReadAsStringAsync) => {resContentString}");

                if (res.StatusCode != HttpStatusCode.OK)
                {
                    return new ResponseCreateUserDto { success = false };
                }

                var resFormatted = JsonSerializer.Deserialize<ResponseCreateUserDto>(resContentString);
                _logger.LogInformation($"Deserialize xong result gọi api tạo user rocketchat => {resFormatted}");

                if (!resFormatted.success)
                {
                    _logger.LogError($"Không tạo được tài khoản rocketchat với body " + JsonSerializer.Serialize(dto));
                }

                return resFormatted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không tạo được tài khoản rocketchat với body " + JsonSerializer.Serialize(dto));
                return new ResponseCreateUserDto { success = false };
            }

        }

        /// <summary>
        /// Login tk bình thường
        /// </summary>
        /// <returns></returns>
        public async Task<ReponseLoginDto> LoginRocketchat()
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);

            var user = _userRepository.FindById(userid);
            var password = genPasswordByUser(user);

            // Nếu là tk được config trong appsettings. Thì sử dụng password trong appsettings
            if (user.UserName == _adminUsername)
            {
                password = _adminPassword;
            }

            var res = await login(new LoginRocketchatDto
            {
                username = user.UserName,
                password = password
            });
            return res;
        }

        /// <summary>
        /// Sinh mật khẩu mặc định cho user của rocket chat
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public string genPasswordByUser(Users user)
        {
            return $"{user.UserName}{_secretKey}";
        }

        /// <summary>
        /// Login rocketchat
        /// </summary>
        /// <returns></returns>
        private async Task<ReponseLoginDto> login(LoginRocketchatDto dto)
        {
            try
            {
                string json = JsonSerializer.Serialize(dto);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await _httpClient.PostAsync(ENDPOINT_LOGIN, content);

                string resContentString = await res.Content.ReadAsStringAsync();

                var resFormatted = JsonSerializer.Deserialize<ReponseLoginDto>(resContentString);

                if (resFormatted.status == "success")
                {
                    _authToken = resFormatted.data.authToken;
                    _userId = resFormatted.data.userId;
                }
                else
                {
                    _logger.LogError($"Không login được rocketchat với body " + JsonSerializer.Serialize(dto));
                }

                return resFormatted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không login được rocketchat với body " + JsonSerializer.Serialize(dto));
                return new ReponseLoginDto { status = "error" };
            }
        }

        /// <summary>
        /// Tạo channel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ResponseCreateChannelDto> CreateChannel(CreateChannelDto dto)
        {
            try
            {
                string pattern = "[0-9a-zA-Z-_.]+";
                if (!Regex.IsMatch(dto.name, pattern))
                {
                    _logger.LogError($"Tên channel không hợp lệ " + JsonSerializer.Serialize(dto));
                    return new ResponseCreateChannelDto { success = false };
                }

                addHeader(_httpClient.DefaultRequestHeaders);


                string json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await _httpClient.PostAsync(ENDPOINT_CREATE_CHANNEL, content);
                string resContentString = await res.Content.ReadAsStringAsync();

                var resFormatted = JsonSerializer.Deserialize<ResponseCreateChannelDto>(resContentString);

                if (!resFormatted.success)
                {
                    _logger.LogError($"Không tạo được channel trong rocketchat với body " + JsonSerializer.Serialize(dto));
                }

                return resFormatted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không tạo được channel trong rocketchat với body " + JsonSerializer.Serialize(dto));
                return new ResponseCreateChannelDto { success = false };
            }

        }

        /// <summary>
        /// Thêm user vào channel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ResponseAddUserToChannelDto> AddUserToChannel(AddUserToChannelDto dto)
        {
            try
            {
                addHeader(_httpClient.DefaultRequestHeaders);

                string json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await _httpClient.PostAsync(ENDPOINT_ADD_USER_TO_CHANNEL, content);
                string resContentString = await res.Content.ReadAsStringAsync();

                var resFormatted = JsonSerializer.Deserialize<ResponseAddUserToChannelDto>(resContentString);

                if (!resFormatted.success)
                {
                    _logger.LogError($"Không thêm được user vào channel với body " + JsonSerializer.Serialize(dto));
                }

                return resFormatted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không thêm được user vào channel với body " + JsonSerializer.Serialize(dto));
                return new ResponseAddUserToChannelDto { success = false };
            }
        }

        /// <summary>
        /// Tìm channel
        /// </summary>
        /// <param name="roomName"></param>
        /// <returns></returns>
        public async Task<ResponseChannelnfoDto> ChannelInfo(string roomName)
        {
            try
            {
                addHeader(_httpClient.DefaultRequestHeaders);

                var url_ = $"{ENDPOINT_CHANNEL_INFO}?roomName={roomName}";

                HttpResponseMessage res = await _httpClient.GetAsync(url_);
                string resContentString = await res.Content.ReadAsStringAsync();

                var resFormatted = JsonSerializer.Deserialize<ResponseChannelnfoDto>(resContentString);

                if (!resFormatted.success)
                {
                    _logger.LogError($"Không tìm thấy thông tin của channel với channelName = {roomName}");
                }

                return resFormatted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không tìm thấy thông tin của channel với channelName = {roomName}");
                return new ResponseChannelnfoDto { success = false };
            }

        }

        /// <summary>
        /// Sinh tên channel
        /// </summary>
        /// <param name="tradingProvider"></param>
        /// <param name="businessCustomer"></param>
        /// <returns></returns>
        public string genChannelName(TradingProvider tradingProvider, BusinessCustomer businessCustomer)
        {
            var name = $"DLSC-{tradingProvider.TradingProviderId}";
            name = name.Replace(" ", "");

            return name;
        }

        private void addHeader(HttpRequestHeaders header)
        {
            if (header.Contains("X-Auth-Token"))
            {
                header.Remove("X-Auth-Token");
            }
            if (header.Contains("X-User-Id"))
            {
                header.Remove("X-User-Id");
            }

            header.Add("X-Auth-Token", _authToken);
            header.Add("X-User-Id", _userId);
        }

        /// <summary>
        /// Xử lý login sso rocket chat. Chưa có tk thì tạo mới
        /// </summary>
        /// <returns></returns>
        public async Task<string> processLoginSSO()
        {
            _logger.LogInformation("Xử lý login sso rocketchat. Nếu chưa có tài khoản thì tạo tài khoản mới.");

            var loginRes = await LoginRocketchat();
            string token = "";
            string rocketchatUserid = "";
            string rocketchatUsername = "";

            bool rocketUserDontExists = loginRes.status == "error" && loginRes.error == "Unauthorized" && loginRes.message == "Unauthorized";

            var userType = CommonUtils.GetCurrentUserType(_httpContext);

            var userId = CommonUtils.GetCurrentUserId(_httpContext);
            var user = _userRepository.FindById(userId);

            // neu chua co tai khoan
            if (rocketUserDontExists)
            {
                _logger.LogInformation("Chuẩn bị tạo tài khoản");
                // login admin de lau auth token
                var loginAdminRes = await LoginAdmin();

                _logger.LogInformation("Gọi xong api login admin");
                if (loginAdminRes.status != "success")
                {
                    return token;
                }

                _logger.LogInformation("Login admin thành công");
                var roles = new List<string> { "user", "livechat-agent" };

                if (userType == UserTypes.EPIC || userType == UserTypes.ROOT_EPIC || userType == UserTypes.SUPER_ADMIN)
                {
                    roles.Add("admin");
                }
                // tao rocket chat user
                var rocketchatUser = await CreateUser(new CreateRocketChatUserDto
                {
                    email = user.Email ?? $"{user.UserName}@gmail.com",
                    name = user.DisplayName ?? user.UserName,
                    password = genPasswordByUser(user),
                    username = user.UserName,
                    roles = roles
                });
                _logger.LogInformation($"Tạo xong tài khoản rocketchat {JsonSerializer.Serialize(rocketchatUser)}");

                if (!rocketchatUser.success)
                {
                    return token;
                }
                else
                {
                    rocketchatUserid = rocketchatUser.user._id;
                    rocketchatUsername = rocketchatUser.user.username;
                }
            }
            else
            {
                _logger.LogInformation("Đã có tài khoản");
                token = loginRes.data.authToken;
                rocketchatUserid = loginRes.data.userId;
                rocketchatUsername = loginRes.data.me.username;
            }

            if (userType == UserTypes.TRADING_PROVIDER || userType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                _logger.LogInformation("Chuẩn bị kiểm tra department");

                // get department
                var tradingProviderId = user.TradingProviderId ?? CommonUtils.GetCurrentTradingProviderId(_httpContext);

                var departmentName = genChannelName(new TradingProvider { TradingProviderId = (int)tradingProviderId }, new BusinessCustomer { });
                var department = await GetListDepartment(departmentName);

                // add agent vào department
                if (department != null && department.Count > 0)
                {
                    await UpdateAgentDepartment(department[0]?.Id, new UpdateAgentDepartmentDto
                    {
                        Remove = new List<Upsert>(),
                        Upsert = new List<Upsert>
                        {
                            new Upsert
                            {
                                AgentId = rocketchatUserid,
                                Username = rocketchatUsername,
                            }
                        }
                    });
                }
            }

            return token;
        }

        /// <summary>
        /// Lấy thông tin user
        /// </summary>
        /// <returns></returns>
        public async Task<ResponseCreateUserDto> getUserInfo()
        {
            var username = CommonUtils.GetCurrentUsername(_httpContext);

            var url_ = $"{ENDPOINT_USER_INFO}?username={username}";

            try
            {
                addHeader(_httpClient.DefaultRequestHeaders);

                HttpResponseMessage res = await _httpClient.GetAsync(url_);
                string resContentString = await res.Content.ReadAsStringAsync();

                var userInfoRes = JsonSerializer.Deserialize<ResponseCreateUserDto>(resContentString);

                if (userInfoRes.success)
                {
                    var userid = userInfoRes.user._id;
                }
                else
                {
                    _logger.LogError($"Không lấy được thông tin user của rocketchat với username = {username}");
                }

                return userInfoRes;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không lấy được thông tin user của rocketchat với username = {username}");
                return new ResponseCreateUserDto { success = false };
            }
        }

        /// <summary>
        /// Logout rocketchat
        /// </summary>
        /// <param name="authToken"></param>
        /// <returns></returns>
        public async Task<LogoutResponseDto> LogOutSSO(string authToken)
        {
            try
            {
                await LoginAdmin();

                var userinfores = await getUserInfo();

                if (userinfores.success)
                {
                    _authToken = authToken;
                    _userId = userinfores.user._id;

                    addHeader(_httpClient.DefaultRequestHeaders);

                    var url_ = $"{ENDPOINT_USER_LOGOUT}";

                    HttpResponseMessage res = await _httpClient.PostAsync(url_, new StringContent("", Encoding.UTF8, "application/json"));
                    string resContentString = await res.Content.ReadAsStringAsync();

                    var resLougout = JsonSerializer.Deserialize<LogoutResponseDto>(resContentString);

                    if (!resLougout.success)
                    {
                        _logger.LogError($"Không log out được user của rocketchat với authToken = {authToken}");
                    }

                    return resLougout;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không log out được user của rocketchat với authToken = {authToken}");
                return new LogoutResponseDto { success = false };
            }

        }

        /// <summary>
        /// Sinh path (channel, group) cho iframe rocketchat
        /// </summary>
        /// <returns></returns>
        public ChannelNameResponseDto GetChannelName()
        {
            var userid = CommonUtils.GetCurrentUserId(_httpContext);

            var user = _userRepository.FindById(userid);

            if (user.UserType == UserTypes.TRADING_PROVIDER || user.UserType == UserTypes.ROOT_TRADING_PROVIDER)
            {
                var tradingProviderId = CommonUtils.GetCurrentTradingProviderId(_httpContext);
                var channelName = genChannelName(new TradingProvider { TradingProviderId = tradingProviderId }, new BusinessCustomer { });

                return new ChannelNameResponseDto
                {
                    ChannelPath = $"channel/{channelName}",
                    UserType = user.UserType,
                };
            }

            if (user.UserType == UserTypes.SUPER_ADMIN || user.UserType == UserTypes.EPIC || user.UserType == UserTypes.ROOT_EPIC)
            {
                return new ChannelNameResponseDto
                {
                    ChannelPath = "",
                    UserType = user.UserType,
                };
            }

            _logger.LogError($"User này chưa thuộc bất kỳ channel nào. UserId = {userid}");

            return new ChannelNameResponseDto
            {
                ChannelPath = "",
            };
        }

        /// <summary>
        /// Tạo department
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ResponseCreateDepartmentDto> CreateDepartment(CreateDepartmentDto dto)
        {
            try
            {
                string pattern = "[0-9a-zA-Z-_.]+";
                if (!Regex.IsMatch(dto.Department.Name, pattern))
                {
                    _logger.LogError($"Tên department không hợp lệ " + JsonSerializer.Serialize(dto));
                    return new ResponseCreateDepartmentDto { Success = false };
                }
                addHeader(_httpClient.DefaultRequestHeaders);

                string json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await _httpClient.PostAsync(ENDPOINT_CREATE_DEPARTMENT, content);
                string resContentString = await res.Content.ReadAsStringAsync();

                var resFormatted = JsonSerializer.Deserialize<ResponseCreateDepartmentDto>(resContentString);

                if (!resFormatted.Success)
                {
                    _logger.LogError($"Không tạo được department với body " + JsonSerializer.Serialize(dto));
                }

                return resFormatted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không tạo được department với body " + JsonSerializer.Serialize(dto));
                return new ResponseCreateDepartmentDto { Success = false };
            }
        }

        /// <summary>
        /// Đăng ký user làm agent
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ResponseCreateAgentDto> CreateAgent(CreateAgentDto dto)
        {
            try
            {
                addHeader(_httpClient.DefaultRequestHeaders);

                string json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage res = await _httpClient.PostAsync(ENDPOINT_CREATE_AGENT, content);
                string resContentString = await res.Content.ReadAsStringAsync();

                var resFormatted = JsonSerializer.Deserialize<ResponseCreateAgentDto>(resContentString);

                if (!resFormatted.Success)
                {
                    _logger.LogError($"Không tạo được agent với body " + JsonSerializer.Serialize(dto));
                    _logger.LogError(resFormatted.Error);
                }

                return resFormatted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không tạo được agent với body " + JsonSerializer.Serialize(dto));
                return new ResponseCreateAgentDto { Success = false };
            }
        }

        /// <summary>
        /// Lấy danh sách department
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public async Task<List<DepartmentRocketchat>> GetListDepartment(string name = "", bool useAdmin = false)
        {
            try
            {
                if (useAdmin)
                {
                    await LoginAdmin();
                }

                addHeader(_httpClient.DefaultRequestHeaders);


                HttpResponseMessage res = await _httpClient.GetAsync(ENDPOINT_GET_LIST_DEPARTMENT);
                string resContentString = await res.Content.ReadAsStringAsync();

                var resFormatted = JsonSerializer.Deserialize<ResponseGetListDepartmentDto>(resContentString);

                if (!resFormatted.Success)
                {
                    _logger.LogError($"Không get được danh sách department ");
                    return null;
                }

                if (!string.IsNullOrEmpty(name))
                {
                    return resFormatted.Departments.Where(d => d.Name == name)?.ToList();
                }
                return resFormatted.Departments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không get được danh sách department");
                return null;
            }
        }

        public async Task<List<DepartmentRocketchat>> GetListDepartment(List<string> listName)
        {
            try
            {
                addHeader(_httpClient.DefaultRequestHeaders);

                HttpResponseMessage res = await _httpClient.GetAsync(ENDPOINT_GET_LIST_DEPARTMENT);
                string resContentString = await res.Content.ReadAsStringAsync();

                var resFormatted = JsonSerializer.Deserialize<ResponseGetListDepartmentDto>(resContentString);

                if (!resFormatted.Success)
                {
                    _logger.LogError($"Không get được danh sách department ");
                    return null;
                }

                if (listName.Count > 0)
                {
                    return resFormatted.Departments.Where(d => listName.Contains(d.Name))?.ToList();
                }
                return resFormatted.Departments;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không get được danh sách department");
                return null;
            }
        }

        /// <summary>
        /// Cập nhật agent của department
        /// </summary>
        /// <param name="departmentId"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ResponseCommonDto> UpdateAgentDepartment(string departmentId, UpdateAgentDepartmentDto dto)
        {
            try
            {
                addHeader(_httpClient.DefaultRequestHeaders);

                string json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string endpoint = ENDPOINT_UPDATE_AGENT_IN_DEPARTMENT.Replace(":departmentId", departmentId);

                HttpResponseMessage res = await _httpClient.PostAsync(endpoint, content);
                string resContentString = await res.Content.ReadAsStringAsync();

                var resFormatted = JsonSerializer.Deserialize<ResponseCommonDto>(resContentString);

                if (!resFormatted.Success)
                {
                    _logger.LogError($"Không cập nhật được agent của department với body " + JsonSerializer.Serialize(dto));
                    _logger.LogError(resFormatted.Error);
                }

                return resFormatted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không cập nhật được agent của department với body " + JsonSerializer.Serialize(dto));
                return new ResponseCommonDto { Success = false };
            }
        }

        /// <summary>
        /// Lấy tên của department mặc định
        /// </summary>
        /// <returns></returns>
        public string GetDefaultDepartmentName()
        {
            return _defaultDepartmentName;
        }

        /// <summary>
        /// Tạo user trên rocketchat khi app đăng ký investor, cms duyệt thêm mới investor
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task CreateRocketchatUserForInvestor(CreateRocketChatUserDto dto)
        {
            await LoginAdmin();

            if (dto.roles == null)
            {
                dto.roles = new List<string> { "user" };
            }

            // tạo user rocketchat
            await CreateUser(dto);

            // get department chung
            var listDepartment = await GetListDepartment(_defaultDepartmentName);

            // tạo department chung
            //if (listDepartment == null || listDepartment.Count == 0)
            //{
            //    await CreateDepartment(new CreateDepartmentDto
            //    {
            //        Department = new CreateDepartmentRocketchat
            //        {
            //            Name = _defaultDepartmentName,
            //            Description = _defaultDepartmentName,
            //            Enabled = true,
            //            ShowOnOfflineForm = true,
            //            ShowOnRegistration = true,
            //            Email = "defaultmail@example.com"
            //        },
            //        Agents = new List<Agent>()
            //    });
            //}
        }

        /// <summary>
        /// Lấy ds rocketchat department theo investorid
        /// </summary>
        /// <returns></returns>
        public async Task<List<DepartmentRocketchat>> GetListDepartmentForInvestor()
        {
            var investorId = CommonUtils.GetCurrentInvestorId(_httpContext);

            var listTrading = _investorRepository.GetListTradingByInvestorId(investorId);

            if (listTrading.Count > 0)
            {
                var listDepartmentName = listTrading.Select(trading => genChannelName(trading, new BusinessCustomer { }))?.ToList();

                listDepartmentName.Add(_defaultDepartmentName);

                var resLoginAdmin = await LoginAdmin();

                if (resLoginAdmin.status == "success")
                {
                    var listDepartment = await GetListDepartment(listDepartmentName?.ToList());
                    return listDepartment;
                }
            }

            return null;
        }

        /// <summary>
        /// Gửi tin nhắn livechat
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task<ResponseCommonDto> SendMessage(SendMessageDto dto)
        {
            _logger.LogInformation($"${nameof(SendMessage)} => {JsonSerializer.Serialize(dto)}");

            try
            {
                addHeader(_httpClient.DefaultRequestHeaders);
                string json = JsonSerializer.Serialize(dto);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                string endpoint = ENDPOINT_SEND_MSG;

                HttpResponseMessage res = await _httpClient.PostAsync(endpoint, content);
                string resContentString = await res.Content.ReadAsStringAsync();

                var resFormatted = JsonSerializer.Deserialize<ResponseCommonDto>(resContentString);

                if (!resFormatted.Success)
                {
                    _logger.LogError($"Không gửi được tin nhắn: " + JsonSerializer.Serialize(dto));
                    //_logger.LogError(resFormatted.Error);
                }

                return resFormatted;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Không gửi được tin nhắn: " + JsonSerializer.Serialize(dto));
                return new ResponseCommonDto { Success = false };
            }
        }

        /// <summary>
        /// Xử lý callback request từ webhook omni channel
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task ProcessCallbackWebhook(WebhookRequestDto dto)
        {
            _logger.LogInformation($"Body api rc webhook => {JsonSerializer.Serialize(dto)}");

            // Live chat session vừa được khởi tạo
            // Nhanh trí chào luôn
            if (dto.type == RcWebhookTypes.LivechatSessionStart)
            {
                string msg = "Xin chào! Chúng tôi có thể giúp gì được cho bạn?";

                // token là phone của investor
                var phone = dto.visitor.token;

                var tradingMsg = _tradingFirstMsgRepository.FindTradingProviderByInvestorPhone(phone);
                if (tradingMsg != null && !string.IsNullOrEmpty(tradingMsg.Message))
                {
                    msg = tradingMsg.Message;
                }

                await LoginAdmin();
                await SendMessage(new SendMessageDto
                {
                    message = new RocketchatEntities.Dto.Message.Message
                    {
                        msg = msg,
                        rid = dto._id,
                    }
                });
            }
            // Visitor gửi tin nhắn
            else if (dto.type == RcWebhookTypes.Message)
            {
                var investor = _investorEFRepository.GetByEmailOrPhone(dto.visitor?.token)
                    .ThrowIfNull(ErrorCode.InvestorNotFound, $"Không tìm thấy investor có phone = {dto.visitor?.token}");
                var msgContent = dto.messages[0]?.msg ?? "";
                await _rocketChatNotificationServices.SendNotificationAgentSentMsg(investor.InvestorId, msgContent);
            }
        }

        /// <summary>
        /// Gửi thông báo cho receiver
        /// </summary>
        /// <param name="input"></param>
        /// <param name="senderPhone"></param>
        /// <param name="receiverPhone"></param>
        /// <returns></returns>
        public async Task SendDirectMessage(IntegrationDirectDto input)
        {
            var senderInvestorId = CommonUtils.GetCurrentInvestorId(_httpContext);
            _logger.LogInformation($"{nameof(SendDirectMessage)} input = {JsonSerializer.Serialize(input)}, senderPhone = {senderInvestorId}");
            var sender = _investorEFRepository.FindActiveInvestorById(senderInvestorId)
                    .ThrowIfNull(ErrorCode.InvestorNotFound, $"Không tìm thấy investor có Id = {senderInvestorId}");
            var receiver = _investorEFRepository.GetByEmailOrPhone(input.ReceiverPhone)
                    .ThrowIfNull(ErrorCode.InvestorNotFound, $"Không tìm thấy investor có phone = {input.ReceiverPhone}");
            await _rocketChatNotificationServices.SendNotificationSenderSendMsgToReceiver(sender.InvestorId, receiver.InvestorId, input);
        }

        public async Task<ResponseIntegrationDto> ProcessIntegration(ProcessIntegrationDto input)
        {
            _logger.LogInformation($"{nameof(ProcessIntegration)} input = {JsonSerializer.Serialize(input)}");

            var result = new ResponseIntegrationDto();

            //Đăng nhập với admin
            var loginResponse = await LoginAdmin();

            if (loginResponse.status == "error")
            {
                _defErrorEFRepository.ThrowException(Utils.ErrorCode.UserLoginWithAdminFailed);
            }

            //Cấu hình header và gọi request
            using (HttpClient httpClient = new())
            {
                string json = JsonSerializer.Serialize(input);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                content.Headers.Add("X-Auth-Token", loginResponse.data.authToken);
                content.Headers.Add("X-User-Id", loginResponse.data.userId);
                HttpResponseMessage res = null;
                string resContentString = "";
                try
                {
                    res = await _httpClient.PostAsync(ENDPOINT_INTEGRATION_CREATE, content);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, $"Có lỗi xảy ra khi gọi Rocketchat Url {ENDPOINT_INTEGRATION_CREATE}, RequestBody = {json}, exception: {ex.Message}");
                }

                if (res != null)
                {
                    resContentString = await res.Content.ReadAsStringAsync();
                    result = JsonSerializer.Deserialize<ResponseIntegrationDto>(resContentString);
                }
                else
                {
                    throw new FaultException(new FaultReason($"Lỗi không gọi được API {ENDPOINT_INTEGRATION_CREATE}"), new FaultCode(((int)Utils.ErrorCode.HttpRequestException).ToString()), "");
                }
            }
            return result;
        }
    }
}
