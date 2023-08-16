using EPIC.BondDomain.Interfaces;
using EPIC.Entities.Dto.Bond;
using EPIC.Utils;
using EPIC.Utils.ConstantVariables.Identity;
using EPIC.Utils.Controllers;
using EPIC.WebAPIBase.FIlters;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.ServiceModel;

namespace EPIC.BondAPI.Controllers.InvestorControllers
{
    [Route("api/bond/product-bond")]
    [ApiController]
    public class AppProductBondController : BaseController
    {
        /// <summary>
        /// Data Ưu đãi
        /// </summary>
        public static readonly List<PromotionInvestor> promotionInvestors = new List<PromotionInvestor>(){
            new PromotionInvestor
            {
                InvestorId = 1,
                PolicyId = 1,
                Promotions = new List<Promotion>()
                {
                    new Promotion
                    {    Id = 1,
                         Title = "Giảm giá 30% khi mua The Hignland",
                         MoneyRequired = 1000000000
                    },
                    new Promotion
                    {
                         Id = 2,
                         Title = "Thẻ cào điện thoại",
                         MoneyRequired = 2000000000
                    },
                    new Promotion
                    {
                         Id = 3,
                         Title = "Giảm giá 30% khi mua The Hignland",
                         MoneyRequired = 1000000000
                    }
                }
            },
            new PromotionInvestor
            {
                InvestorId = 1,
                PolicyId = 2,
                Promotions = new List<Promotion>()
                {
                    new Promotion
                    {
                         Id = 4,
                         Title = "Giảm giá 30% khi mua The Hignland",
                         MoneyRequired = 1000000000
                    },
                    new Promotion
                    {
                         Id = 5,
                         Title = "Thẻ cào điện thoại",
                         MoneyRequired = 2000000000
                    },
                    new Promotion
                    {
                         Id = 6,
                         Title = "Giảm giá 30% khi mua The Hignland",
                         MoneyRequired = 1000000000
                    }
                }
            },
            new PromotionInvestor
            {
                InvestorId = 1,
                PolicyId = 3,
                Promotions = new List<Promotion>()
                {
                    new Promotion
                    {
                         Id = 7,
                         Title = "Giảm giá 30% Vé vào khu vui chơi Royal City",
                         MoneyRequired = 1000000000
                    },
                    new Promotion
                    {
                         Id = 8,
                         Title = "Thẻ cào điện thoại",
                         MoneyRequired = 2000000000
                    }
                }
            },
            new PromotionInvestor
            {
                InvestorId = 2,
                PolicyId = 1,
                Promotions = new List<Promotion>()
                {
                    new Promotion
                    {
                         Id = 9,
                         Title = "Giảm giá 20% khi mua The Hignland",
                         MoneyRequired = 500000000
                    },
                    new Promotion
                    {
                         Id = 10,
                         Title = "Thẻ cào điện thoại",
                         MoneyRequired = 1000000000
                    },
                    new Promotion
                    {
                         Id = 11,
                         Title = "Giảm giá 30% khi mua The Hignland",
                         MoneyRequired = 1000000000
                    }
                }
            }
        };

        private readonly IBondProductBondInfoService _productBondInfoServices;
        private readonly IBondSecondaryService _productBondSecondaryServices;
        private readonly IBackgroundJobClient _backgroundJobs;

        public AppProductBondController(
            ILogger<AppProductBondController> logger,
            IBondProductBondInfoService productBondInfoServices,
            IBondIssuerService issuerService,
            IBondSecondaryService productBondSecondaryServices,
            IBackgroundJobClient backgroundJobs)
        {
            _logger = logger;
            _productBondInfoServices = productBondInfoServices;
            _productBondSecondaryServices = productBondSecondaryServices;
            _backgroundJobs = backgroundJobs;
        }

        [HttpGet("test-job")]
        public IActionResult Test()
        {
            _backgroundJobs.Enqueue(() => _productBondSecondaryServices.FindAllBondSecondary(null, false));
            return Ok();
        }

        #region lấy thông tin trái phiếu, chính sách
        /// <summary>
        /// Lấy danh sách sản phẩm, lấy danh sách trái phiếu, ds kỳ hạn,
        /// Nếu là khách hàng đăng nhập thì sẽ xem được bảng hàng cố định
        /// </summary>
        /// <param name="keyword"></param>
        /// <param name="orderByInterestDesc"></param>
        /// <returns></returns>
        [HttpGet("find")]
        public APIResponse FindAllBondSecondary(string keyword, bool orderByInterestDesc)
        {
            try
            {
                var result = _productBondSecondaryServices.FindAllBondSecondary(keyword?.Trim(), orderByInterestDesc);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Thông tin chi tiết trái phiếu
        /// </summary>
        /// <param name="bondSecondaryId"></param>
        /// <returns></returns>
        [HttpGet("find/{bondSecondaryId}")]
        public APIResponse FindBondById([Range(1, int.MaxValue, ErrorMessage = "bondSecondaryId không được nhỏ hơn 1")] int bondSecondaryId)
        {
            try
            {
                var result = _productBondSecondaryServices.FindBondById(bondSecondaryId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get ưu đãi
        /// </summary>
        [HttpGet]
        [Route("policy/get-promotion/{policyId}")]
        public APIResponse GetPromotion([Range(1, int.MaxValue)] int policyId)
        {
            try
            {
                var promotions = promotionInvestors.Where(p => p.InvestorId == 1 && p.PolicyId == policyId).Select(p => p.Promotions);
                return new APIResponse(Utils.StatusCode.Success, promotions, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Lấy chính sách (sản phẩm), tất cả chính sách còn mở, cho show app
        /// </summary>
        /// <param name="bondSecondaryId"></param>
        /// <returns></returns>
        [Authorize]
        [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
        [HttpGet("policy/{bondSecondaryId}")]
        public APIResponse FindAllPolicy([Range(1, int.MaxValue)] int bondSecondaryId)
        {
            try
            {
                var result = _productBondSecondaryServices.FindAllListPolicy(bondSecondaryId);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }

        /// <summary>
        /// Get kỳ hạn, còn show app của chính sách đang chọn
        /// <param name="policyId"></param>
        /// <param name="totalValue"></param>
        /// </summary>
        [Authorize]
        [AuthorizeUserTypeFilter(UserTypes.INVESTOR)]
        [HttpGet("policy-detail/{policyId}")]
        public APIResponse FindAllPolicyDetail(int policyId, [Range(1, double.MaxValue)] [Required] decimal? totalValue)
        {
            try
            {
                var result = _productBondSecondaryServices.FindAllListPolicyDetail(policyId, totalValue ?? 0);
                return new APIResponse(Utils.StatusCode.Success, result, 200, "Ok");
            }
            catch (Exception ex)
            {
                return OkException(ex);
            }
        }
        #endregion
    }
}
