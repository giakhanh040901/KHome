using EPIC.DataAccess.Models;
using EPIC.LoyaltyDomain.Interfaces;
using EPIC.LoyaltyEntities.Dto.LoyLuckyProgram;
using EPIC.LoyaltyEntities.Dto.LoyLuckyProgramInvestor;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace EPIC.LoyaltyAPI.Controllers
{
    [Route("api/loyalty/lucky-program")]
    [ApiController]
    public class LoyLuckyProgramController : BaseController
    {
        private readonly ILoyLuckyProgramServices _loyLuckyProgramServices;

        public LoyLuckyProgramController(ILogger<LoyLuckyProgramController> logger,
            ILoyLuckyProgramServices loyLuckyProgramServices)
        {
            _logger = logger;
            _loyLuckyProgramServices = loyLuckyProgramServices;
        }

        /// <summary>
        /// Thêm mới chương trình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("add")]
        [ProducesResponseType(typeof(APIResponse<ViewCreateLuckyProgramDto>), (int)HttpStatusCode.OK)]
        public APIResponse Add([FromForm] CreateLuckyProgramDto input)
        {
            try
            {
                var result = _loyLuckyProgramServices.Add(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật chương trình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPut("update")]
        public APIResponse Update([FromForm] UpdateLoyLuckyProgramDto input)
        {
            try
            {
                _loyLuckyProgramServices.Update(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật cài đặt thời gian tham gia chương trình
        /// </summary>
        /// <param name="input"></param>
        [HttpPut("update-setting")]
        public APIResponse UpdateSetting(UpdateLuckyLoyProgramSettingDto input)
        {
            try
            {
                _loyLuckyProgramServices.UpdateSetting(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Cập nhật trạng thái chương trình
        /// </summary>
        /// <param name="luckyProgramId"></param>
        /// <returns></returns>
        [HttpPut("change-status/{luckyProgramId}")]
        public APIResponse ChangeStatusLuckyProgram(int luckyProgramId)
        {
            try
            {
                _loyLuckyProgramServices.ChangeStatus(luckyProgramId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }


        /// <summary>
        /// Xóa chương trình
        /// </summary>
        /// <param name="luckyProgramId"></param>
        /// <returns></returns>
        [HttpPut("delete/{luckyProgramId}")]
        public APIResponse DeleteLuckyProgram(int luckyProgramId)
        {
            try
            {
                _loyLuckyProgramServices.Delete(luckyProgramId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách chương trình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewLoyLuckyProgramDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllLuckyProgram([FromQuery] FilterLoyLuckyProgramDto input)
        {
            try
            {
                var result = _loyLuckyProgramServices.FindAllLuckyProgram(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách lịch sử tham gia
        /// </summary>
        [HttpGet("history")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<LoyLuckyProgramHistoryDto>>), (int)HttpStatusCode.OK)]
        public APIResponse LuckyProgramHistory([FromQuery] FilterLoyLuckyProgramHistoryDto input)
        {
            try
            {
                var result = _loyLuckyProgramServices.LuckyProgramHistory(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách lịch sử trúng thưởng
        /// </summary>
        [HttpGet("prize-history")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<LoyLuckyProgramPrizeHistoryDto>>), (int)HttpStatusCode.OK)]
        public APIResponse LuckyProgramPrizeHistory([FromQuery] FilterLoyLuckyProgramPrizeHistoryDto input)
        {
            try
            {
                var result = _loyLuckyProgramServices.LuckyProgramPrizeHistory(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }

        }
        /// <summary>
        /// Xem chi tiết chương trình
        /// </summary>
        /// <returns></returns>
        [HttpGet("find-by-id/{luckyProgramId}")]
        [ProducesResponseType(typeof(APIResponse<LoyLuckyProgramDto>), (int)HttpStatusCode.OK)]
        public APIResponse FindByIdLuckyProgram(int luckyProgramId)
        {
            try
            {
                var result = _loyLuckyProgramServices.FindById(luckyProgramId);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thêm khách hàng tham gia
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost("investor/add")]
        public APIResponse AddLuckyProgramInvestor(CreateLuckyProgramInvestorDto input)
        {
            try
            {
                _loyLuckyProgramServices.AddLuckyProgramInvestor(input);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xóa khách hàng tham gia/ Khi khách hàng chưa tham gia 
        /// </summary>
        [HttpPut("investor/delete/{luckyProgramInvestorId}")]
        public APIResponse DeleteLuckyProgramInvestor(int luckyProgramInvestorId)
        {
            try
            {
                _loyLuckyProgramServices.DeleteLuckyProgramInvestor(luckyProgramInvestorId);
                return new APIResponse();
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Xem danh sách nhà đầu tư tham gia chương trình
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("investor/find-all")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<ViewLoyLuckyProgramDto>>), (int)HttpStatusCode.OK)]
        public APIResponse FindAllLuckyProgramInvestor([FromQuery] FilterLoyLuckyProgramInvestorDto input)
        {
            try
            {
                var result = _loyLuckyProgramServices.FindAllLuckyProgramInvestor(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy danh sách nhà đầu tư của đại lý
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("get-all-investor")]
        [ProducesResponseType(typeof(APIResponse<PagingResult<LoyInvestorOfTradingDto>>), (int)HttpStatusCode.OK)]
        public APIResponse GetAllInvestorByTrading([FromQuery] FilterLoyInvestorOfTradingDto input)
        {
            try
            {
                var result = _loyLuckyProgramServices.GetAllInvestorByTrading(input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
