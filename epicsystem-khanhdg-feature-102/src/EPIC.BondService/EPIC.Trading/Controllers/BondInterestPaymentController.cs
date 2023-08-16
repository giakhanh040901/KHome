using EPIC.BondDomain.Interfaces;
using EPIC.BondEntities.Dto.InterestPayment;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace EPIC.BondAPI.Controllers
{
    [Authorize]
    [AuthorizeAdminUserTypeFilter]
    [Route("api/bond/interest-payment")]
    [ApiController]
    public class BondInterestPaymentController : BaseController
    {
        private readonly IBondInterestPaymentService _interestPaymentServices;
        private readonly IConfiguration _configuration;

        public BondInterestPaymentController(
            ILogger<BondInterestPaymentController> logger,
            IBondInterestPaymentService interestPaymentServices,
            IConfiguration configuration)
        {
            _logger = logger;
            _interestPaymentServices = interestPaymentServices;
            _configuration = configuration;
        }

        /// <summary>
        /// Lập chi trả
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        public APIResponse InterestPaymentAdd(InterestPaymentCreateListDto input)
        {
            try
            {
                _interestPaymentServices.InterestPaymentAdd(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách chi trả phân trang
        /// </summary>
        /// <returns></returns>
        [Route("find")]
        [HttpGet]
        public APIResponse FindAll(int? pageSize, int pageNumber, string keyword, int? status, string phone, string contractCode)
        {
            try
            {
                var result = _interestPaymentServices.FindAll(pageSize ?? 100, pageNumber, keyword?.Trim(), status, phone?.Trim(), contractCode?.Trim());
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Danh sách chi trả theo id 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Route("find/{id}")]
        [HttpGet]
        public APIResponse FindById(int id)
        {
            try
            {
                var result = _interestPaymentServices.FindById(id);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Chuyển trạng thái từ đã lập chưa chi trả sang đã chi trả,
        /// nếu hợp động chọn tái tục vốn thì xử lý tái tục vốn
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [Route("change-established-to-paid-status")]
        [HttpPut]
        public async Task<APIResponse> ChangeEstablishedWithOutPayingToPaidStatus(ChangeStatusDto input)
        {
            try
            {
                await _interestPaymentServices.ChangeEstablishedWithOutPayingToPaidStatus(input);
                return new APIResponse(Utils.StatusCode.Success, null, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
