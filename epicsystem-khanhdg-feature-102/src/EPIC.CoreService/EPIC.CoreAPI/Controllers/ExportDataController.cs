using EPIC.CoreDomain.Interfaces;
using EPIC.CoreEntities.Dto.ExportData;
using EPIC.Utils;
using EPIC.Utils.Controllers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace EPIC.CoreAPI.Controllers
{
    //[Route("api/core/export-data")]
    //[ApiController]
    public class ExportDataController : BaseController
    {
        private IExportDataServices _exportDataServices;

        public ExportDataController(ILogger<ExportDataController> logger, IExportDataServices exportDataServices)
        {
            _logger = logger;
            _exportDataServices = exportDataServices;
        }

        /// <summary>
        /// Export thông tin nhà đầu tư
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("investor/{token}")]
        [ProducesResponseType(typeof(APIResponse<ExportDataInvestorDto>), (int)HttpStatusCode.OK)]
        public APIResponse ExportDataInvestor(string token, [FromQuery] FilterExportDataDto input)
        {
            try
            {
                var result = _exportDataServices.ExportDataInvestor(token, input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Export thông tin sale
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("sale/{token}")]
        [ProducesResponseType(typeof(APIResponse<ExportDataSaleDto>), (int)HttpStatusCode.OK)]
        public APIResponse ExportDataSale(string token, [FromQuery] FilterExportDataDto input)
        {
            try
            {
                var result = _exportDataServices.ExportDataSale(token, input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Export thông tin InvestOrder
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        [HttpGet("invest-order/{token}")]
        [ProducesResponseType(typeof(APIResponse<ExportDataInvestOrderDto>), (int)HttpStatusCode.OK)]
        public APIResponse ExportDataInvestOrder(string token,[FromQuery] FilterExportDataDto input)
        {
            try
            {
                var result = _exportDataServices.ExportDataInvestOrder(token, input);
                return new APIResponse(result);
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
    }
}
