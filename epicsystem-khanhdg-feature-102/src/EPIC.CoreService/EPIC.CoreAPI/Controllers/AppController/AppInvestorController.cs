using EPIC.CoreDomain.Interfaces;
using EPIC.CoreDomain.Interfaces.v2;
using EPIC.CoreEntities.Dto.CallCenterConfig;
using EPIC.CoreEntities.Dto.InvestorSearch;
using EPIC.CoreEntities.Dto.ManagerInvestor;
using EPIC.CoreSharedEntities.Dto.Investor;
using EPIC.DataAccess.Models;
using EPIC.GarnerEntities.Dto.GarnerDistribution;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net;

namespace EPIC.CoreAPI.Controllers.AppController
{
    [Authorize]
    [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
    [Route("api/core/investor")]
    [ApiController]
    public class AppInvestorController : BaseController
    {
        private readonly IInvestorServices _investorServices;
        private readonly IInvestorSearchService _investorSearchService;
        private readonly ICallCenterConfigService _callCenterConfigService;
        private readonly IInvestorV2Services _investorV2Services;

        public AppInvestorController(IInvestorServices investorServices, IInvestorSearchService investorSearchService, ICallCenterConfigService callCenterConfigService, IInvestorV2Services investorV2Services)
        {
            _investorServices = investorServices;
            _investorSearchService = investorSearchService;
            _callCenterConfigService = callCenterConfigService;
            _investorV2Services = investorV2Services;
        }

        /// <summary>
        /// Danh sách to do
        /// </summary>
        /// <returns></returns>
        [HttpGet("todo/find")]
        [ProducesResponseType(typeof(APIResponse<List<InvestorTodoDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllTodo()
        {
            try
            {
                var result = _investorServices.FindAllTodo();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách to do
        /// </summary>
        /// <returns></returns>
        [HttpGet("todo/find-all")]
        [ProducesResponseType(typeof(APIResponse<List<InvestorTodoDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllTodo()
        {
            try
            {
                var result = _investorServices.GetAllTodo();
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="toDoId"></param>
        /// <returns></returns>
        [HttpPut("todo/seen/{toDoId}")]
        public APIResponse SeenToDo(int toDoId)
        {
            try
            {
                _investorServices.UpdateStatusToDo(toDoId);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách đại lý
        /// </summary>
        /// <returns></returns>
        [HttpGet("trading-provider/find-all")]
        [ProducesResponseType(typeof(APIResponse<List<int>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllTradingProvider()
        {
            try
            {
                return new APIResponse(_investorServices.FindAllTradingProviderIds());
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Tìm kiếm toàn bộ hệ thống, trả ra kết quả các sản phẩm trong 1 danh sách,
        /// danh sách này trả ra thông tin tương tự các phần xử lý xem danh sách trong các sản phẩm khác nhau
        /// </summary>
        /// <returns></returns>
        [HttpGet("search")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(APIResponse<PagingResult<InvestorSearchResultDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllTodo([FromQuery] FilterInvestorSearchDto input)
        {
            try
            {
                return new(_investorSearchService.Search(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy dach sách lịch sử tìm kiếm sản phẩm
        /// danh sách này trả ra thông tin tương tự các phần xử lý xem danh sách trong các sản phẩm khác nhau
        /// </summary>
        /// <returns></returns>
        [HttpGet("search-history")]
        [AllowAnonymous]
        [ProducesResponseType(typeof(APIResponse<PagingResult<InvestorSearchResultDto>>), (int)HttpStatusCode.OK)]
        public APIResponse HistorySearch([FromQuery] FilterInvestorSearchDto input)
        {
            try
            {
                return new(_investorSearchService.HistorySearch(input));
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// API lưu trữ lịch sử tì kiếm mỗi khi investor bấm vào 1 sản phẩm, bấm vào loại sản phẩm nào truyền lên id của loại sản phẩm đó
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add-search-history")]
        [AllowAnonymous]
        public APIResponse AddHistorySearch([FromBody] InvestorSearchHistoryCreateDto input)
        {
            try
            {
                _investorSearchService.AddHistorySearch(input);
                return new();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// API xóa lịch sử tìm kiếm
        /// </summary>
        /// <returns></returns>
        [HttpDelete("delete-search-history")]
        [AllowAnonymous]
        public APIResponse DeleteHistorySearch()
        {
            try
            {
                _investorSearchService.DeleteHistorySearch();
                return new();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách user id trong call center để investor có thể gọi 
        /// (nếu nhiều hơn 1 đại lý trong quan hệ với investor sẽ trả ra danh sách root)
        /// </summary>
        /// <returns></returns>
        [HttpGet("call-center/list-user-id")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<UserIdCallCenterDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetListUserIdCallCenter()
        {
            try
            {
                var result = _callCenterConfigService.GetListUserIdCallCenter();
                return new(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        [HttpGet("contact-list")]
        [ProducesResponseType(typeof(APIResponse<IEnumerable<AppInvestorContacListDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllContactList([FromQuery] AppFilterContactListDto input)
        {
            try
            {
                var result = _investorV2Services.GetAllContactList(input);
                return new(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
