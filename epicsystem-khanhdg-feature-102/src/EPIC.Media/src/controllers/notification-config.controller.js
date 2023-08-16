const catchAsync = require("../utils/catchAsync");
const { notificationConfig } = require("../services");
const httpStatus = require("http-status");

const saveNotificationConfiguration = catchAsync(async (req, res) => {
  if(req.user.isSuperAdmin) {
    req.body.trading_provider_id = 0;
  }else {
    req.body.trading_provider_id = req.user.trading_provider_id;
  }
  const config = await notificationConfig.saveNotificationConfig(req.body.trading_provider_id, req.body);
  res.status(httpStatus.CREATED).send(config);
});

const getNotificationConfiguration = catchAsync(async (req, res) => {
  if(req.user.isSuperAdmin) {
    req.body.trading_provider_id = 0;
  }
    const config = await notificationConfig.getNotificationConfigByTradingProviderId(req.user.trading_provider_id);
  res.status(httpStatus.OK).send(config);
});



module.exports = {
  saveNotificationConfiguration,
  getNotificationConfiguration
}

