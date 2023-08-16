const httpStatus = require('http-status');
const pick = require('../utils/pick');
const ApiError = require('../utils/ApiError');
const catchAsync = require('../utils/catchAsync');
const { mediaService, garnerDescriptionService, knowledgeService} = require('../services');

module.exports.updateGarnerDesc = catchAsync(async (req, res) => {
  req.body.trading_provider_id = req.user.trading_provider_id;
  await garnerDescriptionService.updateGarnerDescription(req.garnerDesc.id, req.body)
  res.status(httpStatus.OK).send({status: 'OK'});
});

module.exports.updateGarnerDescImages = catchAsync(async (req, res) => {
  req.body.trading_provider_id = req.user.trading_provider_id;
  await garnerDescriptionService.updateGarnerDescriptionImages(req.garnerDesc.id, req.body)
  res.status(httpStatus.OK).send({status: 'OK'});
});

module.exports.updateGarnerDescFeatures = catchAsync(async (req, res) => {
  req.body.trading_provider_id = req.user.trading_provider_id;
  await garnerDescriptionService.updateGarnerDescriptionFeatures(req.garnerDesc.id, req.body.data)
  res.status(httpStatus.OK).send({status: 'OK'});
});


module.exports.getGarnerDesc = catchAsync(async (req, res) => {
  let filter = {}
  if(req.user.user_type == 'E' || req.user.user_type == 'RE') {
    filter.isRoot = true
  }else {
    filter.trading_provider_id =  req.user.trading_provider_id
  }
  const garnerDesc = await garnerDescriptionService.getGarnerDescriptionByFilter(filter)
  res.status(httpStatus.OK).send(garnerDesc);
});

module.exports.getGarnerDescMobile = catchAsync(async (req, res) => {
  let filter = {}
  if(req.tradingProvidersCanAccess) {
    filter.trading_provider_id = Number(req.tradingProvidersCanAccess)
  }else {
    filter.isRoot = true;
  }
  let garnerDesc = await garnerDescriptionService.getGarnerDescriptionMobile(filter)
  if(!garnerDesc) {
    garnerDesc = await garnerDescriptionService.getGarnerDescriptionByFilter({ isRoot: true })
  }
  if(!garnerDesc) {
    garnerDesc = {
      "descriptionContentType" : "MARKDOWN",
      "status" : "ACTIVE",
      "isRoot" : true,
      "tradingProviderName" : "",
      "images" : [
        {
          "position" : "banner",
          "status" : "ACTIVE",
          "order" : 1,
          "path" : "api/file/get?folder=media&file=epic-468dc94514d4433d8267e46449fcf01c.jpg"
        },
        {
          "position" : "hinh_anh_noi_bat",
          "status" : "ACTIVE",
          "order" : 2,
          "path" : "api/file/get?folder=media&file=epic-8f2495445168408aa753ae9f568c3b03.jpg"
        }
      ],
      "description" : "Một sản phẩm đầu tư vượt trội! Khách hàng có thể đầu tư tích lũy với số tiền nhỏ không kỳ hạn thông qua việc mua hoặc hợp tác đầu tư vào chứng chỉ tiền gửi/cổ phiếu ưu đãi của các tổ chức uy tín. Đặc biệt có thể rút tiền linh hoạt bất kỳ lúc nào với mức lợi tức hấp dẫn, tích lũy đến thời điểm nào sẽ được hưởng lợi tức đến thời điểm đó.",
      "features" : [
        {
          "status" : "ACTIVE",
          "order" : 1,
          "iconUri" : "api/file/get?folder=media&file=epic-0ad1e07c941f4183831a19150147cad0.png",
          "description" : "Đầu tư không kỳ hạn"
        },
        {
          "status" : "ACTIVE",
          "order" : 2,
          "iconUri" : "api/file/get?folder=media&file=epic-970d6f4674334848a1a5bb208bd29ba1.png",
          "description" : "Lợi tức lũy tiến theo thời gian"
        },
        {
          "status" : "ACTIVE",
          "order" : 3,
          "iconUri" : "api/file/get?folder=media&file=epic-4fa02bcbeea34fc6ba0844b9cd683607.png",
          "description" : "Nạp rút mọi thời điểm"
        },
        {
          "status" : "ACTIVE",
          "order" : 4,
          "iconUri" : "api/file/get?folder=media&file=epic-bcb2aff085044929b4b8517121a690bd.png",
          "description" : "Tích lũy chỉ từ 50k"
        }
      ]
    }
  }

  res.status(httpStatus.OK).send(garnerDesc);
});



