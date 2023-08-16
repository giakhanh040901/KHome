const httpStatus = require('http-status');
const { TradingProviderNotificationConfig } = require('../models');
const ApiError = require('../utils/ApiError');
const defaultConfig = require("../config/config");

/**
 * Create a user
 * @param {Object} notificationBody
 * @returns {Promise<Notification>}
 */
const saveNotificationConfig = async (tradingProviderId, notificationConfigBody) => {
  let config = await TradingProviderNotificationConfig.findOneAndUpdate({
    trading_provider_id: tradingProviderId
  }, notificationConfigBody, {
    new: true,
    upsert: true // Make this update into an upsert
  });
  return config
};

const getNotificationConfigByTradingProviderId = async (tradingProviderId) => {
  let config = await TradingProviderNotificationConfig.findOne({
    trading_provider_id: tradingProviderId
  })
  if(!config) {
    return {
      pushAppTitle: "EPIC",
      smsConfig: {
        USERNAME: defaultConfig.oneSMS.username,
        PASSWORD: defaultConfig.oneSMS.password,
        BRANDNAME: defaultConfig.oneSMS.brandName
      },
      stmpConfig: {
        SMTP_HOST: defaultConfig.email.smtp.host,
        SMTP_PORT: defaultConfig.email.smtp.port,
        SMTP_USERNAME: defaultConfig.email.smtp.auth.user,
        SMTP_PASSWORD: defaultConfig.email.smtp.auth.pass,
        EMAIL_FROM: defaultConfig.email.from,
        EMAIL_BRAND_NAME:  defaultConfig.email.brandName,
      }
    }
  }
  return config;
}


module.exports = {
  saveNotificationConfig,
  getNotificationConfigByTradingProviderId
};
